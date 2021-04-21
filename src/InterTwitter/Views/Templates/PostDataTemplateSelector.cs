﻿using InterTwitter.Enums;
using InterTwitter.ViewModels.Posts;
using Xamarin.Forms;

namespace InterTwitter.Views.Templates
{
    public class PostDataTemplateSelector : DataTemplateSelector
    {
        public DataTemplate PhotoPostDataTemplate { get; set; }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            DataTemplate output = null;
            
            var postVM = item as BasePostViewModel;

            switch (postVM.PostModel.MediaType)
            {
                case EMediaType.Photo:
                    output = PhotoPostDataTemplate;
                    break;
            }

            return output;
        }
    }
}
