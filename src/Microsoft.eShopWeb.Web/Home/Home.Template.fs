namespace Microsoft.eShopWeb.Web.Home

open Falco
open Falco.Markup.Attr
open Falco.Markup.Elem
open Microsoft.eShopWeb.Web.Home.CatalogFilters
open Microsoft.eShopWeb.Web.ProductsGrid
open Microsoft.eShopWeb.Web.ViewTemplate

module HomePage =
  let metadata: HeadMetadata = { Title = "Home"; Description = "" }

  let head = head metadata
  let body = body [ catalogFilters; div [ class' "container" ] [ productsGrid ] ]
  let page = layout head body

  let handler: HttpHandler = Response.ofHtml page
