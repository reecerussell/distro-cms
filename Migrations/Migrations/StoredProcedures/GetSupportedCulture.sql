CREATE OR ALTER PROCEDURE [dbo].GetSupportedCulture
	@Id NVARCHAR(36)
AS
	SELECT
		Id,
		DateCreated,
		DateUpdated,
		[Name],
		DisplayName,
		IsDefault
	FROM [dbo].SupportedCultures
	WHERE Id = @Id;