using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using InterTwitter.Helpers;
using InterTwitter.Models;
using InterTwitter.Resources;

namespace InterTwitter.Services.UserService
{
    public class UserService : IUserService
    {
        private IEnumerable<User> _mockedUsers;

        public UserService()
        {
            _mockedUsers = new List<User>
            {
                new User
                {
                    Id = 1,
                    Name = "Stas",
                    Email = "stas@gmail.com",
                    Password = "Qwert1",
                    BlockedUserIds = new List<int>(),
                    MutedUserIds = new List<int>(),
                    ProfileBackgroundImagePath = Constants.DEFAULT_PROFILE_IMAGE_PATH
                },
                new User
                {
                    Id = 2,
                    Name = "Vlad",
                    Email = "vlad@gmail.com",
                    Password = "Qwert2",
                    BlockedUserIds = new List<int>(),
                    MutedUserIds = new List<int>{3},
                    ProfileImagePath = "https://pbs.twimg.com/profile_images/1383184766959120385/MM9DHPWC_400x400.jpg",
                    ProfileBackgroundImagePath = Constants.DEFAULT_PROFILE_IMAGE_PATH
                },
                new User
                {
                    Id = 3,
                    Name = "Evgeny",
                    Email = "evgeny@gmail.com",
                    Password = "Qwert3",
                    BlockedUserIds = new List<int>{2},
                    MutedUserIds = new List<int>(),
                    ProfileImagePath = Constants.DEFAULT_PROFILE_IMAGE_PATH
                },
                new User
                {
                    Id = 4,
                    Name = "Ilya",
                    Email = "ilya@gmail.com",
                    Password = "Qwert4",
                    BlockedUserIds = new List<int>{1, 2, 3},
                    MutedUserIds = new List<int>{5},
                    ProfileImagePath = Constants.DEFAULT_PROFILE_IMAGE_PATH
                },
                new User
                {
                    Id = 5,
                    Name = "Alexey",
                    Email = "lesha@gmail.com",
                    Password = "Qwert5",
                    BlockedUserIds = new List<int>(),
                    MutedUserIds = new List<int>(),
                    ProfileImagePath = Constants.DEFAULT_PROFILE_IMAGE_PATH
                }
            };
        }

        #region -- IUserService implementation --

        public async Task<AOResult<IEnumerable<User>>> GetUsersAsync()
        {
            var result = new AOResult<IEnumerable<User>>();

            try
            {
                var users = await GetUserMocksAsync();

                if (users != null)
                {
                    result.SetSuccess(users);
                }
                else
                {
                    result.SetFailure("Users collection was null!");
                }
            }
            catch
            {
                result.SetFailure();
            }

            return result;
        }

        public async Task<AOResult<User>> GetUserAsync(int id)
        {
            var result = new AOResult<User>();

            try
            {
                var users = await GetUserMocksAsync();

                var resultUser = users.FirstOrDefault(user => user.Id == id);

                if (resultUser != null)
                {
                    result.SetSuccess(resultUser);
                }
                else
                {
                    result.SetFailure($"User with id {id} was not found!");
                }
            }
            catch
            {
                result.SetFailure();
            }

            return result;
        }

        public async Task<AOResult<User>> GetUserAsync(Expression<Func<User, bool>> predicate)
        {
            var result = new AOResult<User>();

            try
            {
                var users = await GetUserMocksAsync();

                var resultUser = users.Where(predicate.Compile()).FirstOrDefault();

                if (resultUser != null)
                {
                    result.SetSuccess(resultUser);
                }
                else
                {
                    result.SetFailure();
                }
            }
            catch
            {
                result.SetFailure();
            }

            return result;
        }

        public async Task<AOResult<int>> InsertUserAsync(User user)
        {
            var result = new AOResult<int>();

            try
            {
                var sameUser = _mockedUsers.FirstOrDefault(u => u.Email == user.Email);

                if (sameUser == null)
                {
                    var insertedUserId = await InsertUserToMockCollectionAsync(user);

                    result.SetSuccess(insertedUserId);
                }
                else
                {
                    result.SetFailure(Strings.SuchUserAlreadyExists);
                }
            }
            catch
            {
                result.SetFailure();
            }

            return result;
        }

        public async Task<AOResult<int>> UpdateUserAsync(User user)
        {
            var result = new AOResult<int>();

            try
            {
                var insertedUserId = await UpdateUserInMockCollectionAsync(user);

                result.SetSuccess(insertedUserId);
            }
            catch
            {
                result.SetFailure();
            }

            return result;
        }

        public async Task<AOResult<int>> DeleteUserAsync(User user)
        {
            var result = new AOResult<int>();

            try
            {
                var deletedId = await DeleteUserMockAsync(user);
                result.SetSuccess(deletedId);
            }
            catch
            {
                result.SetFailure();
            }

            return result;
        }

        #endregion

        #region -- Private helpers --

        private async Task<IEnumerable<User>> GetUserMocksAsync()
        {
            await Task.Delay(300);

            return _mockedUsers;
        }

        private async Task<int> InsertUserToMockCollectionAsync(User user)
        {
            var lastUserId = _mockedUsers.Last().Id;
            user.Id = ++lastUserId;

            (_mockedUsers as List<User>)?.Add(user);

            await Task.Delay(100);

            return user.Id;
        }

        private async Task<int> UpdateUserInMockCollectionAsync(User user)
        {
            var oldUser = _mockedUsers.FirstOrDefault(u => u.Id == user.Id);

            (_mockedUsers as List<User>)?.Remove(oldUser);

            (_mockedUsers as List<User>)?.Add(user);

            (_mockedUsers as List<User>)?.Sort((u1, u2) => u1.Id.CompareTo(u2.Id));

            await Task.Delay(300);

            return user.Id;
        }

        private async Task<int> DeleteUserMockAsync(User user)
        {
            _mockedUsers = _mockedUsers.Where(u => u.Id != user.Id);

            await Task.Delay(300);

            return user.Id;
        }

        #endregion
    }
}
