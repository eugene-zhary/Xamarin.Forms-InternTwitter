using InterTwitter.Enums;
using InterTwitter.Models;
using System.Collections.Generic;

namespace InterTwitter.Services
{
    public class MockManager : IMockManager
    {
        // random pictures :
        //https://picsum.photos/400/400/?random

        public IEnumerable<Post> GetMockedPosts()
        {
            var mock = new List<Post>
            {
                new Post
                {
                    MediaType = EMediaType.Photo,
                    Text = "comming soon...",
                    MediaPaths = new List<string>
                    {
                        "https://i.picsum.photos/id/959/400/400.jpg?hmac=j0GPhzbNUAQj4NRpMirUEUwNEbvFCxEiUPyXucYpBHg"
                    }
                },
                new Post
                {
                    MediaType = EMediaType.Photo,
                    Text = "comming soon...",
                    MediaPaths = new List<string>
                    {
                        "https://i.picsum.photos/id/565/400/400.jpg?hmac=u7Whpa6_lgizpVIu4_25vL1BSD-lz3EZl0Ipaj4445E"
                    }
                },
                new Post
                {
                    MediaType = EMediaType.Photo,
                    Text = "comming soon...",
                    MediaPaths = new List<string>
                    {
                        "https://i.picsum.photos/id/1040/400/400.jpg?hmac=vOoeuciVz-_9Ejrgf5PpfD1ic7vTu0GYm3kn8BxdrHs"
                    }
                },
                new Post
                {
                    MediaType = EMediaType.Photo,
                    Text = "comming soon...",
                    MediaPaths = new List<string>
                    {
                        "https://i.picsum.photos/id/904/400/400.jpg?hmac=f6UILh_efUs-iFYkEzhc1EFcHjQBgidgfhFwBbScofs"
                    }
                },
            };

            return mock;
        }
    }
}
