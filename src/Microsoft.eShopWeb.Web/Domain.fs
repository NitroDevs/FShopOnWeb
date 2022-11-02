namespace Microsoft.eShopWeb.Web

open System

module Domain =

  type CatalogBrand = { Id: int; Brand: string }

  type CatalogType = { Id: int; Type: string }

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

  let catalogBrand: CatalogBrand = { Id = 1; Brand = "Brand" }

  let catalogType: CatalogType = { Id = 1; Type = "Type" }

  let catalogItem1: CatalogItem =
    { Id = Guid.Parse("dd3334a9-a0da-4bd1-b819-b64e5036d614")
      Name = "Hoodie"
      Description = "Description"
      PictureUri = "/images/products/1.png"
      Price = 1.50M
      CatalogBrandId = 1
      CatalogTypeId = 1
      CatalogBrand = catalogBrand
      CatalogType = catalogType }

  let catalogItem2: CatalogItem =
    { Id = Guid.NewGuid()
      Name = "Mug"
      Description = "Description"
      PictureUri = "/images/products/2.png"
      Price = 1.0M
      CatalogBrandId = 1
      CatalogTypeId = 1
      CatalogBrand = catalogBrand
      CatalogType = catalogType }

  let catalogItem3: CatalogItem =
    { Id = Guid.NewGuid()
      Name = "T-Shirt"
      Description = "Description"
      PictureUri = "/images/products/3.png"
      Price = 1.0M
      CatalogBrandId = 1
      CatalogTypeId = 1
      CatalogBrand = catalogBrand
      CatalogType = catalogType }

  let catalogItems = [ catalogItem1; catalogItem2; catalogItem3 ]
