
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [RealEstates].[Measurements_Delete] 
@MeasurementID int

AS
BEGIN
	DELETE FROM [RealEstates].[Measurements] WHERE MeasurementID = @MeasurementID ;
END
GO
