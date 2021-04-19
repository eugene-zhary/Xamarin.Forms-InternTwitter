namespace InterTwitter.Services.Authorization
{
    public interface IAuthorizationService
    {
        bool IsAuthorized { get; }

        int GetCurrentUserId { get; }

        void Authorize(int id);

        void UnAuthorize();
    }
}
