
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [RealEstates].[Measurements_SelectByID] 
	 @ID int 
AS
BEGIN
	SELECT * FROM [RealEstates].[Measurements]
	 where MeasurementID = @ID;
END
GO
