using System;
using System.Collections.Generic;
using InterTwitter.Enums;

namespace InterTwitter.Models
{
    public class Post : IEntityBase
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public string Text { get; set; }

        public DateTime CreationDateTime { get; set; }

        /// <summary>
        /// Collection of user id's, who liked this post
        /// </summary>
        public IList<int> LikedUserIds { get; set; }

        /// <summary>
        /// Collection of user id's, who bookmarked this post
        /// </summary>
        public IList<int> BookmarkedUserIds { get; set; }

        public EMediaType MediaType { get; set; }

        public IEnumerable<string> MediaPaths { get; set; }
    }
}
