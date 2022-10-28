namespace Microsoft.eShopWeb.Web

module Program =

  open Falco
  open Falco.Routing
  open Falco.HostBuilder
  open Microsoft.AspNetCore.Builder
  open System
  open Microsoft.eShopWeb.Web.Domain
  open Microsoft.eShopWeb.Web.Home

  let getById =
    fun (repository) -> fun (id: Guid) -> repository |> List.tryFind (fun x -> x.Id = id)

  let getCatalogItemByIdFromRoute =
    fun (route: RouteCollectionReader) ->
      let getCatalogItem = getById catalogItems
      route.TryGetGuid "id" |> Option.bind getCatalogItem

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

  [<EntryPoint>]
  let main args =
    webHost args {
      use_if FalcoExtensions.IsDevelopment DeveloperExceptionPageExtensions.UseDeveloperExceptionPage
      use_ifnot FalcoExtensions.IsDevelopment (FalcoExtensions.UseFalcoExceptionHandler exceptionHandler)
      use_static_files

      endpoints
        [ get "/" HomePage.handler

          get "/catalogItems/{id:guid}" (Request.mapRoute getCatalogItemByIdFromRoute responseHandler) ]
    }

    0
