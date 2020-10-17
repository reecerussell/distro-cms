using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Passwords
{
    public class PasswordGeneratorOptions
    {
        public string UppercaseCharacters { get; set; } = "abcdefghijklmnopqrstuvwxyz";
        public string LowercaseCharacters { get; set; } = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        public string Digits { get; set; } = "0123456789";
        public string SpecialCharacters { get; set; } = "-_#@$*";
    }
}
