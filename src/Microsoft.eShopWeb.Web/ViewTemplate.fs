namespace Microsoft.eShopWeb.Web

module ViewTemplate =

  open Falco.Markup

  type HeadMetadata = { Title: string; Description: string }

  let styles = [ Elem.link [ Attr.href "/css/site.min.css"; Attr.rel "stylesheet" ] ]

  let meta metadata =
    [ Elem.title [] [ Text.raw $"FShopOnWeb - {metadata.Title}" ] ]

  let headTemplate elems = Elem.head [] elems

  type XmlHeadNode = XmlNode
  let head (metadata) : XmlHeadNode = headTemplate (styles @ meta metadata)

  type XmlBodyNode = XmlNode
  let bodyTemplate (elems) : XmlBodyNode = Elem.body [] elems

  let body elems = bodyTemplate elems


  let layout (head: XmlHeadNode) (body: XmlBodyNode) =
    Elem.html [ Attr.lang "en" ] [ head; body ]
