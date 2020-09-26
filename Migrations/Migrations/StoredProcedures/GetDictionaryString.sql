CREATE OR ALTER PROCEDURE [dbo].GetDictionaryString
	@Key NVARCHAR(45),
	@CultureName NVARCHAR(14)
AS
	SELECT [Value] 
	FROM DictionaryItems 
	WHERE [Key] = @Key
		AND CultureName = @CultureName;