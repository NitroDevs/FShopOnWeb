namespace Microsoft.eShopWeb.Web

open System
open System.ComponentModel.DataAnnotations

module Domain =

  [<CLIMutable>]
  type CatalogBrand =
    { [<Key>]
      Id: int
      Name: string }

  [<CLIMutable>]
  type CatalogType =
    { [<Key>]
      Id: int
      Name: string }

  [<CLIMutable>]
  type CatalogItem =
    { [<Key>]
      Id: Guid
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
    { [<Key>]
      Id: int
      CatalogItemId: Guid
      ProductName: string
      UnitPrice: decimal
      OldUnitPrice: decimal
      mutable Quantity: int
      PictureUri: string
      BasketId: Guid }

  [<CLIMutable>]
  type Basket =
    { [<Key>]
      Id: Guid
      Items: BasketItem seq
      BuyerId: Nullable<Guid> }

  let emptyBasket =
    { Id = Unchecked.defaultof<Guid>
      Items = []
      BuyerId = Nullable() }
