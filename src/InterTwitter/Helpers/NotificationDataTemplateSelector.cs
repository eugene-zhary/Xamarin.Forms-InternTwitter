using InterTwitter.Enums;
using InterTwitter.ViewModels.Notification;
using Xamarin.Forms;

namespace InterTwitter.Views.Templates.Notifications
{
    class NotificationDataTemplateSelector : DataTemplateSelector
    {
        public DataTemplate EmptyPostDataTemplate { get; set; }
        public DataTemplate GalleryPostDataTemplate { get; set; }
        public DataTemplate GifPostDataTemplate { get; set; }
        public DataTemplate VideoPostDataTemplate { get; set; }

        #region -- Overrides --

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            var output = EmptyPostDataTemplate;

            if (item is NotificationViewModel notification)
            {
                switch (notification.Post.PostModel.MediaType)
                {
                    case EMediaType.Empty:
                        output = EmptyPostDataTemplate;
                        break;

                    case EMediaType.Gallery:
                    case EMediaType.Photo:
                        output = GalleryPostDataTemplate;
                        break;

                    case EMediaType.Gif:
                        output = GifPostDataTemplate;
                        break;

                    case EMediaType.Video:
                        output = VideoPostDataTemplate;
                        break;
                }
            }

            return output;
        }

        #endregion
    }
}
