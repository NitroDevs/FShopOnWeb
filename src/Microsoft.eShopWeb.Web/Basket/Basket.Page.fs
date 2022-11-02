namespace Microsoft.eShopWeb.Web.Basket

open Falco
open Falco.Markup.Attr
open Falco.Markup.Elem
open Microsoft.eShopWeb.Web

module BasketPage =
  module private Template =

    let metadata: PublicLayout.HeadMetadata = { Title = "Basket"; Description = "" }

    let head = PublicLayout.head metadata

    let body =
      PublicLayout.body [ div [ class' "container" ] (BasketComponent.cmpt BasketDomain.basket) ]

    let page = PublicLayout.layout head body

  let handler: HttpHandler = Response.ofHtml Template.page
