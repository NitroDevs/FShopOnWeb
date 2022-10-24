namespace Microsoft.eShopWeb.Web

module ViewTemplate =

  open Falco.Markup

  type HeadMetadata = { Title: string; Description: string }

  let styles = [ Elem.link [ Attr.href "/css/site.min.css"; Attr.rel "stylesheet" ] ]

  let meta metadata =
    [ Elem.title [] [ Text.raw $"FShopOnWeb - {metadata.Title}" ] ]

  let headTemplate elems = Elem.head [] elems

  let head metadata = headTemplate (styles @ meta metadata)


  let bodyTemplate elems = Elem.body [] elems

  let body elems = bodyTemplate elems


  let layout head body =
    Elem.html [ Attr.lang "en" ] [ head; body ]
