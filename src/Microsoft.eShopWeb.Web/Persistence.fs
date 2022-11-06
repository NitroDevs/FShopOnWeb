namespace Microsoft.eShopWeb.Web

open Microsoft.EntityFrameworkCore
open Domain

module Persistence =
  open System

  type ShopContext(options: DbContextOptions<ShopContext>) =
    inherit DbContext(options)

    [<DefaultValue>]
    val mutable private _catalogItems: DbSet<CatalogItem>

    member this.CatalogItems
      with get () = this._catalogItems
      and set v = this._catalogItems <- v

  module Seeding =
    type CatalogBrands =
      { DotNet: CatalogBrand
        Azure: CatalogBrand
        Other: CatalogBrand
        SQL_Server: CatalogBrand
        Visual_Studio: CatalogBrand }

    let brands =
      { DotNet = { Id = 1; Name = ".NET" }
        Azure = { Id = 2; Name = "Azure" }
        Other = { Id = 3; Name = "Other" }
        SQL_Server = { Id = 4; Name = "SQL Server" }
        Visual_Studio = { Id = 5; Name = "Visual Studio" } }

    type CatalogTypes =
      { Mug: CatalogType
        Sheet: CatalogType
        T_Shirt: CatalogType
        USB_Memory_Stick: CatalogType
        Hoodie: CatalogType }

    let types =
      { Mug = { Id = 1; Name = "Mug" }
        Sheet = { Id = 2; Name = "Sheet" }
        T_Shirt = { Id = 3; Name = "T-Shirt" }
        USB_Memory_Stick = { Id = 4; Name = "USB Memory Stick" }
        Hoodie = { Id = 5; Name = "Hoodie" } }

    let catalogItem1 =
      { Id = Unchecked.defaultof<Guid>
        Name = "Hoodie"
        Description = "A nice hoodie"
        PictureUri = "/images/products/1.png"
        Price = 40.50M
        CatalogBrandId = 0
        CatalogTypeId = 0
        CatalogBrand = brands.DotNet
        CatalogType = types.Hoodie }

    let catalogItem2 =
      { Id = Unchecked.defaultof<Guid>
        Name = "Mug"
        Description = "Description"
        PictureUri = "/images/products/2.png"
        Price = 7.25M
        CatalogBrandId = 0
        CatalogTypeId = 0
        CatalogBrand = brands.DotNet
        CatalogType = types.Mug }

    let catalogItem3 =
      { Id = Unchecked.defaultof<Guid>
        Name = "T-Shirt"
        Description = "Description"
        PictureUri = "/images/products/3.png"
        Price = 20.0M
        CatalogBrandId = 0
        CatalogTypeId = 0
        CatalogBrand = brands.Azure
        CatalogType = types.T_Shirt }

    let catalogItems = [ catalogItem1; catalogItem2; catalogItem3 ]

  let initializeDb (context: ShopContext) =
    context.Database.EnsureDeleted() |> ignore
    context.Database.EnsureCreated() |> ignore
    context.CatalogItems.AddRange Seeding.catalogItems
    context.SaveChanges()
