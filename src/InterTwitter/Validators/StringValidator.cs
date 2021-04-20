using System.Text.RegularExpressions;

namespace InterTwitter.Validators
{
    public class StringValidator
    {
        #region --- Private Constants ---

        // accepts only latin symbols
        private const string USERNAME_REGEX =
            @"^[A-Za-z ]{1,}$";

        private const string EMAIL_REGEX =
            @"^[\w\.]+@([\w-]+\.)+[\w-]{1,}$";

        // accepts string with at least one uppercase letter and one digit
        private const string PASSWORD_REGEX =
            @"^(?=.*\d)(?=.*[A-ZА-ЯЁ]).{6,}$";

        #endregion

        #region --- Properties ---

        private string Pattern { get; }

        public static StringValidator Name { get; }
        public static StringValidator Email { get; }
        public static StringValidator Password { get; }

        #endregion

        #region --- Constructors ---

        private StringValidator(string pattern)
        {
            Pattern = pattern;
        }

        static StringValidator()
        {
            Email = new StringValidator(EMAIL_REGEX);
            Password = new StringValidator(PASSWORD_REGEX);
            Name = new StringValidator(USERNAME_REGEX);
        }

        #endregion

        #region --- Public Methods ---

        public static bool Validate(string input, StringValidator type)
        {
            return !string.IsNullOrEmpty(input) &&
                   Regex.IsMatch(input, type.Pattern);
        }

        #endregion
    }
}
