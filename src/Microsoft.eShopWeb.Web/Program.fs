module Microsoft.eShopWeb.Web.Program

open Domain
open Falco
open Falco.Routing
open Falco.HostBuilder
open Microsoft.AspNetCore.Builder
open System
open Falco.Markup

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

let htmlHandler: HttpHandler =
  let html =
    Elem.html
      [ Attr.lang "en" ]
      [ Elem.head [] []
        Elem.body [] [ Elem.h1 [] [ Text.raw "Welcome to the F# Shop!" ] ] ]

  Response.ofHtml html

[<EntryPoint>]
let main args =
  webHost args {
    use_if FalcoExtensions.IsDevelopment DeveloperExceptionPageExtensions.UseDeveloperExceptionPage
    use_ifnot FalcoExtensions.IsDevelopment (FalcoExtensions.UseFalcoExceptionHandler exceptionHandler)

    endpoints
      [ get "/" htmlHandler

        get "/catalogItems/{id:guid}" (Request.mapRoute getCatalogItemByIdFromRoute responseHandler) ]
  }

  0
