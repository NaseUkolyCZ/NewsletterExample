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

type dbSchema = SqlDataConnection<"Data Source=nutfs01;Initial Catalog=Newsletter;Integrated Security=SSPI;">

type RecipientsController() =
    inherit ApiController()

    let db = dbSchema.GetDataContext()

    let recipients =
        query {
            for row in db.Recipients do
            select row
        }

    // GET /api/contacts
    // http://msdn.microsoft.com/en-us/library/hh361033.aspx & http://msdn.microsoft.com/en-us/library/dd233209.aspx
    member x.Get() = 
        seq { for row in recipients -> new Recipient(SmtpAddress = row.SmtpAddress, RecipientName = row.RecipientName, FirstProvidedBy = row.FirstProvidedBy ) }

    // POST /api/contacts
    member x.Post ([<FromBody>] recipient:Recipient) = 
        let newRecipient = new dbSchema.ServiceTypes.Recipients( SmtpAddress=recipient.SmtpAddress, RecipientName=recipient.RecipientName, FirstProvidedBy = recipient.FirstProvidedBy )
        db.Recipients.InsertOnSubmit(newRecipient)
        db.DataContext.SubmitChanges()
        x.Get()

    member x.Delete ([<FromBody>] recipient:Recipient) =
        let toBeDeletedRecipient = new dbSchema.ServiceTypes.Recipients( SmtpAddress=recipient.SmtpAddress, RecipientName=recipient.RecipientName, FirstProvidedBy = recipient.FirstProvidedBy )
        db.Recipients.DeleteOnSubmit(toBeDeletedRecipient)
        db.DataContext.SubmitChanges()
        x.Get()
