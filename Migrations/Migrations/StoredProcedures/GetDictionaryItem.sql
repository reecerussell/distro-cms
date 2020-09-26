CREATE OR ALTER PROCEDURE [dbo].GetDictionaryItem
	@Id NVARCHAR(36)
AS
	SELECT
		Id,
		CultureId,
		[Key],
		DisplayName,
		[Value],
		DateCreated,
		DateUpdated
	FROM DictionaryItems
	WHERE Id = @Id;