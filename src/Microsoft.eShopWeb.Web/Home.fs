namespace Microsoft.eShopOnWeb.Web

open Microsoft.eShopWeb.Web
open Falco.Markup
open Falco

module Home =

  let head = ViewTemplate.head { Title = "Home"; Description = "" }

  let body = ViewTemplate.body [ Elem.h1 [] [ Text.raw "Welcome to the F# Shop!!" ] ]

  let page = ViewTemplate.layout head body

  let homeHandler: HttpHandler = Response.ofHtml page
