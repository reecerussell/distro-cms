CREATE OR ALTER PROCEDURE [dbo].GetCulturesForDropdown
AS
	SELECT 
		Id, [Name], DisplayName 
	FROM SupportedCultures 
	ORDER BY IsDefault DESC, DisplayName;