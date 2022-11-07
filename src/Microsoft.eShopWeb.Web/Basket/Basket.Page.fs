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

  let handler: HttpHandler =
    Services.inject<ShopContext> (fun context ->
      let dbItems = context.CatalogItems.ToList()

      let items = List.ofSeq (dbItems)
      let basket = BasketDomain.basketFromCatalog items

      Response.ofHtml (Template.page basket))
