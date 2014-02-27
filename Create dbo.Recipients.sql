DROP TABLE Recipients

CREATE TABLE [dbo].[Recipients] (
    [SmtpAddress]     NVARCHAR (255) NOT NULL,
    [RecipientName]   NVARCHAR (1024) NOT NULL,
    [FirstProvidedBy] NVARCHAR (255) NOT NULL,
    [EmailDomain]     NVARCHAR (255) NULL,
    [Excluded]        BIT            NULL, 
    CONSTRAINT [PK_Recipients] PRIMARY KEY ([SmtpAddress])
);

