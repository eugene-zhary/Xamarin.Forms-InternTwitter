using System.Collections.Generic;

namespace InterTwitter.Models
{
    public class User : IEntityBase
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public string ProfileImagePath { get; set; }
        public string ProfileBackgroundImagePath { get; set; }

        public IEnumerable<int> MutedUserIds { get; set; }

        public IEnumerable<int> BlockedUserIds { get; set; }
    }
}
