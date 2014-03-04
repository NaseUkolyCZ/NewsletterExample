USE [Newsletter]
GO

drop procedure sp_UpdateSampleSubject

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
CREATE PROCEDURE sp_UpdateSampleSubject
	@smtpAddress nvarchar(max), 
	@SampleSubject nvarchar(max) 
AS
BEGIN
	SET NOCOUNT ON;

	begin tran
		 update dbo.Recipients with (serializable)
		 set [SampleSubject1] = ISNULL( [SampleSubject1], @SampleSubject ), 
		 [SampleSubject2] = CASE WHEN [SampleSubject1] is NULL THEN NULL ELSE ISNULL( [SampleSubject2], @SampleSubject ) END,
		 [SampleSubject3] = CASE WHEN [SampleSubject2] is NULL THEN NULL ELSE ISNULL( [SampleSubject3], @SampleSubject ) END
		 where smtpAddress = @smtpAddress
	commit tran
END
