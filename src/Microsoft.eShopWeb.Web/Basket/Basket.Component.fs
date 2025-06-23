namespace Microsoft.eShopWeb.Web

open Falco.Markup
open Falco.Markup.Elem
open Falco.Markup.Attr
open Falco.Markup.Text
open Microsoft.eShopWeb.Web.Basket.BasketDomain
open Domain
open System

module BasketComponent =

  module private Template =

    let itemTmpl index (item: BasketItem) =
      article
        [ class' "esh-basket-items" ]
        [ div
            [ class' "row row-cols-auto justify-content-between" ]
            [ section
                [ class' "esh-basket-item esh-basket-item--middle hidden-lg-down col" ]
                [ img
                    [ src (
                        match item.PictureUri with
                        | emptyUri when String.IsNullOrWhiteSpace(emptyUri) -> productFallbackImageUri
                        | uri -> uri
                      )
                      class' "esh-basket-image" ] ]
              section [ class' "esh-basket-item esh-basket-item--middle col" ] [ raw item.ProductName ]
              section [ class' "esh-basket-item esh-basket-item--middle col" ] [ raw (item.UnitPrice.ToString "C") ]
              section [ class' "esh-basket-item esh-basket-item--middle col" ] [ raw (item.Quantity.ToString()) ]
              section [ class' "esh-basket-item esh-basket-item--middle col" ] [ raw ((decimal(item.Quantity) * item.UnitPrice).ToString "C" ) ]
              section [ class' "esh-basket-item esh-basket-item--middle col" ] 
                [ Elem.form
                    [ method "post"; action "/basket/remove" ]
                    [ input [ type' "hidden"; name "catalogItemId"; value (item.CatalogItemId.ToString()) ]
                      button 
                        [ type' "submit"
                          class' "btn btn-sm btn-outline-danger" 
                          title "Remove item from basket"
                          onclick "return confirm('Are you sure you want to remove this item from your basket?');" ]
                        [ raw "Remove" ] ] ] ] ]

    let itemsTmpl items =
      [ Elem.form
          [ method "post" ]
          [ article
              [ class' "esh-basket-titles row row-cols-auto justify-content-between" ]
              [ section [ class' "esh-basket-title col" ] [ raw "Product" ]
                section [ class' "esh-basket-title col hidden-lg-down" ] []
                section [ class' "esh-basket-title col" ] [ raw "Price" ]
                section [ class' "esh-basket-title col" ] [ raw "Quantity" ]
                section [ class' "esh-basket-title col" ] [ raw "Cost" ]
                section [ class' "esh-basket-title col" ] [ raw "Actions" ] ]
            div [ class' "esh-catalog-items" ] (Seq.mapi itemTmpl items |> List.ofSeq) ] ]

    let noItemsTmpl =
      [ h3 [ class' "esh-catalog-items row" ] [ raw "Basket is empty." ]

        section
          [ class' "esh-basket-item" ]
          [ a [ href "/"; class' "btn esh-basket-checkout text-white" ] [ raw "[ Continue Shopping ]" ] ] ]

  let cmpt basket =
    let items = basket.Items

    match items with
    | s when Seq.isEmpty s -> Template.noItemsTmpl
    | _ -> Template.itemsTmpl items
