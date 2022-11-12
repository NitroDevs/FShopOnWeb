namespace Microsoft.eShopWeb.Web

module Request =
  open System.Threading.Tasks
  open Microsoft.AspNetCore.Http
  open Falco

  let mapFormAsync
    (mapAsync : FormCollectionReader -> Task<'T>)
    (next : 'T -> HttpHandler) =
    fun (ctx: HttpContext) -> (task {
      let! form = Request.getForm ctx
      let! res = mapAsync form
      return! next res ctx
    }:> Task)
