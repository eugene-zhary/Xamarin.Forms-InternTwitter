using InterTwitter.ViewModels.Posts;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using InterTwitter.Extensions;
using InterTwitter.Helpers;
using System.Threading.Tasks;

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
            return _mock.GetMockedPosts().ToViewModelCollection();
        }
    }
}
