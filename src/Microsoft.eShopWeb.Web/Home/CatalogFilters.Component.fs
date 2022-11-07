namespace Microsoft.eShopWeb.Web.Home

open Falco.Markup
open Falco.Markup.Attr
open Falco.Markup.Elem
open Falco.Markup.Text

module CatalogFiltersComponent =
  type Props =
    { Brands: string list
      Types: string list }

  module private Template =

    let allOption = option [] [ raw "All" ]

    let optionTmpl index v =
      option [ value $"{index}" ] [ raw $"{v}" ]

    let brandSelect (brands: string list) =
      let brandOptions = [ allOption ] @ List.mapi optionTmpl brands

      select
        [ class' "esh-catalog-filter"
          id "CatalogModel_BrandFilterApplied"
          name "CatalogModel.BrandFilterApplied" ]
        brandOptions

    let typesSelect (types: string list) =
      let typeOptions = [ allOption ] @ List.mapi optionTmpl types

      select
        [ class' "esh-catalog-filter"
          id "CatalogModel_TypesFilterApplied"
          name "CatalogModel.TypesFilterApplied" ]
        typeOptions

  let cmpt props =
    section
      [ class' "esh-catalog-filters" ]
      [ div
          [ class' "container" ]
          [ form
              [ method "get" ]
              [ label
                  [ class' "esh-catalog-label"; Attr.create "data-title" "brand" ]
                  [ Template.brandSelect props.Brands ]
                label
                  [ class' "esh-catalog-label"; Attr.create "data-title" "type" ]
                  [ Template.typesSelect props.Types ]
                input [ src "/images/arrow-right.svg"; class' "esh-catalog-send"; type' "image" ] ] ] ]
