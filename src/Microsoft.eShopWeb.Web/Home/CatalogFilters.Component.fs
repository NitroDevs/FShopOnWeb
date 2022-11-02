namespace Microsoft.eShopWeb.Web.Home

open Falco.Markup
open Falco.Markup.Attr
open Falco.Markup.Elem
open Falco.Markup.Text
open Microsoft.eShopWeb.Web.Home

module CatalogFiltersComponent =
  module private Template =
    let allOption = option [] [ raw "All" ]

    let optionTmpl index v =
      option [ value $"{index}" ] [ raw $"{v}" ]

    let brandSelect =
      let brandOptions = [ allOption ] @ List.mapi optionTmpl HomeDomain.brands

      select
        [ class' "esh-catalog-filter"
          id "CatalogModel_BrandFilterApplied"
          name "CatalogModel.BrandFilterApplied" ]
        brandOptions

    let typesSelect =
      let typeOptions = [ allOption ] @ List.mapi optionTmpl HomeDomain.types

      select
        [ class' "esh-catalog-filter"
          id "CatalogModel_TypesFilterApplied"
          name "CatalogModel.TypesFilterApplied" ]
        typeOptions

  let cmpt =
    section
      [ class' "esh-catalog-filters" ]
      [ div
          [ class' "container" ]
          [ form
              [ method "get" ]
              [ label [ class' "esh-catalog-label"; Attr.create "data-title" "brand" ] [ Template.brandSelect ]
                label [ class' "esh-catalog-label"; Attr.create "data-title" "type" ] [ Template.typesSelect ]
                input [ src "/images/arrow-right.svg"; class' "esh-catalog-send"; type' "image" ] ] ] ]
