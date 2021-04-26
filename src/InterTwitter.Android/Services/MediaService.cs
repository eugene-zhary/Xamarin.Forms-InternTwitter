using System;
using Android.App;
using Android.Content;
using Android.Widget;
using InterTwitter.Droid.Services;
using InterTwitter.Services;

[assembly: Xamarin.Forms.Dependency(typeof(MediaService))]
namespace InterTwitter.Droid.Services
{
    public class MediaService : IMediaService
    {
        public void SaveImageFromByte(byte[] imageByte, string fileName)
        {
            try
            {
                Java.IO.File storagePath = Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryPictures);
                string path = System.IO.Path.Combine(storagePath.ToString(), fileName);
                System.IO.File.WriteAllBytes(path, imageByte);
                var mediaScanIntent = new Intent(Intent.ActionMediaScannerScanFile);
                mediaScanIntent.SetData(Android.Net.Uri.FromFile(new Java.IO.File(path)));
            }
            catch(Exception ex)
            {
                Toast.MakeText(Application.Context, "Error on save image", ToastLength.Long);
            }
        }
    }
}