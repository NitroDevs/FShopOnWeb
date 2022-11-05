namespace Microsoft.eShopWeb.Web.Home

open Falco.Markup.Attr
open Falco.Markup.Elem
open Falco.Markup.Text
open Microsoft.eShopWeb.Web.Domain

module CatalogPagerComponent =
  module private Template =
    let currentPage = 1
    let pageSize = 10
    let totalPages = (catalogItems.Length / pageSize) + 1
    let morePagesAvailable = catalogItems.Length > pageSize
    let hasPreviousPage = currentPage > 1
    let hasNextPage = currentPage < totalPages

    let totalItemsOnPage =
      match morePagesAvailable with
      | true -> pageSize
      | false -> catalogItems.Length

    let currentItemIndex = (pageSize * (currentPage - 1)) + 1

    let pagingText =
      sprintf "Showing %d of %d products - Page %d - %d" currentItemIndex totalItemsOnPage currentPage totalPages

  let cmpt =
    div
      [ class' "esh-pager" ]
      [ div
          [ class' "container-fluid" ]
          [ article
              [ class' "esh-pager-wrapper row" ]
              [ nav
                  []
                  [ div
                      [ class' "col-md-2 col-xs-12" ]
                      [ a
                          [ class' "esh-pager-item-left esh-pager-item--navigable esh-pager-item is-disabled"
                            href "/?pageId=-1" ]
                          [ raw "Previous" ] ]
                    div [ class' "col-md-8 col-xs-12" ] [ span [ class' "esh-pager-item" ] [ raw Template.pagingText ] ]
                    div
                      [ class' "col-md-2 col-xs-12" ]
                      [ a
                          [ class' "esh-pager-item-right esh-pager-item--navigable esh-pager-item is-disabled"
                            id "Next"
                            href "/?pageId=1" ]
                          [ raw "Next" ] ] ] ] ] ]
