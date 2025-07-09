module UpdateBasketItemQuantity

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
let seedTestData (context: ShopContext) =
    let basketId = Guid.NewGuid()
    let catalogItemId = Guid.NewGuid()
    
    // Create a basket entry first to ensure the basket exists
    let basketItem = {
        Id = 0
        CatalogItemId = catalogItemId
        ProductName = "Test Product"
        UnitPrice = 10.0M
        OldUnitPrice = 10.0M
        Quantity = 2
        PictureUri = "/test.png"
        BasketId = basketId
    }
    
    // Create the basket by directly executing insert (without navigation property issues)
    // Since we can't use SQL with in-memory, we need to create a minimal basket
    // We'll work around this by using the emptyBasket approach the function expects
    
    // First add the basket item with emptyBasket.Id
    let basketItemWithEmptyBasketId = { basketItem with BasketId = Unchecked.defaultof<Guid> }
    context.BasketItems.Add(basketItemWithEmptyBasketId) |> ignore
    context.SaveChanges() |> ignore
    
    (catalogItemId, Unchecked.defaultof<Guid>)

[<Fact>]
let ``updateBasketItemQuantity should update quantity when item exists and quantity is positive`` () =
    async {
        // Arrange
        use context = createInMemoryContext()
        context.Database.EnsureCreated() |> ignore
        let (catalogItemId, basketId) = seedTestData context
        let newQuantity = 5
        
        // Act
        let! result = updateBasketItemQuantity context catalogItemId newQuantity
        
        // Assert
        Assert.Equal(Some newQuantity, result)
        
        // Verify item quantity was updated in database
        let updatedItem = context.BasketItems.Where(fun bi -> bi.CatalogItemId = catalogItemId && bi.BasketId = basketId)
                         |> Seq.tryHead
        match updatedItem with
        | Some item -> Assert.Equal(newQuantity, item.Quantity)
        | None -> Assert.True(false, "Item should exist in database")
    }

[<Fact>]
let ``updateBasketItemQuantity should remove item when quantity is zero`` () =
    async {
        // Arrange
        use context = createInMemoryContext()
        context.Database.EnsureCreated() |> ignore
        let (catalogItemId, basketId) = seedTestData context
        let newQuantity = 0
        
        // Act
        let! result = updateBasketItemQuantity context catalogItemId newQuantity
        
        // Assert
        Assert.Equal(Some 0, result)
        
        // Verify item was removed from database
        let remainingItems = context.BasketItems.Where(fun bi -> bi.CatalogItemId = catalogItemId && bi.BasketId = basketId)
                            |> Seq.toList
        Assert.Empty(remainingItems)
    }

[<Fact>]
let ``updateBasketItemQuantity should remove item when quantity is negative`` () =
    async {
        // Arrange
        use context = createInMemoryContext()
        context.Database.EnsureCreated() |> ignore
        let (catalogItemId, basketId) = seedTestData context
        let newQuantity = -1
        
        // Act
        let! result = updateBasketItemQuantity context catalogItemId newQuantity
        
        // Assert
        Assert.Equal(Some 0, result)
        
        // Verify item was removed from database
        let remainingItems = context.BasketItems.Where(fun bi -> bi.CatalogItemId = catalogItemId && bi.BasketId = basketId)
                            |> Seq.toList
        Assert.Empty(remainingItems)
    }

[<Fact>]
let ``updateBasketItemQuantity should return None when item does not exist`` () =
    async {
        // Arrange
        use context = createInMemoryContext()
        context.Database.EnsureCreated() |> ignore
        let nonExistentItemId = Guid.NewGuid()
        let newQuantity = 5
        
        // Act
        let! result = updateBasketItemQuantity context nonExistentItemId newQuantity
        
        // Assert
        Assert.Equal(None, result)
    }

[<Fact>]
let ``updateBasketItemQuantity should handle empty basket gracefully`` () =
    async {
        // Arrange
        use context = createInMemoryContext()
        context.Database.EnsureCreated() |> ignore
        let itemId = Guid.NewGuid()
        let newQuantity = 3
        
        // Act
        let! result = updateBasketItemQuantity context itemId newQuantity
        
        // Assert
        Assert.Equal(None, result)
    }