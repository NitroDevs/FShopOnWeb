namespace Microsoft.eShopWeb.Web.Home

open Falco.Markup.Attr
open Falco.Markup.Elem
open Falco.Markup.Text
open Microsoft.eShopWeb.Web.Home
open Microsoft.eShopWeb.Web.ViewTemplate.Attr

module CatalogFilters =

  let allOption = option [] [ raw "All" ]

  let optionTmpl index v =
    option [ value $"{index}" ] [ raw $"{v}" ]

  let brandSelect =
    let brandOptions = [ allOption ] @ List.mapi optionTmpl HomeData.brands

    select
      [ class' "esh-catalog-filter"
        id "CatalogModel_BrandFilterApplied"
        name "CatalogModel.BrandFilterApplied" ]
      brandOptions

  let typesSelect =
    let typeOptions = [ allOption ] @ List.mapi optionTmpl HomeData.types

    select
      [ class' "esh-catalog-filter"
        id "CatalogModel_TypesFilterApplied"
        name "CatalogModel.TypesFilterApplied" ]
      typeOptions

  let catalogFilters =
    section
      [ class' "esh-catalog-filters" ]
      [ div
          [ class' "container" ]
          [ form
              [ method "get" ]
              [ label [ class' "esh-catalog-label"; data "title" "brand" ] [ brandSelect ]
                label [ class' "esh-catalog-label"; data "title" "type" ] [ typesSelect ] ] ] ]
