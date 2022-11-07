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
