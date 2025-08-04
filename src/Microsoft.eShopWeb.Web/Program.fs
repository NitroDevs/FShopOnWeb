namespace Microsoft.eShopWeb.Web

open Falco
open Falco.Routing
open Falco.HostBuilder
open Microsoft.AspNetCore.Builder
open EntityFrameworkCore.FSharp.Extensions
open Microsoft.Extensions.DependencyInjection
open Microsoft.eShopWeb.Web.Persistence
open Microsoft.eShopWeb.Web.Home
open Microsoft.eShopWeb.Web.Basket
open Microsoft.EntityFrameworkCore

module Program =
  open Microsoft.eShopWeb.Web.Account.Login

  // let getById =
  //   fun (repository) -> fun (id: Guid) -> repository |> List.tryFind (fun x -> x.Id = id)

  // let getCatalogItemByIdFromRoute =
  //   fun (route: RouteCollectionReader) ->
  //     let getCatalogItem = getById catalogItems
  //     route.TryGetGuid "id" |> Option.bind getCatalogItem

  let notFoundHandler: HttpHandler =
    Response.withStatusCode 404 >> Response.ofPlainText "Not found"

  let responseHandler =
    fun (value) ->
      match value with
      | Some x -> Response.ofJson x
      | None -> notFoundHandler

  // ------------
  // Exception Handler
  // ------------
  let exceptionHandler: HttpHandler =
    Response.withStatusCode 500 >> Response.ofPlainText "Server error"

  let ensureDatabaseCreatedAndReset (builder: IApplicationBuilder) =
    use serviceScope = builder.ApplicationServices.CreateScope()
    let db = serviceScope.ServiceProvider.GetRequiredService<ShopContext>()
    printfn "Records inserted: %d " (initializeDb db)
    builder


  [<EntryPoint>]
  let main args =

    // This webHost computation expression gives us access to different hooks into the WebApplicationHost (eg DI and middleware)
    // https://github.com/pimbrouwers/Falco/issues/14#issue-603574072
    webHost args {
      add_antiforgery

      add_service (fun services ->
        services.AddDbContext<ShopContext>(fun options ->
          options.UseSqlite("Data Source=\".\\App_Data\\FShopOnWeb.sqlite\"") |> ignore
          options.UseFSharpTypes() |> ignore))

      use_if FalcoExtensions.IsDevelopment ensureDatabaseCreatedAndReset
      use_if FalcoExtensions.IsDevelopment DeveloperExceptionPageExtensions.UseDeveloperExceptionPage
      use_ifnot FalcoExtensions.IsDevelopment (FalcoExtensions.UseFalcoExceptionHandler exceptionHandler)
      use_static_files

      endpoints
        [ get "/" HomePage.handler

          get "/basket" BasketPage.get
          post "/basket" BasketPage.post
          post "/basket/remove" BasketPage.remove
          post "/basket/updateQuantity" BasketPage.updateQuantity

          get "/identity/account/login" LoginPage.handler

          //get "/catalogItems/{id:guid}" (Request.mapRoute getCatalogItemByIdFromRoute responseHandler)
          ]
    }

    0
