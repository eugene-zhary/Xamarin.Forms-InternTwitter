using InterTwitter.Controls;
using InterTwitter.Resources;
using InterTwitter.Validators;
using Xamarin.Forms;

namespace InterTwitter.Behaviors
{
    class NameValidationBehavior : Behavior<RegisteringEntry>
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
            if (sender is RegisteringEntry registeringEntry)
            {
                var preparedInput = e.NewTextValue.Trim();

                if (string.IsNullOrWhiteSpace(preparedInput))
                {
                    SetEntryStyles(registeringEntry, true, Color.Red, Strings.BlankNameError);
                }
                else if (!StringValidator.Validate(preparedInput, StringValidator.Name))
                {
                    SetEntryStyles(registeringEntry, true, Color.Red, Strings.NameInputError);
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
