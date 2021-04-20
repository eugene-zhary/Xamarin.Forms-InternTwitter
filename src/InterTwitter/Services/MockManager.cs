using InterTwitter.Enums;
using InterTwitter.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace InterTwitter.Services
{
    public class MockManager : IMockManager
    {
        private List<Post> _mockedPosts;
        private List<User> _mockedUsers;

        public MockManager()
        {
            InitializeUserMock();
            InitializePostMock();
        }

        #region -- IMockManager implementation --

        public IEnumerable<Post> GetMockedPosts(Func<Post, bool> predicate = null)
        {
            IEnumerable<Post> output = null;

            output = (predicate == null) ? _mockedPosts : _mockedPosts.Where(predicate);

            return output;
        }

        public IEnumerable<User> GetMockedUsers(Func<User, bool> predicate = null)
        {
            IEnumerable<User> output = null;

            output = (predicate == null) ? _mockedUsers : _mockedUsers.Where(predicate);

            return output;
        }

        #endregion

        #region -- Private helpers --

        private void InitializeUserMock()
        {
            _mockedUsers = new List<User>
            {
                new User
                {
                    Id = 1,
                    Name = "Jhon Lennon",
                    Email ="jhon.lennon@email.com",
                    Password = "1111",
                    ProfileImagePath = "https://pbs.twimg.com/profile_images/428144059446599680/KSLIq_WY_400x400.jpeg",
                    MutedUserIds = new List<int>{2},
                    BlockedUserIds = new List<int>{2}
                },
                new User
                {
                    Id = 2,
                    Name = "Elvis Presley",
                    Email ="elvis.presley@email.com",
                    Password = "1111",
                    ProfileImagePath = "https://pbs.twimg.com/profile_images/424180229016268800/k1UVHgjq_400x400.jpeg",
                    MutedUserIds = new List<int>{},
                    BlockedUserIds = new List<int>{}
                },
                new User
                {
                    Id = 3,
                    Name = "Kurt Cobain",
                    Email ="kurt.cobain@email.com",
                    Password = "1111",
                    ProfileImagePath ="https://pbs.twimg.com/profile_images/561606018795601922/7OQvyZsl_400x400.jpeg",
                    MutedUserIds = new List<int>{},
                    BlockedUserIds = new List<int>{}
                },
                new User
                {
                    Id = 4,
                    Name = "Ozzy Osbourne",
                    Email ="ozzy.osbourne@email.com",
                    Password = "1111",
                    ProfileImagePath = "https://pbs.twimg.com/profile_images/1215311553152438273/ewie0ixT_400x400.jpg",
                },
                new User
                {
                    Id = 5,
                    Name = "Michael Jackson",
                    Email ="michael.jackson@email.com",
                    Password = "1111",
                    ProfileImagePath = "https://pbs.twimg.com/profile_images/556179314660478976/l_MadSiU_400x400.jpeg",
                    MutedUserIds = new List<int>{},
                    BlockedUserIds = new List<int>{}
                },
                new User
                {
                    Id = 6,
                    Name = "Johnny Cash",
                    Email ="johnny.cash@email.com",
                    Password = "1111",
                    ProfileImagePath = "https://pbs.twimg.com/profile_images/961615866944040961/U7RDW7lZ_400x400.jpg",
                    MutedUserIds = new List<int>{},
                    BlockedUserIds = new List<int>{}
                },
            };

        }

        // random pictures :
        //https://picsum.photos/400/400/?random
        private void InitializePostMock()
        {
            _mockedPosts = new List<Post>
            {
                new Post
                {
                    Id = 1,
                    UserId = 2,
                    MediaType = EMediaType.Photo,
                    Text = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Donec erat tellus, egestas in hendrerit et, iaculis non ipsum. Nunc euismod justo eu nisi tristique mollis.",
                    MediaPaths = new List<string>
                    {
                        "https://i.picsum.photos/id/959/400/400.jpg?hmac=j0GPhzbNUAQj4NRpMirUEUwNEbvFCxEiUPyXucYpBHg"
                    },
                    LikedUserIds = new List<int>{1,2,3},
                    BookmarkedUserIds = new List<int>{2,3},
                    CreationDateTime = new DateTime(2021,4,20, 15,40,00)
                },
                new Post
                {
                    Id =2,
                    UserId = 3,
                    MediaType = EMediaType.Photo,
                    Text = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Donec erat tellus, egestas in hendrerit et, iaculis non ipsum. Nunc euismod justo eu nisi tristique mollis.",
                    MediaPaths = new List<string>
                    {
                        "https://i.picsum.photos/id/565/400/400.jpg?hmac=u7Whpa6_lgizpVIu4_25vL1BSD-lz3EZl0Ipaj4445E"
                    },
                    LikedUserIds = new List<int>{1,2},
                    BookmarkedUserIds = new List<int>{1,2},
                    CreationDateTime = new DateTime(2021,4,18, 15,40,00)
                },
                new Post
                {
                    Id = 3,
                    UserId = 4,
                    MediaType = EMediaType.Photo,
                    Text = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Donec erat tellus, egestas in hendrerit et, iaculis non ipsum. Nunc euismod justo eu nisi tristique mollis.",
                    MediaPaths = new List<string>
                    {
                        "https://i.picsum.photos/id/1040/400/400.jpg?hmac=vOoeuciVz-_9Ejrgf5PpfD1ic7vTu0GYm3kn8BxdrHs"
                    },
                    LikedUserIds = new List<int>{1,2,3,4,5,6 },
                    BookmarkedUserIds = new List<int>{3,4},
                    CreationDateTime = new DateTime(2021,3,20, 15,40,00)
                },
                new Post
                {
                    Id = 4,
                    UserId =1,
                    MediaType = EMediaType.Photo,
                    Text = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Donec erat tellus, egestas in hendrerit et, iaculis non ipsum. Nunc euismod justo eu nisi tristique mollis.",
                    MediaPaths = new List<string>
                    {
                        "https://i.picsum.photos/id/904/400/400.jpg?hmac=f6UILh_efUs-iFYkEzhc1EFcHjQBgidgfhFwBbScofs"
                    },
                    LikedUserIds = new List < int > { 1 },
                    BookmarkedUserIds = new List<int>{5,1},
                    CreationDateTime = new DateTime(2021,4,20, 12,40,00)
                },
                new Post
                {
                    Id = 4,
                    UserId = 5,
                    MediaType = EMediaType.Photo,
                    Text = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Donec erat tellus, egestas in hendrerit et, iaculis non ipsum. Nunc euismod justo eu nisi tristique mollis.",
                    MediaPaths = new List<string>
                    {
                        "https://i.picsum.photos/id/998/400/400.jpg?hmac=WetZ0aq7zNlX1LHIsGSzZwV5MdHfytypy_ji8IU5ocE"
                    },
                    LikedUserIds = new List < int > { 1,3 },
                    BookmarkedUserIds = new List<int>{1,2},
                    CreationDateTime = new DateTime(2021,4,20, 12,40,00)
                },
            };
        }

        #endregion
    }
}
