namespace Microsoft.eShopWeb.Web.Home

open Falco.Markup.Attr
open Falco.Markup.Elem
open Falco.Markup.Text

module CatalogPagerComponent =
  type Props = { CurrentPage: int; ItemsCount: int }

  module private Template =

    let pagingText props =
      let currentPage = 1
      let pageSize = 10
      let totalPages = (props.ItemsCount / pageSize) + 1
      let morePagesAvailable = props.ItemsCount > pageSize
      let hasPreviousPage = currentPage > 1
      let hasNextPage = currentPage < totalPages

      let totalItemsOnPage =
        match morePagesAvailable with
        | true -> pageSize
        | false -> props.ItemsCount

      let currentItemIndex = (pageSize * (currentPage - 1)) + 1
      sprintf "Showing %d of %d products - Page %d - %d" currentItemIndex totalItemsOnPage currentPage totalPages

  let cmpt props =
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
                    div
                      [ class' "col-md-8 col-xs-12" ]
                      [ span [ class' "esh-pager-item" ] [ raw (Template.pagingText props) ] ]
                    div
                      [ class' "col-md-2 col-xs-12" ]
                      [ a
                          [ class' "esh-pager-item-right esh-pager-item--navigable esh-pager-item is-disabled"
                            id "Next"
                            href "/?pageId=1" ]
                          [ raw "Next" ] ] ] ] ] ]
