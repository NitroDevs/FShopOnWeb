namespace Microsoft.eShopWeb.Web.Home

open Falco.Markup
open Microsoft.eShopWeb.Web.Home
open Microsoft.eShopWeb.Web.ViewTemplate

module CatalogFilters =

  let allOption = Elem.option [] [ Text.raw "All" ]

  let brandSelect =
    let brandOptions = [ allOption ] @ List.mapi (fun i x -> Elem.option [ Attr.value $"{i}" ] [ Text.raw $"{x}" ]) HomeData.brands

    Elem.select
      [ Attr.class' "esh-catalog-filter"
        Attr.id "CatalogModel_BrandFilterApplied"
        Attr.name "CatalogModel.BrandFilterApplied" ] brandOptions

  let typesSelect =
    let typeOptions = [ allOption ] @ List.mapi (fun i x -> Elem.option [ Attr.value $"{i}" ] [ Text.raw $"{x}" ]) HomeData.types

    Elem.select
      [ Attr.class' "esh-catalog-filter"
        Attr.id "CatalogModel_TypesFilterApplied"
        Attr.name "CatalogModel.TypesFilterApplied" ] typeOptions

  let cmpt =
    Elem.section
      [ Attr.class' "esh-catalog-filters" ]
      [ Elem.div
          [ Attr.class' "container" ]
          [ Elem.form
              [ Attr.method "get" ]
              [ Elem.label
                  [ Attr.class' "esh-catalog-label"; Attr.data "title" "brand" ]
                  [ brandSelect ]
                Elem.label
                  [ Attr.class' "esh-catalog-label"; Attr.data "title" "type" ]
                  [ typesSelect ] ] ] ]
