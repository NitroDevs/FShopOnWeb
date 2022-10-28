namespace Microsoft.eShopWeb.Web

open Falco.Markup.Attr
open ViewTemplate.Attr
open Falco.Markup.Elem
open Falco.Markup.Text
open Microsoft.eShopWeb.Web.Domain

module ProductsGrid =
  let itemTmpl index item =
    div [ class' "esh-catalog-item col-md-4"] [
      form [ method "post"; action "/Basket"] [
        img [ src item.PictureUri; class' "esh-catalog-thumbnail" ]
        input [ class' "esh-catalog-button"; type' "submit"; value "[ ADD TO BASKET ]"]
        div [ class' "esh-catalog-name"] [ span [] [ raw item.Name ]]
        div [ class' "esh-catelog-price"] [ span [] [ raw (item.Price.ToString("C")) ]]
        input [ type' "hidden"; name "id"; id "catalogItem_Id"; value (sprintf "%d" index) ]
        input [ type' "hidden"; name "name"; id "catalogItem_Name"; value item.Name ]
        input [ type' "hidden"; name "pictureUri"; id "catalogItem_PictureUri"; value item.PictureUri ]
        input [ type' "hidden"; name "price"; id "catalogItem_Price"; value (sprintf "%M" item.Price) ]

        // TODO - figure out how to generate an XSRF token (@Html.AntiForgeryToken() in Razor)
    ] ]

  let cmpt = List.mapi itemTmpl catalogItems
