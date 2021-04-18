using InterTwitter.Controls;
using InterTwitter.Resources;
using InterTwitter.Validators;
using Xamarin.Forms;

namespace InterTwitter.Behaviors
{
    class PasswordValidationBehavior : Behavior<RegisteringEntry>
    {
        #region -- Overrides --

        protected override void OnAttachedTo(RegisteringEntry bindable)
        {
            bindable.TextChanged += OnTextChanged;
            base.OnAttachedTo(bindable);
        }

        protected override void OnDetachingFrom(RegisteringEntry bindable)
        {
            bindable.TextChanged -= OnTextChanged;
            base.OnDetachingFrom(bindable);
        }

        #endregion

        #region -- Private helpers --

        private void OnTextChanged(object sender, TextChangedEventArgs e)
        {
            var password = e.NewTextValue;

            if (sender is RegisteringEntry registeringEntry)
            {
                if (password.Length < 6)
                {
                    SetEntryStyles(registeringEntry, true, Color.Red, Strings.PasswordLengthError);
                }
                else if (!StringValidator.Validate(password, StringValidator.Password))
                {
                    SetEntryStyles(registeringEntry, true, Color.Red, Strings.InvalidPasswordMessage);
                }
                else
                {
                    SetEntryStyles(registeringEntry, false, Color.Black);
                }
            }
        }

        private void SetEntryStyles(RegisteringEntry entry, bool isErrorTextVisible, Color underlineColor,
            string errorText = null)
        {
            entry.IsErrorTextVisible = isErrorTextVisible;
            entry.UnderlineColor = underlineColor;

            if (errorText != null)
            {
                entry.ErrorText = errorText;
            }
        }

        #endregion
    }
}
