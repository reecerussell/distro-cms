CREATE OR ALTER PROCEDURE [dbo].GetCulturesForSetup
AS
SELECT
	[Name],
	IsDefault
FROM SupportedCultures;