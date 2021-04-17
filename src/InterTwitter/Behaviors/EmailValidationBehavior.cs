using InterTwitter.Controls;
using InterTwitter.Resources;
using InterTwitter.Validators;
using Xamarin.Forms;

namespace InterTwitter.Behaviors
{
    class EmailValidationBehavior : Behavior<RegisteringEntry>
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
                    registeringEntry.IsErrorTextVisible = true;
                    registeringEntry.UnderlineColor = Color.Red;
                    registeringEntry.ErrorText = Strings.CannotBeBlank;
                }
                else if (!StringValidator.Validate(preparedInput, StringValidator.Email))
                {
                    registeringEntry.IsErrorTextVisible = true;
                    registeringEntry.UnderlineColor = Color.Red;
                    registeringEntry.ErrorText = Strings.EmailInputError;
                }
                else
                {
                    registeringEntry.IsErrorTextVisible = false;
                    registeringEntry.UnderlineColor = Color.Black;
                }
            }
        }

        #endregion
    }
}
