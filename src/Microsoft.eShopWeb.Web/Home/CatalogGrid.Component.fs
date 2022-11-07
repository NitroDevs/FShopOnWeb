namespace Microsoft.eShopWeb.Web.Home

open Falco.Markup.Attr
open Falco.Markup.Elem
open Falco.Markup.Text
open Microsoft.eShopWeb.Web.Domain

module CatalogGridComponent =
  type Props = { CatalogItems: CatalogItem list }

  module private Template =
    let itemTmpl item =
      div
        [ class' "esh-catalog-item col-md-4" ]
        [ form
            [ method "post"; action "/basket" ]
            [ img [ src item.PictureUri; class' "esh-catalog-thumbnail" ]
              input [ class' "esh-catalog-button"; type' "submit"; value "[ ADD TO BASKET ]" ]
              div [ class' "esh-catalog-name" ] [ span [] [ raw item.Name ] ]
              div [ class' "esh-catelog-price" ] [ span [] [ raw (item.Price.ToString "C") ] ]
              input [ type' "hidden"; name "id"; id "catalogItem_Id"; value $"{item.Id}" ]

              // TODO - add XSRF protection
              // https://www.falcoframework.com/docs/security.html#cross-site-scripting-xss-attacks
              ] ]

  let cmpt props =
    div [ class' "esh-catalog-items row" ] (List.map Template.itemTmpl props.CatalogItems)
