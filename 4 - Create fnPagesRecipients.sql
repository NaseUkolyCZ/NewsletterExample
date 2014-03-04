USE [Newsletter]
GO
/****** Object:  UserDefinedFunction [dbo].[fnRecipients]    Script Date: 4. 3. 2014 17:25:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE FUNCTION [dbo].[fnPagesRecipients] ( @page int )
RETURNS TABLE 
AS
RETURN 
(
	SELECT * FROM (SELECT ROW_NUMBER() OVER (ORDER BY SmtpAddress) AS Row, * FROM Recipients) AS Paged WHERE Row > @page * 50 AND Row <= ( @page + 1 )* 50 
	-- 0..50
	-- 51..100
	-- 101..150
)

