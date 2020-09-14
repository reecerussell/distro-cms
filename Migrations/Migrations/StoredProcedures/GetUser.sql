CREATE OR ALTER PROCEDURE [dbo].GetUser
	@UserId NVARCHAR(36)
AS
BEGIN
	SELECT 
		Id,
		Firstname,
		Lastname,
		Email
	FROM Users 
	WHERE Id = @UserId;
END
