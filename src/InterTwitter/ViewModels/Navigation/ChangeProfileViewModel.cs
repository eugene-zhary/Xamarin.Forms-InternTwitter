using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace InterTwitter.ViewModels.Navigation
{
    public class ChangeProfileViewModel : BaseViewModel
    {
        public ChangeProfileViewModel(INavigationService navigationService) : base(navigationService)
        {
        }
    }
}
