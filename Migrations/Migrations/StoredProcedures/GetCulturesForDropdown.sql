CREATE OR ALTER PROCEDURE [dbo].GetCulturesForDropdown
AS
	SELECT 
		[Name], DisplayName 
	FROM SupportedCultures 
	ORDER BY IsDefault DESC, DisplayName;