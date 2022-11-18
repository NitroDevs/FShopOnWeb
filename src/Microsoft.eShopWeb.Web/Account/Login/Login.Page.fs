namespace Microsoft.eShopWeb.Web.Account.Login

open Microsoft.eShopWeb.Web
open Microsoft.eShopWeb.Web.Domain
open Microsoft.eShopWeb.Web.Persistence
open Microsoft.EntityFrameworkCore
open Microsoft.FSharp.Core.Option
open EntityFrameworkCore.FSharp.DbContextHelpers
open Falco
open Falco.Markup
open Falco.Markup.Elem
open Falco.Markup.Attr
open Falco.Markup.Text
open System.Linq

module LoginPage =

  type Props = { Basket: Basket }

  module private Template =

    let metadata: PublicLayout.HeadMetadata = { Title = "Login"; Description = "" }

    let head = PublicLayout.head metadata

    let body props: XmlNode =
      PublicLayout.body
          [
            div
              [ class' "container account-login-container" ]
              [ h2 [] [ raw "Log in"]
                div [ class' "row" ] [
                  div [ class' "col-md-12" ] [
                    section [] [] ] ] ] ] // TODO: finish this HTML
          props.Basket

    /// <summary>
    /// Generates the <see cref="XmlNode" /> rendering for the Login page
    /// </summary>
    let page props =
      PublicLayout.layout head (body props)

  let handler: HttpHandler =
    Services.inject<ShopContext> (fun db ->
      fun ctx ->
        task {
          let! existingBasket =
            (db.Baskets.Include(fun b -> b.Items).OrderBy(fun b -> b.Id).Take(1))
            |> tryFirstTaskAsync

          let basket = existingBasket |> defaultValue (emptyBasket)

          let props = { Basket = basket }

          return Response.ofHtml (Template.page props) ctx
        })
