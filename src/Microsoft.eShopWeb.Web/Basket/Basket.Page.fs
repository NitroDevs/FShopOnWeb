namespace Microsoft.eShopWeb.Web.Basket

open Falco
open Falco.Markup.Elem
open Microsoft.eShopWeb.Web.Persistence
open Microsoft.eShopWeb.Web
open EntityFrameworkCore.FSharp.DbContextHelpers
open Microsoft.eShopWeb.Web.Domain
open Microsoft.FSharp.Core.Option
open System.Linq
open Microsoft.EntityFrameworkCore

module BasketPage =

  module private Template =

    let metadata: PublicLayout.HeadMetadata = { Title = "Basket"; Description = "" }

    let head = PublicLayout.head metadata

    let body basket =
      PublicLayout.body [ div [ Markup.Attr.class' "container" ] (BasketComponent.cmpt basket) ] basket

    let page basket = PublicLayout.layout head (body basket)

  let get: HttpHandler =
    Services.inject<ShopContext> (fun db ->
      fun ctx ->
        task {
          let! existingBasket =
            (db.Baskets.Include(fun b -> b.Items).OrderBy(fun b -> b.Id).Take(1))
            |> tryFirstTaskAsync

          let basket = existingBasket |> defaultValue (emptyBasket)

          return Response.ofHtml (Template.page basket) ctx
        })

  let post: HttpHandler =
    Services.inject<ShopContext> (fun db ->

      let mapAsync = fun (form: FormCollectionReader) ->
        form.TryGetGuid "id"
        |> BasketDomain.updateBasket db 1
        |> Async.StartAsTask

      Request.mapFormAsync mapAsync (fun quantity ->
        match quantity with
        | None -> Response.redirectPermanently "/basket?error=notfound"
        | Some q -> Response.redirectPermanently $"/basket?added={q}"))

  let remove: HttpHandler =
    Services.inject<ShopContext> (fun db ->

      let mapAsync = fun (form: FormCollectionReader) ->
        form.TryGetGuid "id"
        |> fun catalogItemId ->
          match catalogItemId with
          | Some id -> BasketDomain.removeFromBasket db id |> Async.StartAsTask
          | None -> async { return None } |> Async.StartAsTask

      Request.mapFormAsync mapAsync (fun result ->
        match result with
        | None -> Response.redirectPermanently "/basket?error=notfound"
        | Some _ -> Response.redirectPermanently "/basket?removed=success"))

  // This uses a more low-level approach to reading the form
  let postAlternate: HttpHandler =
    Services.inject<ShopContext> (fun db -> fun ctx ->
      task {
        let! form = Request.getForm ctx

        let productId = form.TryGetGuid "id"

        let! quantity = BasketDomain.updateBasket db 1 productId |> Async.StartAsTask

        return!
          match quantity with
          | Some q -> Response.redirectPermanently $"/basket?added={q}" ctx
          | None -> Response.redirectPermanently "/basket?error=notfound" ctx
      })
