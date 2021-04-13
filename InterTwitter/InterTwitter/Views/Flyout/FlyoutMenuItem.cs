using System;

namespace InterTwitter.Views.Flyout
{
    public class FlyoutMenuItem
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string IconSource { get; set; }

        public Type TargetType { get; set; }
    }
}