using InterTwitter.Enums;
using InterTwitter.Models;
using System.Collections.Generic;

namespace InterTwitter.Services
{
    public class MockManager : IMockManager
    {
        public IEnumerable<Post> GetPosts()
        {
            var mock = new List<Post>
            {
                new Post
                {
                    MediaType = EMediaType.Photo,
                    Text = "comming soon..."
                },
                new Post
                {
                    MediaType = EMediaType.Gif,
                    Text = "comming soon..."
                },
                new Post
                {
                    MediaType = EMediaType.Video,
                    Text = "comming soon..."
                },
                new Post
                {
                    MediaType = EMediaType.Photo,
                    Text = "comming soon..."
                },
                new Post
                {
                    MediaType = EMediaType.Video,
                    Text = "comming soon..."
                },
            };

            return mock;
        }
    }
}
