namespace Microsoft.eShopWeb.Web.Home

open Falco
open Falco.Markup.Attr
open Falco.Markup.Elem
open Microsoft.eShopWeb.Web
open Microsoft.eShopWeb.Web.Home

module HomePage =
  module private Template =
    let metadata: PublicLayout.HeadMetadata = { Title = "Home"; Description = "" }

    let head = PublicLayout.head metadata

    let body =
      PublicLayout.body [ CatalogFilters.cmpt; div [ class' "container" ] [ CatalogGrid.cmpt ] ]

    let page = PublicLayout.layout head body

  let handler: HttpHandler = Response.ofHtml Template.page
