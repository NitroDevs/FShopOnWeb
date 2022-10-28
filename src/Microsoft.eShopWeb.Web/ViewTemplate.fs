namespace Microsoft.eShopWeb.Web

open Falco.Markup

module ViewTemplate =
  module Attr =
    let data value = Attr.create $"data-{value}"

  type HeadMetadata = { Title: string; Description: string }

  let styles = [ Elem.link [ Attr.href "/css/site.min.css"; Attr.rel "stylesheet" ] ]

  let meta metadata =
    [ Elem.title [] [ Text.raw $"FShopOnWeb - {metadata.Title}" ] ]

  let headTemplate elems = Elem.head [] elems

  type XmlHeadNode = XmlNode
  let head (metadata) : XmlHeadNode = headTemplate (styles @ meta metadata)

  let navBar =
    Elem.header
      [ Attr.class' "esh-app-header" ]
      [ Elem.div
          [ Attr.class' "container" ]
          [ Elem.article
              [ Attr.class' "row" ]
              [ Elem.section
                  [ Attr.class' "col-lg-7 col-md-6 col-xs-12" ]
                  [ Elem.a [ Attr.href "/" ] [ Elem.img [ Attr.src "/images/brand.png"; Attr.alt "F# Shop On Web" ] ] ]
                Elem.section
                  [ Attr.class' "col-lg-4 col-md-5 col-xs-12" ]
                  [ Elem.div
                      [ Attr.class' "esh-identity" ]
                      [ Elem.section
                          [ Attr.class' "esh-identity-section" ]
                          [ Elem.div
                              [ Attr.class' "esh-identity-item" ]
                              [ Elem.a
                                  [ Attr.class' "esh-identity-name esh-identity-name--upper"
                                    Attr.href "/Identity/Account/Login" ]
                                  [ Text.raw "Login" ] ] ] ] ]
                Elem.section
                  [ Attr.class' "col-lg-1 col-xs-12" ]
                  [ Elem.a
                      [ Attr.class' "esh-basketstatus"; Attr.href "/Basket" ]
                      [ Elem.div [ Attr.class' "esh-basketstatus-image" ] [ Elem.img [ Attr.src "/images/cart.png" ] ]
                        Elem.div [ Attr.class' "esh-basketstatus-badge" ] [ Text.raw "0" ] ] ] ] ] ]

  let hero =
    Elem.section
      [ Attr.class' "esh-catalog-hero" ]
      [ Elem.div
          [ Attr.class' "container" ]
          [ Elem.img [ Attr.class' "esh-catalog-title"; Attr.src "/images/main_banner_text.png" ] ] ]

  let footer =
    Elem.footer [ Attr.class' "esh-app-footer footer"] [
      Elem.div [ Attr.class' "container"] [
        Elem.article [ Attr.class' "row" ] [
          Elem.section [ Attr.class' "col-sm-6"] []
          Elem.section [ Attr.class' "col-sm-6"] [
            Elem.div [ Attr.class' "esh-app-footer-text hidden-xs"] [
              Text.raw "f-ShopOnWeb. All rights reserved"
            ]
          ]
        ]
      ]
    ]


  type XmlBodyNode = XmlNode
  let bodyTemplate (elems) : XmlBodyNode =
    let slotContents = [navBar; hero;] @ elems @ [footer]
    let wrapper = Elem.div [ Attr.class' "esh-app-wrapper" ] slotContents
    Elem.body [] [wrapper]

  let body elems = bodyTemplate elems


  let layout (head: XmlHeadNode) (body: XmlBodyNode) =
    Elem.html [ Attr.lang "en" ] [ head; body ]
