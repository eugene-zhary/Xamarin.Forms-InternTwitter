using InterTwitter.Enums;
using InterTwitter.ViewModels.Posts;
using Xamarin.Forms;

namespace InterTwitter.Views.Templates
{
    public class PostDataTemplateSelector : DataTemplateSelector
    {
        public DataTemplate PhotoPostDataTemplate { get; set; }
        public DataTemplate GalleryPostDataTemplate { get; set; }
        public DataTemplate VideoPostDataTemplate { get; set; }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            DataTemplate output = null;
            
            var postVM = item as BasePostViewModel;

            switch (postVM.PostModel.MediaType)
            {
                case EMediaType.Photo:
                    output = PhotoPostDataTemplate;
                    break;

                case EMediaType.Gallery:
                    output = GalleryPostDataTemplate;
                    break;

                case EMediaType.Video:
                    output = VideoPostDataTemplate;
                    break;
            }

            return output;
        }
    }
}
