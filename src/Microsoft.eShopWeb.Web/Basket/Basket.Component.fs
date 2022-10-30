namespace Microsoft.eShopWeb.Web

open Falco.Markup
open Falco.Markup.Elem
open Falco.Markup.Attr
open Falco.Markup.Text
open Microsoft.eShopWeb.Web.Basket.Domain

module BasketComponent =

  module private Template =
    let itemTmpl index item =
      article
        [ class' "esh-basket-items" ]
        [ div
            [ class' "row row-cols-auto justify-content-between" ]
            [ section
                [ class' "esh-basket-item esh-basket-item--middle hidden-lg-down col" ]
                [ img
                    [ src (
                        match item.PictureUri with
                        | Some uri -> uri
                        | None -> productFallbackImageUri
                      )
                      class' "esh-basket-image" ] ]
              section [ class' "esh-basket-item esh-basket-item--middle col" ] [ raw item.ProductName ]
              section [ class' "esh-basket-item esh-basket-item--middle col" ] [ raw (item.UnitPrice.ToString "C") ] ] ]

    let itemsTmpl basket =
      [ Elem.form
          [ method "post" ]
          [ article
              [ class' "esh-basket-titles row row-cols-auto justify-content-between" ]
              [ section [ class' "esh-basket-title col" ] [ raw "Product" ]
                section [ class' "esh-basket-title col hidden-lg-down" ] []
                section [ class' "esh-basket-title col" ] [ raw "Price" ]
                section [ class' "esh-basket-title col" ] [ raw "Quantity" ]
                section [ class' "esh-basket-title col" ] [ raw "Cost" ] ]
            div [ class' "esh-catalog-items" ] (List.mapi itemTmpl basket) ] ]

    let noItemsTmpl =
      [ h3 [ class' "esh-catalog-items row" ] [ raw "Basket is empty." ]

        section
          [ class' "esh-basket-item" ]
          [ a [ href "/"; class' "btn esh-basket-checkout text-white" ] [ raw "[ Continue Shopping ]" ] ] ]

  let cmpt basket =
    let items = basket.Items

    match items with
    | [] -> Template.noItemsTmpl
    | _ -> Template.itemsTmpl items
