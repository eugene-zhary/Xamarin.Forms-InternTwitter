using InterTwitter.ViewModels.Posts;
using System.Collections.Generic;
using InterTwitter.Extensions;

namespace InterTwitter.Services
{
    public class PostManager : IPostManager
    {
        private readonly IMockManager _mock;

        public PostManager(IMockManager mock)
        {
            _mock = mock;
        }

        public IEnumerable<BasePostViewModel> GetPosts()
        {
            return _mock.GetMockedPosts().ToViewModelCollection(_mock);
        }
    }
}
