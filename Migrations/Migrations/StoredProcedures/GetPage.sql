CREATE OR ALTER PROCEDURE [dbo].GetPage
	@PageId nvarchar(36),
	@CultureName nvarchar(5)
AS
	SELECT
		p.Id,
		p.[Url],
		p.Active,
		p.DateCreated,
		p.DateUpdated,
		t.Title,
		t.[Description],
		t.Content
	FROM Pages AS p
	INNER JOIN PageTranslations AS t ON t.PageId = p.Id
	WHERE t.CultureName = @CultureName AND p.Id = @PageId;