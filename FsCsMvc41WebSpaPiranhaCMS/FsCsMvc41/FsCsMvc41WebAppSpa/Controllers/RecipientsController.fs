namespace FsWeb.Controllers

open System.Collections.Generic
open System.Web
open System.Web.Mvc
open System.Net.Http
open System.Web.Http
open FsWeb.Models
open System.Data
open System.Data.Linq
open Microsoft.FSharp.Data.TypeProviders
open Microsoft.FSharp.Linq

type dbSchema = SqlDataConnection<"Data Source=(localdb)\Projects;Initial Catalog=Newsletter;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False">

type RecipientsController() =
    inherit ApiController()

    let db = dbSchema.GetDataContext()

    let recipients =
        query {
            for row in db.Recipients do
            select row
        }

    let countOfrecipients =
        query {
                for row in db.Recipients do
                count
        }

    // GET /api/contacts
    // http://msdn.microsoft.com/en-us/library/hh361033.aspx & http://msdn.microsoft.com/en-us/library/dd233209.aspx
    member x.Get() = 
        //seq { for row in recipients -> new Recipient(SmtpAddress = row.SmtpAddress, RecipientName = row.RecipientName, FirstProvidedBy = row.FirstProvidedBy ) }
        x.Get(0)

    member x.Get(id:int) = 
        let nullableInt = new System.Nullable<int>(id)

        seq { for row in  query {
                for row in db.FnPagesRecipients(nullableInt) do
                select row
            } -> new Recipient(SmtpAddress = row.SmtpAddress, RecipientName = row.RecipientName, FirstProvidedBy = row.FirstProvidedBy ) }

    // POST /api/contacts
    member x.Post ([<FromBody>] recipient:Recipient) = 
        let newRecipient = new dbSchema.ServiceTypes.Recipients( SmtpAddress=recipient.SmtpAddress, RecipientName=recipient.RecipientName, FirstProvidedBy = recipient.FirstProvidedBy )
        db.Recipients.InsertOnSubmit(newRecipient)
        db.DataContext.SubmitChanges()
        x.Get()

    // http://stackoverflow.com/questions/14481592/webapi-no-action-was-found-on-the-controller
    member x.Delete (id:string) =
        let deleteRowsFrom (table:Table<_>) rows =
            table.DeleteAllOnSubmit(rows)

        query {
            for rows in db.Recipients do
            where (rows.SmtpAddress = id)
            select rows
            }
        |> deleteRowsFrom db.Recipients
        db.DataContext.SubmitChanges()
        x.Get()
