
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [RealEstates].[Measurements_Update] 
@Id int,
@NameAR nvarchar(150),
@NameEN nvarchar(150)
AS
BEGIN
update [RealEstates].[Measurements]
set NameAR = @NameAR,
NameEN=@NameEN
where MeasurementID= @Id

END
GO
