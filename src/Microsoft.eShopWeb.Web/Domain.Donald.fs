namespace Microsoft.eShopWeb.Web

module Domain_Donald =
  open System

  type CatalogBrand =
    { Id: int
      Name: string }

  type CatalogType =
    { Id: int
      Name: string }

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

  type BasketItem =
    { Id: int
      CatalogItemId: Guid
      ProductName: string
      UnitPrice: decimal
      OldUnitPrice: decimal
      Quantity: int
      PictureUri: string
      BasketId: Guid }

  type Basket =
    { Id: Guid
      Items: BasketItem list
      BuyerId: Guid option }
