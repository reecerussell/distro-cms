CREATE OR ALTER PROCEDURE [dbo].GetDictionaryItem
	@Id NVARCHAR(36)
AS
	SELECT
		Id,
		CultureName,
		[Key],
		[Value]
	FROM DictionaryItems
	WHERE Id = @Id;