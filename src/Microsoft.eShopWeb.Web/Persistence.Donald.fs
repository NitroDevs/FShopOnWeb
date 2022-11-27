namespace Microsoft.eShopWeb.Web

module Persistence_Donald =
  open System
  open System.Data
  open System.Threading.Tasks

  open Donald
  open Domain_Donald

  // Same as Db.Async.querySingle
  let getFirst<'T> (queryResult : Task<Result<'T list, DbError>>) : Task<Result<'T option, DbError>> =
    task {
      let! res = queryResult

      let first =
        match res with
        | Ok items -> items |> List.tryHead |> Ok
        | Error e -> Error e

      return first
    }

  let mapReader<'T> map (queryResult: Task<Result<IDataReader, DbError>>) : Task<Result<'T option, DbError>> =
    task {
      let! res = queryResult;

      return
        match res with
        | Ok rdr -> map rdr |> Ok
        | Error err -> Error err
    }

  let readCatalogBrand (rd : IDataReader) : CatalogBrand =
    // https://learn.microsoft.com/en-us/dotnet/fsharp/language-reference/nameof#nameof-with-instance-members
    { Id = nameof Unchecked.defaultof<CatalogBrand>.Id |> rd.ReadInt32
      Name = nameof Unchecked.defaultof<CatalogBrand>.Name |> rd.ReadString }

  let catalogBrands conn =
    let sql = "
      SELECT Id, Name
      FROM CatalogBrand
    "

    conn
    |> Db.newCommand sql
    |> Db.Async.query readCatalogBrand

  let readCatalogType (rd : IDataReader) : CatalogType =
    { Id = nameof Unchecked.defaultof<CatalogType>.Id |> rd.ReadInt32
      Name = nameof Unchecked.defaultof<CatalogType>.Name |> rd.ReadString }

  let catalogTypes conn =
    let sql = "
      SELECT Id, Name
      FROM CatalogTypes
    "

    conn
    |> Db.newCommand sql
    |> Db.Async.query readCatalogType

  let readCatalogItem (rd : IDataReader) : CatalogItem =
    { Id = nameof Unchecked.defaultof<CatalogItem>.Id |> rd.ReadGuid
      Name = nameof Unchecked.defaultof<CatalogItem>.Name |> rd.ReadString
      Description = nameof Unchecked.defaultof<CatalogItem>.Description |> rd.ReadString
      PictureUri = nameof Unchecked.defaultof<CatalogItem>.PictureUri |> rd.ReadString
      Price = nameof Unchecked.defaultof<CatalogItem>.Price |> rd.ReadDecimal
      CatalogBrandId = nameof Unchecked.defaultof<CatalogItem>.CatalogBrandId |> rd.ReadInt32
      CatalogBrand = {
        Id = nameof Unchecked.defaultof<CatalogItem>.CatalogBrandId |> rd.ReadInt32
        Name = "CatalogBrandName" |> rd.ReadString
      }
      CatalogTypeId = nameof Unchecked.defaultof<CatalogItem>.Description |> rd.ReadInt32
      CatalogType = {
        Id = nameof Unchecked.defaultof<CatalogItem>.CatalogTypeId |> rd.ReadInt32
        Name = "CatalogTypeName" |> rd.ReadString
      } }

  let catalogItemById conn id =
    let sql = "
      SELECT CI.*
        , CB.Name as CatalogBrandName
        , CT.Name as CatalogTypeName
      FROM CatalogItem CI
      INNER JOIN CatalogBrand CB
        ON CI.CatalogBrandId = CB.Id
      INNER JOIN CatalogTypes CT
        ON CI.CatalogTypeId = CT.Id
      WHERE CI.Id = @CatalogItemId
    "

    conn
    |> Db.newCommand sql
    |> Db.setParams [
        "CatalogItemId", SqlType.Guid id
      ]
    |> Db.Async.querySingle readCatalogItem

  let readBasket (rd: IDataReader) : Basket option =
    let readBasketItem (rd: IDataReader) : BasketItem =
      { Id = "BasketItemId" |> rd.ReadInt32
        CatalogItemId = nameof Unchecked.defaultof<BasketItem>.CatalogItemId |> rd.ReadGuid
        ProductName = nameof Unchecked.defaultof<BasketItem>.ProductName |> rd.ReadString
        UnitPrice = nameof Unchecked.defaultof<BasketItem>.UnitPrice |> rd.ReadDecimal
        OldUnitPrice = nameof Unchecked.defaultof<BasketItem>.OldUnitPrice |> rd.ReadDecimal
        Quantity = nameof Unchecked.defaultof<BasketItem>.Quantity |> rd.ReadInt32
        PictureUri = nameof Unchecked.defaultof<BasketItem>.PictureUri |> rd.ReadString
        BasketId = nameof Unchecked.defaultof<BasketItem>.BasketId |> rd.ReadGuid }

    let mutable basketId = option<Guid>.None
    let mutable buyerId = option<Guid>.None
    let mutable count = 0
    let items = new ResizeArray<BasketItem>()

    while rd.Read() do
      readBasketItem rd |> items.Add
      count <- count + 1

      if count = 1 then
        basketId <- "BasketId" |> rd.ReadGuid |> Some
        buyerId <- nameof Unchecked.defaultof<Basket>.BuyerId |> rd.ReadGuidOption

    match basketId with
    | Some id ->
      { Id = id
        Items = Seq.toList items
        BuyerId = buyerId } |> Some
    | None -> None

  let basketById conn id =
    let sql = "
      SELECT B.Id as BasketId
        , B.BuyderId
        , BI.Id as BasketItemId
      FROM Basket B
      INNER JOIN BasketItem BI
        ON B.Id = BI.BasketId
    "

    conn
    |> Db.newCommand sql
    |> Db.setParams [
      "BasketId", SqlType.Guid id
    ]
    |> Db.Async.read
    |> mapReader readBasket
