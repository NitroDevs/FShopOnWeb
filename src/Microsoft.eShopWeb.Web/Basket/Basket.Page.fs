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
    open Microsoft.AspNetCore.Http
    open Falco.Markup.Text

    let metadata: PublicLayout.HeadMetadata = { Title = "Basket"; Description = "" }

    let head = PublicLayout.head metadata

    let messageFromQuery (ctx: HttpContext) =
      let query = Request.getQuery ctx
      match query.TryGetString "added", query.TryGetString "removed", query.TryGetString "error" with
      | Some quantity, _, _ ->
        Some $"✓ {quantity} item(s) added to basket successfully!"
      | _, Some _, _ ->
        Some "✓ Item removed from basket successfully!"
      | _, _, Some "notfound" ->
        Some "⚠ Item not found."
      | _, _, Some "removenotfound" ->
        Some "⚠ Could not remove item from basket."
      | _ -> None

    let messageDiv message =
      match message with
      | Some (msg: string) when msg.StartsWith "✓" ->
        div [ Markup.Attr.class' "alert alert-success alert-dismissible fade show"; Markup.Attr.dataAttr "role" "alert" ]
          [ raw msg
            button [ Markup.Attr.type' "button"; Markup.Attr.class' "btn-close"; Markup.Attr.dataAttr "data-bs-dismiss" "alert" ] [] ]
      | Some msg when msg.StartsWith "⚠" ->
        div [ Markup.Attr.class' "alert alert-warning alert-dismissible fade show"; Markup.Attr.dataAttr "role" "alert" ]
          [ raw msg
            button [ Markup.Attr.type' "button"; Markup.Attr.class' "btn-close"; Markup.Attr.dataAttr "data-bs-dismiss" "alert" ] [] ]
      | _ -> div [] []

    let body basket (ctx: HttpContext) =
      let message = messageFromQuery ctx
      PublicLayout.body
        [ div [ Markup.Attr.class' "container" ]
            [ messageDiv message
              div [] (BasketComponent.cmpt basket) ] ] basket

    let page basket ctx = PublicLayout.layout head (body basket ctx)

  let get: HttpHandler =
    Services.inject<ShopContext> (fun db ->
      fun ctx ->
        task {
          let! existingBasket =
            (db.Baskets.Include(fun b -> b.Items).OrderBy(fun b -> b.Id).Take(1))
            |> tryFirstTaskAsync

          let basket = existingBasket |> defaultValue (emptyBasket)

          return Response.ofHtml (Template.page basket ctx) ctx
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

  let remove: HttpHandler =
    Services.inject<ShopContext> (fun db ->
      let mapAsync = fun (form: FormCollectionReader) ->
        form.TryGetGuid "catalogItemId"
        |> function
        | Some catalogItemId -> BasketDomain.removeBasketItem db catalogItemId |> Async.StartAsTask
        | None -> System.Threading.Tasks.Task.FromResult(false)

      Request.mapFormAsync mapAsync (fun success ->
        match success with
        | true -> Response.redirectPermanently "/basket?removed=true"
        | false -> Response.redirectPermanently "/basket?error=removenotfound"))
