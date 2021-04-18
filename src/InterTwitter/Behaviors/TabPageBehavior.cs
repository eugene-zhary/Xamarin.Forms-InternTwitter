using Prism.Behaviors;
using Prism.Common;
using Prism.Navigation;
using Xamarin.Forms;

namespace InterTwitter.Behaviors
{
    public class TabPageBehavior : BehaviorBase<ContentPage>
    {
        protected override void OnAttachedTo(ContentPage bindable)
        {
            base.OnAttachedTo(bindable);

            var parameters = new NavigationParameters();
            PageUtilities.OnInitializedAsync(bindable, parameters);
        }
    }
}
