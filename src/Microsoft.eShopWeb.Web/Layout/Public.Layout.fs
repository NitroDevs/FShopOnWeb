namespace Microsoft.eShopWeb.Web

open Falco.Markup
open Falco.Markup.Elem
open Falco.Markup.Attr
open Falco.Markup.Text

module PublicLayout =

  type HeadMetadata = { Title: string; Description: string }

  module Template =
    open Domain

    let styles = [ link [ href "/css/site.css"; rel "stylesheet" ] ]
    let head elems = head [] elems

    let meta metadata =
      [ Elem.title [] [ raw $"FShopOnWeb - {metadata.Title}" ] ]

    let navBar (basket: Basket) =
      let itemsCount = basket.Items |> Seq.map (fun i -> i.Quantity) |> Seq.sum

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
                        [ class' "esh-basketstatus"; href "/basket" ]
                        [ div [ class' "esh-basketstatus-image" ] [ img [ src "/images/cart.png" ] ]
                          div [ class' "esh-basketstatus-badge" ] [ raw (itemsCount.ToString()) ] ] ] ] ] ]

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

  let head metadata =
    Template.head (Template.styles @ Template.meta metadata)

  let body elems basket =
    body
      [ class' "m-0" ]
      [ div [ class' "esh-app-wrapper" ] ([ Template.navBar basket; Template.hero ] @ elems @ [ Template.footer ]) ]

  let layout head body = html [ lang "en" ] [ head; body ]
