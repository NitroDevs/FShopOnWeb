module Domain

open System

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
      Name = "Name"
      Description = "Description"
      PictureUri = "PictureUri"
      Price = 1.0M
      CatalogBrandId = 1
      CatalogTypeId = 1
      CatalogBrand = catalogBrand
      CatalogType = catalogType }

let catalogItem2: CatalogItem =
    { Id = Guid.NewGuid()
      Name = "Name"
      Description = "Description"
      PictureUri = "PictureUri"
      Price = 1.0M
      CatalogBrandId = 1
      CatalogTypeId = 1
      CatalogBrand = catalogBrand
      CatalogType = catalogType }

let catalogItems = [ catalogItem1; catalogItem2 ]
