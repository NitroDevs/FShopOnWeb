namespace Microsoft.eShopWeb.Web.Basket

open System
open Microsoft.eShopWeb.Web
open Microsoft.eShopWeb.Web.Domain
open Microsoft.eShopWeb.Web.Persistence
open System.Linq
open Microsoft.EntityFrameworkCore
open EntityFrameworkCore.FSharp.DbContextHelpers
open Microsoft.FSharp.Core.Option

module BasketDomain =

  let basketTotal basket =
    let sum =
      Seq.sumBy (fun i -> i.UnitPrice * Convert.ToDecimal i.Quantity) basket.Items

    Math.Round(sum, 2)

  let mapCatalogItemToBasketItem basketId (catalogItem: CatalogItem) : BasketItem =
    { Id = 0
      CatalogItemId = catalogItem.Id
      ProductName = catalogItem.Name
      UnitPrice = catalogItem.Price
      OldUnitPrice = catalogItem.Price
      Quantity = 1
      BasketId = basketId
      PictureUri = catalogItem.PictureUri }

  let productFallbackImageUri = "/images/brand.png"


  let getCatalogItem (db: ShopContext) productId =
    async {
      let! items = db.CatalogItems.Where(fun ci -> ci.Id = productId) |> toListAsync

      return items |> List.tryHead
    }


  let addItemToBasket quantity basket (catalogItem: CatalogItem) =
    let mapBasketItems = mapCatalogItemToBasketItem basket.Id

    let item: BasketItem option =
      basket.Items |> Seq.tryFind (fun i -> i.CatalogItemId = catalogItem.Id)

    match item with
    | None ->
      let items = basket.Items |> Seq.append [ (mapBasketItems catalogItem) ]

      { basket with Items = items.ToList() }
    | Some item ->

      basket.Items
      |> Seq.iter (fun i ->
        if i.CatalogItemId = catalogItem.Id then
          i.Quantity <- item.Quantity + quantity)
      |> ignore
      basket



  let updateBasket (db: ShopContext) (quantity: int) productId =
    async {
      let! catalogItem =
        match productId with
        | Some pId -> pId |> getCatalogItem db
        | None -> async { return None }

      let! existingBasket =
        (db.Baskets.Include(fun b -> b.Items).OrderBy(fun b -> b.Id)) |> tryFirstAsync

      let basket = existingBasket |> defaultValue emptyBasket

      let updatedBasket =
        catalogItem
        |> map (basket |> addItemToBasket quantity)
        |> defaultValue basket

      try
        match updatedBasket.Id with
        | id when id = Guid.Empty -> db.Baskets.Add updatedBasket |> ignore
        | _ -> ()

        Seq.iter (
          fun (i: BasketItem) ->
            match i.Id with
            | 0 ->
              db.BasketItems.Add i |> ignore
            | _ -> ())
          updatedBasket.Items

        do! saveChangesAsync' db |> Async.Ignore
        return Some quantity
      with exp ->
        match productId with
        | Some id -> printfn $"Error updating basket for Product {id}"; printfn $"{exp}"
        | None -> printfn "No product specified to be added to basket"
        return None
    }

  let updateBasketItemQuantity (db: ShopContext) catalogItemId newQuantity =
    async {
      let! existingBasket =
        (db.Baskets.Include(fun b -> b.Items).OrderBy(fun b -> b.Id)) |> tryFirstAsync

      let basket = existingBasket |> defaultValue emptyBasket

      try
        // Find the basket item to update
        let itemToUpdate = 
          db.BasketItems.Where(fun bi -> bi.CatalogItemId = catalogItemId && bi.BasketId = basket.Id)
          |> Seq.tryHead
        
        match itemToUpdate with
        | Some item -> 
            if newQuantity > 0 then
              item.Quantity <- newQuantity
              do! saveChangesAsync' db |> Async.Ignore
              return Some newQuantity
            else
              // Remove the item if quantity becomes 0 or negative
              db.BasketItems.Remove(item) |> ignore
              do! saveChangesAsync' db |> Async.Ignore
              return Some 0
        | None -> 
            return None
      with exp ->
        printfn $"Error updating quantity for item {catalogItemId} in basket"; printfn $"{exp}"
        return None
    }

  let removeFromBasket (db: ShopContext) catalogItemId =
    async {
      let! existingBasket =
        (db.Baskets.Include(fun b -> b.Items).OrderBy(fun b -> b.Id)) |> tryFirstAsync

      let basket = existingBasket |> defaultValue emptyBasket

      try
        // Remove the basket item from the database
        let itemToRemove = 
          db.BasketItems.Where(fun bi -> bi.CatalogItemId = catalogItemId && bi.BasketId = basket.Id)
          |> Seq.tryHead
        
        match itemToRemove with
        | Some item -> 
            db.BasketItems.Remove(item) |> ignore
            do! saveChangesAsync' db |> Async.Ignore
            return Some catalogItemId
        | None -> 
            return None
      with exp ->
        printfn $"Error removing item {catalogItemId} from basket"; printfn $"{exp}"
        return None
    }
