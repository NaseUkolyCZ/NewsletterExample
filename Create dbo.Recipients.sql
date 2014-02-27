USE [Newsletter]
GO

/****** Object: Table [dbo].[Recipients] Script Date: 2/27/2014 10:50:12 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Recipients] (
    [SmtpAddress]     NVARCHAR (MAX) NOT NULL,
    [RecipientName]   NVARCHAR (MAX) NOT NULL,
    [FirstProvidedBy] NVARCHAR (MAX) NOT NULL,
    [EmailDomain]     NVARCHAR (MAX) NULL,
    [Excluded]        BIT            NULL
);


