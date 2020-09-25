namespace Shared
{
    public class ErrorMessages
    {
        // Page errors
        public const string PageTitleRequired = "PAGE:TITLE_REQUIRED";
        public const string PageTitleTooLong = "PAGE:TITLE_TOO_LONG";
        public const string PageDescriptionRequired = "PAGE:DESCRIPTION_TITLE";
        public const string PageDescriptionTooLong = "PAGE:DESCRIPTION_TOO_LONG";
        public const string PageContentRequired = "PAGE:CONTENT_REQUIRED";
        public const string PageContentTagNotAllowed = "PAGE:TAG_NOT_ALLOWED:{0}";
        public const string PageUrlTooLong = "PAGE:URL_TOO_LONG";
        public const string PageUrlCharacterNotAllowed = "PAGE:CHARACTER_NOT_ALLOWED:{0}";
        public const string PageNotFound = "PAGE:NOT_FOUND";
        public const string PageAlreadyActive = "PAGE:ALREADY_ACTIVE";
        public const string PageAlreadyInactive = "PAGE:ALREADY_INACTIVE";

        // User errors
        public const string UserFirstnameRequired = "USER:FIRSTNAME_REQUIRED";
        public const string UserFirstnameTooLong = "USER:FIRSTNAME_TOO_LONG";
        public const string UserLastnameRequired = "USER:LASTNAME_REQUIRED";
        public const string UserLastnameTooLong = "USER:LASTNAME_TOO_LONG";
        public const string UserEmailRequired = "USER:EMAIL_REQUIRED";
        public const string UserEmailTooLong = "USER:EMAIL_TOO_LONG";
        public const string UserEmailInvalid = "USER:EMAIL_INVALID";
        public const string UserPasswordRequired = "USER:PASSWORD_REQUIRED";
        public const string UserPasswordInvalid = "USER:PASSWORD_INVALID";
        public const string UserConfirmPasswordRequired = "USER:CONFIRM_PASSWORD_REQUIRED";
        public const string UserEmailTaken = "USER:EMAIL_TAKEN";
        public const string UserNotFound = "USER:NOT_FOUND";
        public const string UserAlreadyAssignedToRole = "USER:ALREADY_ASSIGNED_TO_ROLE";
        public const string UserNotAssignedToRole = "USER:NOT_ASSIGNED_TO_ROLE";

        // Password errors
        public const string PasswordRequired = "PASSWORD:REQUIRED";
        public const string PasswordTooShort = "PASSWORD:TOO_SHORT:{0}";
        public const string PasswordRequiresNonAlphanumeric = "PASSWORD:REQUIRES_NON_ALPHANUMERIC";
        public const string PasswordRequiresDigit = "PASSWORD:REQUIRES_DIGIT";
        public const string PasswordRequiresLowercase = "PASSWORD:REQUIRES_LOWERCASE";
        public const string PasswordRequiresUppercase = "PASSWORD:REQUIRES_UPPERCASE";
        public const string PasswordRequiresUniqueChars = "PASSWORD:REQUIRES_UNIQUE_CHARS:{0}";

        // Role errors
        public const string RoleNameRequired = "ROLE:NAME_REQUIRED";
        public const string RoleNameTooLong = "ROLE:NAME_TOO_LONG";
        public const string RoleNameTaken = "ROLE:NAME_TAKEN";
        public const string RoleNotFound = "ROLE:NOT_FOUND";

        // Dictionary errors
        public const string DictionaryKeyRequired = "DICTIONARY:KEY_REQUIRED";
        public const string DictionaryKeyTooLong = "DICTIONARY:KEY_TOO_LONG";
        public const string DictionaryDisplayNameRequired = "DICTIONARY:DISPLAY_NAME_REQUIRED";
        public const string DictionaryDisplayNameTooLong = "DICTIONARY:DISPLAY_NAME_TOO_LONG";
        public const string DictionaryValueRequired = "DICTIONARY:VALUE_REQUIRED";
        public const string DictionaryValueTooLong = "DICTIONARY:VENUE_TOO_LONG";
        public const string DictionaryItemAlreadyExists = "DICTIONARY:ITEM_ALREADY_EXISTS";
        public const string DictionaryItemNotFound = "DICTIONARY:ITEM_NOT_FOUND";
    }
}
