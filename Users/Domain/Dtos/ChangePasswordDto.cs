namespace Users.Domain.Dtos
{
    public class ChangePasswordDto
    {
        public string Id { get; set; }
        public string NewPassword { get; set; }
        public string CurrentPassword { get; set; }
    }
}
