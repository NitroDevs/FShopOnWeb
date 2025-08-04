module UpdateItemQuantity

open System
open System.Linq
open Xunit
open Microsoft.EntityFrameworkCore
open Microsoft.eShopWeb.Web
open Microsoft.eShopWeb.Web.Domain
open Microsoft.eShopWeb.Web.Persistence
open Microsoft.eShopWeb.Web.Basket.BasketDomain
open EntityFrameworkCore.FSharp.DbContextHelpers

// Helper function to create an in-memory database context
let createInMemoryContext () =
    let options = DbContextOptionsBuilder<ShopContext>()
                    .UseInMemoryDatabase(databaseName = Guid.NewGuid().ToString())
                    .Options
    new ShopContext(options)

// Helper function to seed test data
let seedBasketItemForUpdate (context: ShopContext) quantity =
    let basketId = Unchecked.defaultof<Guid>
    let catalogItemId = Guid.NewGuid()
    
    let basketItem = {
        Id = 0
        CatalogItemId = catalogItemId
        ProductName = "Test Product"
        UnitPrice = 10.0M
        OldUnitPrice = 10.0M
        Quantity = quantity
        PictureUri = "/test.png"
        BasketId = basketId
    }
    
    context.BasketItems.Add(basketItem) |> ignore
    context.SaveChanges() |> ignore
    
    (catalogItemId, basketId)

[<Fact>]
let ``updateItemQuantity should return Some newQuantity when item exists and quantity is valid`` () =
    async {
        // Arrange
        use context = createInMemoryContext()
        context.Database.EnsureCreated() |> ignore
        let (catalogItemId, basketId) = seedBasketItemForUpdate context 2
        let newQuantity = 5
        
        // Act
        let! result = updateItemQuantity context catalogItemId newQuantity
        
        // Assert
        Assert.Equal(Some newQuantity, result)
        
        // Verify item quantity was updated in database
        let updatedItem = context.BasketItems.Where(fun bi -> bi.CatalogItemId = catalogItemId && bi.BasketId = basketId)
                         |> Seq.tryHead
        match updatedItem with
        | Some item -> Assert.Equal(newQuantity, item.Quantity)
        | None -> Assert.True(false, "Item should exist after update")
    }

[<Fact>]
let ``updateItemQuantity should return None when quantity is zero`` () =
    async {
        // Arrange
        use context = createInMemoryContext()
        context.Database.EnsureCreated() |> ignore
        let (catalogItemId, _) = seedBasketItemForUpdate context 2
        let invalidQuantity = 0
        
        // Act
        let! result = updateItemQuantity context catalogItemId invalidQuantity
        
        // Assert
        Assert.Equal(None, result)
    }

[<Fact>]
let ``updateItemQuantity should return None when quantity is negative`` () =
    async {
        // Arrange
        use context = createInMemoryContext()
        context.Database.EnsureCreated() |> ignore
        let (catalogItemId, _) = seedBasketItemForUpdate context 2
        let invalidQuantity = -1
        
        // Act
        let! result = updateItemQuantity context catalogItemId invalidQuantity
        
        // Assert
        Assert.Equal(None, result)
    }

[<Fact>]
let ``updateItemQuantity should return None when item does not exist`` () =
    async {
        // Arrange
        use context = createInMemoryContext()
        context.Database.EnsureCreated() |> ignore
        let nonExistentItemId = Guid.NewGuid()
        let validQuantity = 3
        
        // Act
        let! result = updateItemQuantity context nonExistentItemId validQuantity
        
        // Assert
        Assert.Equal(None, result)
    }

[<Fact>]
let ``updateItemQuantity should leave other items unchanged`` () =
    async {
        // Arrange
        use context = createInMemoryContext()
        context.Database.EnsureCreated() |> ignore
        
        let basketId = Unchecked.defaultof<Guid>
        let catalogItemId1 = Guid.NewGuid()
        let catalogItemId2 = Guid.NewGuid()
        
        let basketItem1 = {
            Id = 0
            CatalogItemId = catalogItemId1
            ProductName = "Test Product 1"
            UnitPrice = 10.0M
            OldUnitPrice = 10.0M
            Quantity = 2
            PictureUri = "/test1.png"
            BasketId = basketId
        }
        
        let basketItem2 = {
            Id = 0
            CatalogItemId = catalogItemId2
            ProductName = "Test Product 2"
            UnitPrice = 20.0M
            OldUnitPrice = 20.0M
            Quantity = 3
            PictureUri = "/test2.png"
            BasketId = basketId
        }
        
        context.BasketItems.AddRange([basketItem1; basketItem2]) |> ignore
        context.SaveChanges() |> ignore
        
        let newQuantity = 5
        
        // Act - update first item
        let! result = updateItemQuantity context catalogItemId1 newQuantity
        
        // Assert
        Assert.Equal(Some newQuantity, result)
        
        // Verify first item was updated
        let updatedItem1 = context.BasketItems.Where(fun bi -> bi.CatalogItemId = catalogItemId1)
                          |> Seq.tryHead
        match updatedItem1 with
        | Some item -> Assert.Equal(newQuantity, item.Quantity)
        | None -> Assert.True(false, "First item should exist after update")
        
        // Verify second item was unchanged
        let unchangedItem2 = context.BasketItems.Where(fun bi -> bi.CatalogItemId = catalogItemId2)
                            |> Seq.tryHead
        match unchangedItem2 with
        | Some item -> Assert.Equal(3, item.Quantity)
        | None -> Assert.True(false, "Second item should still exist")
    }

[<Fact>]
let ``updateItemQuantity should handle large valid quantities`` () =
    async {
        // Arrange
        use context = createInMemoryContext()
        context.Database.EnsureCreated() |> ignore
        let (catalogItemId, basketId) = seedBasketItemForUpdate context 1
        let largeQuantity = 100
        
        // Act
        let! result = updateItemQuantity context catalogItemId largeQuantity
        
        // Assert
        Assert.Equal(Some largeQuantity, result)
        
        // Verify item quantity was updated in database
        let updatedItem = context.BasketItems.Where(fun bi -> bi.CatalogItemId = catalogItemId && bi.BasketId = basketId)
                         |> Seq.tryHead
        match updatedItem with
        | Some item -> Assert.Equal(largeQuantity, item.Quantity)
        | None -> Assert.True(false, "Item should exist after update")
    }