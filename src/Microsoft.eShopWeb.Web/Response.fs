namespace Microsoft.eShopWeb.Web

open System.Threading.Tasks
open Falco.Markup
open Falco
open Microsoft.AspNetCore.Http

module Response =
  /// <summary>
  /// Returns a <see cref="HttpHandler" /> that processes an async rendering of an Html response
  /// </summary>
  let ofHtmlTask (xmlTask: Task<XmlNode>) (httpContext: HttpContext) =
    ((task {
      let! xml = xmlTask
      return! Response.ofHtml xml httpContext
    })
    :> Task)
