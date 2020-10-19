namespace Users.Domain.Dtos
{
    public class NewlyCreatedUserDto : UserDto
    {
        public NewlyCreatedUserDto(UserDto dto)
        {
            Id = dto.Id;
            Firstname = dto.Firstname;
            Lastname = dto.Lastname;
            Email = dto.Email;
            DateCreated = dto.DateCreated;
            DateUpdated = dto.DateUpdated;
        }

        public string Password { get; set; }
    }
}
