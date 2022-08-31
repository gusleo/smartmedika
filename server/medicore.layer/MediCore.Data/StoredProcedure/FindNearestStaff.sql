-- ================================================
-- Template generated from Template Explorer using:
-- Create Procedure (New Menu).SQL
--
-- Use the Specify Values for Template Parameters 
-- command (Ctrl-Shift-M) to fill in the parameter 
-- values below.
--
-- This block of comments will not be included in
-- the definition of the procedure.
-- ================================================
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		GusLeo
-- Create date: 02/08/2017
-- Description:	FindNearestStaff
-- =============================================
ALTER PROCEDURE FindNearestStaff 
	-- Add the parameters for the stored procedure here
	@longitude decimal, 
	@latitude decimal,
	@radius int,
	@staffType int,
	@search	varchar(max),
	@pageIndex int,
	@pageSize int
AS
BEGIN
	DECLARE @searchLocation geography = geography::Point(@longitude, @latitude, 4326);
	
	DECLARE @MedicalStaff TABLE(
		MedicalStaffId INT,--just dummy value, used on oder by offset
		Id INT,
		Title VARCHAR(10),
		FirstName VARCHAR(100),
		LastName VARCHAR(100),
		MedicalStaffRegisteredNumber VARCHAR(100),
		PhoneCodeArea VARCHAR(5),
		PhoneNumber VARCHAR(20),
		StaffType INT
	)

	INSERT INTO @MedicalStaff
		SELECT 0 AS MedicalStaffId, MedicalStaff1.Id, MedicalStaff1.Title, MedicalStaff1.FirstName, MedicalStaff1.LastName, MedicalStaff1.MedicalStaffRegisteredNumber, 
				MedicalStaff1.PhoneCodeArea, MedicalStaff1.PhoneNumber, MedicalStaff1.StaffType
			FROM HospitalMedicalStaff HospitalMedicalStaf1
				INNER JOIN Hospital Hospital1 ON HospitalMedicalStaf1.HospitalId = Hospital1.Id
				INNER JOIN MedicalStaff MedicalStaff1 ON HospitalMedicalStaf1.MedicalStaffId = MedicalStaff1.Id AND MedicalStaff1.StaffType = @staffType AND MedicalStaff1.[Status] = 1
				INNER JOIN MedicalStaffSpecialistMap MSSM ON MSSM.MedicalStaffId = MedicalStaff1.Id
				INNER JOIN MedicalStaffSpecialist MSS ON MSSM.MedicalStaffSpecialistId = MSS.Id
			WHERE MedicalStaff1.FirstName + ' ' + MedicalStaff1.LastName LIKE '%' + @search + '%'
				OR Hospital1.Name LIKE '%' + @search + '%'
				OR MSS.Name LIKE '%' + @search + '%'
				AND HospitalMedicalStaf1.[Status] = 1
				AND @searchLocation.STDistance(geography::Point(Hospital1.Longitude, Hospital1.Latitude, 4326)) <= @radius			
			ORDER BY @searchLocation.STDistance(geography::Point(Hospital1.Longitude, Hospital1.Latitude, 4326)) DESC

		
		SELECT MS.Id, MS.Title, MS.FirstName, MS.LastName, MS.MedicalStaffRegisteredNumber, 
				MS.PhoneCodeArea, MS.PhoneNumber, MS.StaffType
		FROM @MedicalStaff MS
		GROUP BY MS.Id, MS.Title, MS.FirstName, MS.LastName, MS.MedicalStaffRegisteredNumber, 
				MS.PhoneCodeArea, MS.PhoneNumber, MS.StaffType, MS.MedicalStaffId
		ORDER BY MS.MedicalStaffId
			OFFSET ((@PageIndex - 1) * @PageSize) ROWS 
						FETCH NEXT @PageSize ROWS ONLY


			
END
GO