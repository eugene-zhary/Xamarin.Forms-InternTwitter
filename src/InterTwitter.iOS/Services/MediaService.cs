using Foundation;
using InterTwitter.iOS.Services;
using InterTwitter.Services;
using System;
using UIKit;

[assembly: Xamarin.Forms.Dependency(typeof(MediaService))]
namespace InterTwitter.iOS.Services
{
    public class MediaService : IMediaService
    {
        public void SaveImageFromByte(byte[] imageByte, string fileName)
        {
            var imageData = new UIImage(NSData.FromArray(imageByte));
            imageData.SaveToPhotosAlbum((image, error) =>
            {
                if(error != null)
                {
                    Console.WriteLine(error.ToString());
                }
            });
        }
    }
}