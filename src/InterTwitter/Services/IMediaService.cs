namespace InterTwitter.Services
{
    public interface IMediaService
    {
        void SaveImageFromByte(byte[] imageByte, string filename);
    }
}
