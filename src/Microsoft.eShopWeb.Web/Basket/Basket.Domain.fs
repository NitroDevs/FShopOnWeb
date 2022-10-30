namespace Microsoft.eShopWeb.Web.Basket

open System
open Microsoft.eShopWeb.Web.Domain

module Domain =

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
