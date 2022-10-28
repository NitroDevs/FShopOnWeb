namespace Microsoft.eShopOnWeb.Web

open Microsoft.eShopWeb.Web
open Falco.Markup
open Falco

module Home =

  let head = ViewTemplate.head { Title = "Home"; Description = "" }

  //Elem.h1 [] [ Text.raw "Welcome to the F# Shop!!" ] ]
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

  let body =
    ViewTemplate.body [ Elem.div [ Attr.class' "esh-app-wrapper" ] [ navBar ] ]

  let page = ViewTemplate.layout head body

  let homeHandler: HttpHandler = Response.ofHtml page
