CREATE OR ALTER PROCEDURE [dbo].GetUser
	@UserId NVARCHAR(36)
AS
BEGIN
	SELECT 
		Id,
		Firstname,
		Lastname,
		Email,
		DateCreated,
		DateUpdated
	FROM Users 
	WHERE Id = @UserId;
END