namespace Microsoft.eShopWeb.Web

module Domain_Dapper =
  open System

  [<CLIMutable>]
  type CatalogBrand =
    { Id: int
      Name: string }

  [<CLIMutable>]
  type CatalogType =
    { Id: int
      Name: string }

  [<CLIMutable>]
  type CatalogItem =
    { Id: Guid
      Name: string
      Description: string
      PictureUri: string
      Price: decimal
      CatalogBrandId: int
      CatalogTypeId: int
      CatalogBrand: CatalogBrand
      CatalogType: CatalogType }

  [<CLIMutable>]
  type BasketItem =
    { Id: int
      CatalogItemId: Guid
      ProductName: string
      UnitPrice: decimal
      OldUnitPrice: decimal
      mutable Quantity: int
      PictureUri: string
      BasketId: Guid }

  [<CLIMutable>]
  type Basket =
    { Id: Guid
      Items: BasketItem seq
      BuyerId: Guid option }

  let emptyBasket =
    { Id = Unchecked.defaultof<Guid>
      Items = []
      BuyerId = None }
