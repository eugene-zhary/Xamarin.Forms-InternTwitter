using InterTwitter.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

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
                    Text = "comming soon..."
                },
                new Post
                {
                    Text = "comming soon..."
                },
                new Post
                {
                    Text = "comming soon..."
                },
                new Post
                {
                    Text = "comming soon..."
                },
                new Post
                {
                    Text = "comming soon..."
                },
            };

            return mock;
        }
    }
}
