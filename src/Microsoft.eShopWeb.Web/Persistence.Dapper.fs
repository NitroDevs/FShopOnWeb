namespace Microsoft.eShopWeb.Web

module Persistence_Dapper =
  open Domain_Dapper
  open Dapper.FSharp.MSSQL

  let catalogBrandTable = table<CatalogBrand>
