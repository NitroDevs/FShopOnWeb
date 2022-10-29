namespace Microsoft.eShopWeb.Web

open Falco.Markup
open Falco.Markup.Elem
open Falco.Markup.Attr

module ViewTemplate =
  open Falco.Markup.Text

  module Attr =
    let data value = create $"data-{value}"

  type HeadMetadata = { Title: string; Description: string }

  let styles = [ link [ href "/css/site.min.css"; rel "stylesheet" ] ]

  let meta metadata =
    [ Elem.title [] [ raw $"FShopOnWeb - {metadata.Title}" ] ]

  let headTemplate elems = head [] elems

  type XmlHeadNode = XmlNode
  let head (metadata) : XmlHeadNode = headTemplate (styles @ meta metadata)

  let navBar =
    header
      [ class' "esh-app-header" ]
      [ div
          [ class' "container" ]
          [ article
              [ class' "row" ]
              [ section
                  [ class' "col-lg-7 col-md-6 col-xs-12" ]
                  [ a [ href "/" ] [ img [ src "/images/brand.png"; alt "F# Shop On Web" ] ] ]
                section
                  [ class' "col-lg-4 col-md-5 col-xs-12" ]
                  [ div
                      [ class' "esh-identity" ]
                      [ section
                          [ class' "esh-identity-section" ]
                          [ div
                              [ class' "esh-identity-item" ]
                              [ a
                                  [ class' "esh-identity-name esh-identity-name--upper"
                                    href "/Identity/Account/Login" ]
                                  [ raw "Login" ] ] ] ] ]
                section
                  [ class' "col-lg-1 col-xs-12" ]
                  [ a
                      [ class' "esh-basketstatus"; href "/Basket" ]
                      [ div [ class' "esh-basketstatus-image" ] [ img [ src "/images/cart.png" ] ]
                        div [ class' "esh-basketstatus-badge" ] [ raw "0" ] ] ] ] ] ]

  let hero =
    section
      [ class' "esh-catalog-hero" ]
      [ div [ class' "container" ] [ img [ class' "esh-catalog-title"; src "/images/main_banner_text.png" ] ] ]

  let footer =
    footer
      [ class' "esh-app-footer footer" ]
      [ div
          [ class' "container" ]
          [ article
              [ class' "row" ]
              [ section [ class' "col-sm-6" ] []
                section
                  [ class' "col-sm-6" ]
                  [ div [ class' "esh-app-footer-text hidden-xs" ] [ raw "f-ShopOnWeb. All rights reserved" ] ] ] ] ]


  type XmlBodyNode = XmlNode

  let bodyTemplate (elems) : XmlBodyNode =
    let slotContents = [ navBar; hero ] @ elems @ [ footer ]
    let wrapper = div [ class' "esh-app-wrapper" ] slotContents
    body [] [ wrapper ]

  let body elems = bodyTemplate elems


  let layout (head: XmlHeadNode) (body: XmlBodyNode) = html [ lang "en" ] [ head; body ]
