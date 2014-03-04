USE [Newsletter]
GO

/****** Object: SqlProcedure [dbo].[sp_InsertOrUpdate] Script Date: 4. 3. 2014 6:44:15 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE sp_InsertOrUpdate
	@smtpAddress nvarchar(max), 
	@RecipientName nvarchar(max) 
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	begin tran
	   IF EXISTS (SELECT 1 FROM [dbo].[Recipients]  WHERE smtpAddress = @smtpAddress)
	   begin
		 update [dbo].[Recipients] with (serializable) set RecipientName = @RecipientName where smtpAddress = @smtpAddress
		 and @RecipientName <> @smtpAddress
	   end
	   else
	   begin
		  insert [dbo].[Recipients] (RecipientName, smtpAddress, FirstProvidedBy ) values (@RecipientName,@smtpAddress, SYSTEM_USER)
	   end
	commit tran

END
