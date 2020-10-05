namespace Shared.Security
{
    public class ClaimDto
    {
        public string Key { get; set; }
        public string Value { get; set; }

        public ClaimDto()
        {
        }

        public ClaimDto(string key, string value)
        {
            Key = key;
            Value = value;
        }
    }
}
