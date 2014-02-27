namespace FsWeb.Models

type Recipient() = 
    let mutable smtpAddress = ""
    let mutable recipientName = ""
    let mutable firstProvidedBy = ""
    member x.SmtpAddress with get() = smtpAddress and set v = smtpAddress <- v
    member x.RecipientName with get() = recipientName and set v = recipientName <- v
    member x.FirstProvidedBy with get() = firstProvidedBy and set v = firstProvidedBy <- v
