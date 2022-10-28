namespace Microsoft.eShopWeb.Web.Home

open Microsoft.eShopWeb.Web
open Falco
open Microsoft.eShopWeb.Web.ViewTemplate
open Microsoft.eShopWeb.Web.Home

module HomePage =
  open Falco.Markup
  let metadata: HeadMetadata = { Title = "Home"; Description = "" }

  let head = ViewTemplate.head metadata
  let body =
    ViewTemplate.body  [ CatalogFilters.cmpt; Elem.div [ Attr.class' "container"] ProductsGrid.cmpt ]
  let page = layout head body

  let handler: HttpHandler = Response.ofHtml page
