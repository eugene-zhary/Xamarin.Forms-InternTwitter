using InterTwitter.Enums;

namespace InterTwitter.Models
{
    public class Notification : IEntityBase
    {
        public int Id { get; set; }

        /// <summary>
        /// Who liked/bookmarked post
        /// </summary>
        public int ActorId { get; set; }

        public int PostId { get; set; }

        public ENotificationTypes NotificationType { get; set; }
    }
}
