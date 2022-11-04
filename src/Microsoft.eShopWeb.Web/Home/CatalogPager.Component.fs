namespace Microsoft.eShopWeb.Web.Home

open Falco.Markup.Attr
open Falco.Markup.Elem
open Falco.Markup.Text

module CatalogPagerComponent =
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
                    div
                      [ class' "col-md-8 col-xs-12" ]
                      [ span [ class' "esh-pager-item" ] [ raw "Showing 3 of 3 products - Page 1 - 1" ] ]
                    div
                      [ class' "col-md-2 col-xs-12" ]
                      [ a
                          [ class' "esh-pager-item-right esh-pager-item--navigable esh-pager-item is-disabled"
                            id "Next"
                            href "/?pageId=1" ]
                          [ raw "Next" ] ] ] ] ] ]
