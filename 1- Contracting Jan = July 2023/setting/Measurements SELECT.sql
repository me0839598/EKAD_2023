
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [RealEstates].[Measurements_SelectAll] 

AS
BEGIN
	SELECT * FROM [RealEstates].[Measurements];
END
GO
