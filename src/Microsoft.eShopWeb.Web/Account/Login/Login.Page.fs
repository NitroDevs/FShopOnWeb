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
                    section [ ] [ Elem.form [ method "post"; novalidate ] [
                      hr []
                      div [ class' "text-danger validation-summary-valid" ] [ ul [] [ li [style "display:none" ] [] ] ]
                      div [ class' "form-group" ] [
                        Elem.label [ for' "Input_Email" ] [ raw "Email" ]
                        input [ class' "form-control"; type' "email"; id "Input_Email"; name "Input.Email"; value "" ]
                        Elem.span [ class' "text-danger field-validation-valid" ] [] ] // TODO: data-X markup
                      div [ class' "form-group" ] [
                        Elem.label [ for' "Input_Password" ] [ raw "Password" ]
                        input [ class' "form-control"; type' "password"; id "Input_Password"; name "Input.Password"; ]
                        Elem.span [ class' "text-danger field-validation-valid" ] [] ] // TODO: data-X markup
                      div [ class' "form-group" ] [
                        div [ class' "checkbox" ] [
                          Elem.label [ for' "Input_RememberMe" ] [
                            input [ type' "checkbox"; id "Input_RememberMe"; name "Input.RememberMe"; value "true"; ]  // TODO: data-X markup
                            raw "Remember me?" ] ] ]
                      div [ class' "form-group" ] [ button [ type' "submit"; class' "btn btn-default" ] [ raw "Log in"] ]
                      div [ class' "form-group" ] [
                        p [] [ a [ href "/Identity/Account/ForgotPassword" ] [ raw "Forgot your password?" ] ]
                        p [] [ a [ href "/Identity/Account/Register?returnUrl=%2F"] [ raw "Register as a new user" ] ] ]
                      p [] [ raw "Note that for demo purposes you don't need to register and can login with these credentials:" ]
                      p [] [
                        raw "User: "
                        b [] [ raw "demouser@microsoft.com" ]
                        raw " OR "
                        b [] [ raw "admin@microsoft.com" ] ]
                      p [] [
                        raw " Password: "
                        b [] [ raw "Pass@word1" ] ]
                      input [ name "__RequestVerificationToken"; type' "hidden"; value "CfDJ8JYtL6prw3BBhkC7TvYaDIZZ_oxnJ_V_JLkcwX9koXookmDnqOuLzh-Gaq7_CPbywFqGS9N3cs3Tc2Pu7PsDqewH2j0RHGjfuGR38gtaZKovrU88mhVimoembgpa3tvspyEB_vvimhsVzK3ZyUGws3M"]
                      input [ name "Input.RememberMe"; type' "hidden"; value "false" ] ] ] ] ] ] ]

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
