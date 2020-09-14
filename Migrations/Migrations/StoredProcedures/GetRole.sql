CREATE OR ALTER PROCEDURE [dbo].GetRole
	@Id NVARCHAR(36)
AS
SELECT
	Id,
	[Name],
	DateCreated,
	DateUpdated
FROM Roles
WHERE Id = @Id;