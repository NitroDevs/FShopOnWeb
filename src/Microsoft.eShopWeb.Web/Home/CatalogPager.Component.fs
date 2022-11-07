namespace Microsoft.eShopWeb.Web.Home

open Falco.Markup.Attr
open Falco.Markup.Elem
open Falco.Markup.Text

module CatalogPagerComponent =
  type Props = { CurrentPage: int; ItemsCount: int }

  module private Template =
    type State =
      { PagingText: string
        PreviousClass: string
        NextClass: string }

    let calculateState props =
      let currentPage = 1
      let pageSize = 10

      let totalPages =
        match (props.ItemsCount / pageSize) with
        | 0 -> 1
        | _ -> (props.ItemsCount / pageSize)

      let morePagesAvailable = props.ItemsCount > pageSize
      let hasPreviousPage = currentPage > 1
      let hasNextPage = currentPage < totalPages

      let totalItemsOnPage =
        match morePagesAvailable with
        | true -> pageSize
        | false -> props.ItemsCount

      let currentItemIndex = (pageSize * (currentPage - 1)) + 1

      let previousClass =
        match hasPreviousPage with
        | true -> "esh-pager-item-left esh-pager-item--navigable esh-pager-item"
        | false -> "esh-pager-item-left esh-pager-item--navigable esh-pager-item is-disabled"

      let nextClass =
        match hasNextPage with
        | true -> "esh-pager-item-right esh-pager-item--navigable esh-pager-item"
        | false -> "esh-pager-item-right esh-pager-item--navigable esh-pager-item is-disabled"

      let pagingText =
        sprintf "Showing %d of %d products - Page %d - %d" currentItemIndex totalItemsOnPage currentPage totalPages

      { PagingText = pagingText
        NextClass = nextClass
        PreviousClass = previousClass }

  let cmpt props =
    let state = Template.calculateState props

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
                      [ a [ class' state.PreviousClass; href "/?pageId=-1" ] [ raw "Previous" ] ]
                    div [ class' "col-md-8 col-xs-12" ] [ span [ class' "esh-pager-item" ] [ raw state.PagingText ] ]
                    div
                      [ class' "col-md-2 col-xs-12" ]
                      [ a [ class' state.NextClass; id "Next"; href "/?pageId=1" ] [ raw "Next" ] ] ] ] ] ]
