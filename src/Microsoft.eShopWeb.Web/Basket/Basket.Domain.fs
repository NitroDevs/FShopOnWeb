namespace Microsoft.eShopWeb.Web.Basket

open System
open Microsoft.eShopWeb.Web.Domain

module BasketDomain =

  type BasketItem =
    { Id: int
      CatalogItemId: Guid
      ProductName: string
      UnitPrice: decimal
      OldUnitPrice: decimal
      Quantity: int
      PictureUri: string option }

  type Basket =
    { Id: int
      Items: BasketItem list
      BuyerId: string option }

    member this.Total() =
      let sum =
        List.sumBy (fun i -> i.UnitPrice * Convert.ToDecimal i.Quantity) this.Items

      Math.Round(sum, 2)

  let mapCatalogItem id (catalogItem: CatalogItem) : BasketItem =
    { Id = id
      CatalogItemId = catalogItem.Id
      ProductName = catalogItem.Name
      UnitPrice = catalogItem.Price
      OldUnitPrice = catalogItem.Price
      Quantity = 1
      PictureUri = Some catalogItem.PictureUri }

  let productFallbackImageUri = "/images/brand.png"

  let basket =
    { Id = 1
      Items = List.mapi mapCatalogItem catalogItems
      BuyerId = None }

  let addItemToBasket (catalogItem: CatalogItem) =
    // let id = form.TryGetString "id" |> Option.map int
    let item: BasketItem option =
      basket.Items |> List.tryFind (fun i -> i.CatalogItemId = catalogItem.Id)

    match item with
    | None ->
      let items =
        basket.Items
        |> List.append [ (mapCatalogItem (basket.Items.Length + 1) catalogItem) ]

      { basket with Items = items }
    | Some item ->
      let quantity = item.Quantity + 1

      let items =
        basket.Items
        |> List.map (fun i ->
          if i.CatalogItemId = catalogItem.Id then
            { i with Quantity = quantity }
          else
            i)

      { basket with Items = items }
