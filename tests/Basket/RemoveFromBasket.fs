module RemoveFromBasket

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
        Quantity = 1
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
let ``removeFromBasket should return Some catalogItemId when item exists`` () =
    async {
        // Arrange
        use context = createInMemoryContext()
        context.Database.EnsureCreated() |> ignore
        let (catalogItemId, basketId) = seedTestData context
        
        // Act
        let! result = removeFromBasket context catalogItemId
        
        // Assert
        Assert.Equal(Some catalogItemId, result)
        
        // Verify item was removed from database
        let remainingItems = context.BasketItems.Where(fun bi -> bi.CatalogItemId = catalogItemId && bi.BasketId = basketId)
                            |> Seq.toList
        Assert.Empty(remainingItems)
    }

[<Fact>]
let ``removeFromBasket should return None when item does not exist`` () =
    async {
        // Arrange
        use context = createInMemoryContext()
        context.Database.EnsureCreated() |> ignore
        let (_, _) = seedTestData context
        let nonExistentItemId = Guid.NewGuid()
        
        // Act
        let! result = removeFromBasket context nonExistentItemId
        
        // Assert
        Assert.Equal(None, result)
    }

[<Fact>]
let ``removeFromBasket should return None when basket is empty`` () =
    async {
        // Arrange
        use context = createInMemoryContext()
        context.Database.EnsureCreated() |> ignore
        let itemId = Guid.NewGuid()
        
        // Act
        let! result = removeFromBasket context itemId
        
        // Assert
        Assert.Equal(None, result)
    }

[<Fact>]
let ``removeFromBasket should only remove specified item from basket with multiple items`` () =
    async {
        // Arrange
        use context = createInMemoryContext()
        context.Database.EnsureCreated() |> ignore
        
        let basketId = Guid.NewGuid()
        let catalogItemId1 = Guid.NewGuid()
        let catalogItemId2 = Guid.NewGuid()
        
        let basketItem1 = {
            Id = 0
            CatalogItemId = catalogItemId1
            ProductName = "Test Product 1"
            UnitPrice = 10.0M
            OldUnitPrice = 10.0M
            Quantity = 1
            PictureUri = "/test1.png"
            BasketId = Unchecked.defaultof<Guid>
        }
        
        let basketItem2 = {
            Id = 0
            CatalogItemId = catalogItemId2
            ProductName = "Test Product 2"
            UnitPrice = 20.0M
            OldUnitPrice = 20.0M
            Quantity = 2
            PictureUri = "/test2.png"
            BasketId = Unchecked.defaultof<Guid>
        }
        
        // Add a minimal basket directly without navigation properties
        context.BasketItems.AddRange([basketItem1; basketItem2]) |> ignore
        context.SaveChanges() |> ignore
        
        // Act - remove first item
        let! result = removeFromBasket context catalogItemId1
        
        // Assert
        Assert.Equal(Some catalogItemId1, result)
        
        // Verify only the specified item was removed
        let remainingItems = context.BasketItems.Where(fun bi -> bi.BasketId = Unchecked.defaultof<Guid>)
                            |> Seq.toList
        Assert.Single(remainingItems) |> ignore
        Assert.Equal(catalogItemId2, remainingItems.[0].CatalogItemId)
    }

[<Fact>]
let ``removeFromBasket should handle basket with no items gracefully`` () =
    async {
        // Arrange
        use context = createInMemoryContext()
        context.Database.EnsureCreated() |> ignore
        
        let basketId = Guid.NewGuid()
        
        let itemId = Guid.NewGuid()
        
        // No need to create any basket items since we're testing empty basket behavior
        
        // Act
        let! result = removeFromBasket context itemId
        
        // Assert
        Assert.Equal(None, result)
    }
