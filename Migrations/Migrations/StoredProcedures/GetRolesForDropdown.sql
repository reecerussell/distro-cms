CREATE OR ALTER PROCEDURE [dbo].GetRolesForDropdown
AS
SELECT [Id], [Name] FROM [Roles] ORDER BY [Name]