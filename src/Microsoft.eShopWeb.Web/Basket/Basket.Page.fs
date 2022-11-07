namespace Microsoft.eShopWeb.Web.Basket

open Falco
open Falco.Markup.Attr
open Falco.Markup.Elem
open Microsoft.eShopWeb.Web.Persistence
open Microsoft.eShopWeb.Web
open System.Linq

module BasketPage =

  module private Template =

    let metadata: PublicLayout.HeadMetadata = { Title = "Basket"; Description = "" }

    let head = PublicLayout.head metadata

    let body basket =
      PublicLayout.body [ div [ class' "container" ] (BasketComponent.cmpt basket) ]

    let page basket = PublicLayout.layout head (body basket)

  let private getIdFromForm (form: FormCollectionReader) =
    form.TryGetString "id" |> Option.map int

  let handler: HttpHandler =
    Services.inject<ShopContext> (fun context ->
      Request.mapForm
        (fun form ->
          let dbItems = context.CatalogItems.ToList()

          let items = List.ofSeq (dbItems)
          let basket = BasketDomain.basketFromCatalog List.empty

          let id = getIdFromForm form

          let basketToRender =
            match id with
            | None -> basket
            | Some id ->
              let catalogItem = items[id]

              let updatedBasket = BasketDomain.addItemToBasket basket catalogItem
              updatedBasket

          Template.page basketToRender)
        Response.ofHtml)
