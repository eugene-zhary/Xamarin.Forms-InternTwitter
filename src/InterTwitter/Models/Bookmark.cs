namespace InterTwitter.Models
{
    public class Bookmark : IEntityBase
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public int PostId { get; set; }
    }
}
