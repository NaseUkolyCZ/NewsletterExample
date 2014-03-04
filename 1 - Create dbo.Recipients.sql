DROP TABLE Recipients

CREATE TABLE [dbo].[Recipients] (
    [SmtpAddress]     NVARCHAR (255) NOT NULL,
    [RecipientName]   NVARCHAR (1024) NOT NULL,
    [FirstProvidedBy] NVARCHAR (255) NOT NULL,
	[SampleSubject1]     NVARCHAR (255) NULL,
	[SampleSubject2]     NVARCHAR (255) NULL,
	[SampleSubject3]     NVARCHAR (255) NULL,
    [EmailDomain]     NVARCHAR (255) NULL,
    [Excluded]        BIT            NULL, 
    CONSTRAINT [PK_Recipients] PRIMARY KEY ([SmtpAddress])
);

