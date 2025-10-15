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
    let catalogItemId = Guid.NewGuid()
    
    let basketItem = {
        Id = 0
        CatalogItemId = catalogItemId
        ProductName = "Test Product"
        UnitPrice = 10.0M
        OldUnitPrice = 10.0M
        Quantity = 5
        PictureUri = "/test.png"
        BasketId = Unchecked.defaultof<Guid>
    }
    
    context.BasketItems.Add(basketItem) |> ignore
    context.SaveChanges() |> ignore
    
    catalogItemId

[<Fact>]
let ``updateBasketItemQuantity should return Ok when updating to valid quantity`` () =
    async {
        // Arrange
        use context = createInMemoryContext()
        context.Database.EnsureCreated() |> ignore
        let catalogItemId = seedTestData context
        let newQuantity = 10
        
        // Act
        let! result = updateBasketItemQuantity context catalogItemId newQuantity
        
        // Assert
        match result with
        | Ok qty -> 
            Assert.Equal(newQuantity, qty)
            
            // Verify the quantity was actually updated in the database
            let updatedItem = context.BasketItems.FirstOrDefault(fun bi -> bi.CatalogItemId = catalogItemId)
            Assert.NotNull(updatedItem)
            Assert.Equal(newQuantity, updatedItem.Quantity)
        | Error msg -> 
            Assert.True(false, $"Expected Ok but got Error: {msg}")
    }

[<Fact>]
let ``updateBasketItemQuantity should return Error when quantity is less than 1`` () =
    async {
        // Arrange
        use context = createInMemoryContext()
        context.Database.EnsureCreated() |> ignore
        let catalogItemId = seedTestData context
        let invalidQuantity = 0
        
        // Act
        let! result = updateBasketItemQuantity context catalogItemId invalidQuantity
        
        // Assert
        match result with
        | Ok _ -> Assert.True(false, "Expected Error but got Ok")
        | Error msg -> Assert.Equal("Quantity must be at least 1", msg)
    }

[<Fact>]
let ``updateBasketItemQuantity should return Error when quantity is negative`` () =
    async {
        // Arrange
        use context = createInMemoryContext()
        context.Database.EnsureCreated() |> ignore
        let catalogItemId = seedTestData context
        let invalidQuantity = -5
        
        // Act
        let! result = updateBasketItemQuantity context catalogItemId invalidQuantity
        
        // Assert
        match result with
        | Ok _ -> Assert.True(false, "Expected Error but got Ok")
        | Error msg -> Assert.Equal("Quantity must be at least 1", msg)
    }

[<Fact>]
let ``updateBasketItemQuantity should return Error when item does not exist`` () =
    async {
        // Arrange
        use context = createInMemoryContext()
        context.Database.EnsureCreated() |> ignore
        let _ = seedTestData context
        let nonExistentItemId = Guid.NewGuid()
        let newQuantity = 5
        
        // Act
        let! result = updateBasketItemQuantity context nonExistentItemId newQuantity
        
        // Assert
        match result with
        | Ok _ -> Assert.True(false, "Expected Error but got Ok")
        | Error msg -> Assert.Equal("Item not found in basket", msg)
    }

[<Fact>]
let ``updateBasketItemQuantity should update quantity to 1`` () =
    async {
        // Arrange
        use context = createInMemoryContext()
        context.Database.EnsureCreated() |> ignore
        let catalogItemId = seedTestData context
        let newQuantity = 1
        
        // Act
        let! result = updateBasketItemQuantity context catalogItemId newQuantity
        
        // Assert
        match result with
        | Ok qty -> 
            Assert.Equal(newQuantity, qty)
            
            // Verify the quantity was actually updated in the database
            let updatedItem = context.BasketItems.FirstOrDefault(fun bi -> bi.CatalogItemId = catalogItemId)
            Assert.NotNull(updatedItem)
            Assert.Equal(1, updatedItem.Quantity)
        | Error msg -> 
            Assert.True(false, $"Expected Ok but got Error: {msg}")
    }

[<Fact>]
let ``updateBasketItemQuantity should handle large quantities`` () =
    async {
        // Arrange
        use context = createInMemoryContext()
        context.Database.EnsureCreated() |> ignore
        let catalogItemId = seedTestData context
        let largeQuantity = 1000
        
        // Act
        let! result = updateBasketItemQuantity context catalogItemId largeQuantity
        
        // Assert
        match result with
        | Ok qty -> 
            Assert.Equal(largeQuantity, qty)
            
            // Verify the quantity was actually updated in the database
            let updatedItem = context.BasketItems.FirstOrDefault(fun bi -> bi.CatalogItemId = catalogItemId)
            Assert.NotNull(updatedItem)
            Assert.Equal(largeQuantity, updatedItem.Quantity)
        | Error msg -> 
            Assert.True(false, $"Expected Ok but got Error: {msg}")
    }

[<Fact>]
let ``updateBasketItemQuantity should not affect other basket items`` () =
    async {
        // Arrange
        use context = createInMemoryContext()
        context.Database.EnsureCreated() |> ignore
        
        let catalogItemId1 = Guid.NewGuid()
        let catalogItemId2 = Guid.NewGuid()
        
        let basketItem1 = {
            Id = 0
            CatalogItemId = catalogItemId1
            ProductName = "Test Product 1"
            UnitPrice = 10.0M
            OldUnitPrice = 10.0M
            Quantity = 5
            PictureUri = "/test1.png"
            BasketId = Unchecked.defaultof<Guid>
        }
        
        let basketItem2 = {
            Id = 0
            CatalogItemId = catalogItemId2
            ProductName = "Test Product 2"
            UnitPrice = 20.0M
            OldUnitPrice = 20.0M
            Quantity = 3
            PictureUri = "/test2.png"
            BasketId = Unchecked.defaultof<Guid>
        }
        
        context.BasketItems.AddRange([basketItem1; basketItem2]) |> ignore
        context.SaveChanges() |> ignore
        
        let newQuantity = 10
        
        // Act - update only first item
        let! result = updateBasketItemQuantity context catalogItemId1 newQuantity
        
        // Assert
        match result with
        | Ok _ -> 
            // Verify first item was updated
            let updatedItem1 = context.BasketItems.FirstOrDefault(fun bi -> bi.CatalogItemId = catalogItemId1)
            Assert.Equal(newQuantity, updatedItem1.Quantity)
            
            // Verify second item was not affected
            let unchangedItem2 = context.BasketItems.FirstOrDefault(fun bi -> bi.CatalogItemId = catalogItemId2)
            Assert.Equal(3, unchangedItem2.Quantity)
        | Error msg -> 
            Assert.True(false, $"Expected Ok but got Error: {msg}")
    }
