namespace Microsoft.eShopWeb.Web.Home

open Microsoft.eShopWeb.Web.Domain
open Falco
open Falco.Markup.Attr
open Falco.Markup.Elem
open Microsoft.eShopWeb.Web
open Microsoft.eShopWeb.Web.Home
open Microsoft.eShopWeb.Web.Persistence
open Microsoft.EntityFrameworkCore
open EntityFrameworkCore.FSharp.DbContextHelpers
open Microsoft.AspNetCore.Http

module HomePage =

  type Props =
    { FiltersProps: CatalogFiltersComponent.Props
      GridProps: CatalogGridComponent.Props
      PagerProps: CatalogPagerComponent.Props }

  module private Template =
    let metadata: PublicLayout.HeadMetadata = { Title = "Home"; Description = "" }

    let head = PublicLayout.head metadata

    let body props =
      PublicLayout.body
        [ CatalogFiltersComponent.cmpt props.FiltersProps
          div
            [ class' "container" ]
            [ CatalogPagerComponent.cmpt props.PagerProps
              CatalogGridComponent.cmpt props.GridProps
              CatalogPagerComponent.cmpt props.PagerProps ] ]

    /// <summary>
    /// Generates the <see cref="XmlNode" /> rendering for the Home page
    /// </summary>
    let page (db: ShopContext) =
      task {
        let! dbItems =
          toListTaskAsync (db.CatalogItems.Include(fun i -> i.CatalogBrand).Include(fun i -> i.CatalogType))

        let items = List.ofSeq (dbItems)
        let brands = List.map (fun i -> i.CatalogBrand.Name) items |> List.distinct
        let types = List.map (fun i -> i.CatalogType.Name) items |> List.distinct

        let props =
          { FiltersProps = { Types = types; Brands = brands }
            GridProps = { CatalogItems = items }
            PagerProps =
              { ItemsCount = items.Length
                CurrentPage = 1 } }

        return PublicLayout.layout head (body props)
      }

  let handler: HttpHandler =
    Services.inject<ShopContext> (fun db -> db |> Template.page |> Response.ofHtmlTask)
