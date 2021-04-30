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
        private readonly IMockService _mock;

        public UserService(IMockService mock)
        {
            _mock = mock;
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
            catch (Exception e)
            {
                result.SetError($"{nameof(GetUserAsync)}: exception", "Something went wrong", e);
            }

            return result;
        }

        public async Task<AOResult<IEnumerable<User>>> GetUsersAsync(Expression<Func<User, bool>> predicate)
        {
            var result = new AOResult<IEnumerable<User>>();

            try
            {
                var allUsers = (await GetUserMocksAsync()).ToList();

                result.SetSuccess(allUsers.Where(predicate.Compile()));
            }
            catch (Exception e)
            {
                result.SetError($"{nameof(GetUserAsync)}: exception", "Something went wrong", e);
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
            catch (Exception e)
            {
                result.SetError($"{nameof(GetUserAsync)}: exception", "Something went wrong", e);
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
            catch (Exception e)
            {
                result.SetError($"{nameof(GetUserAsync)}: exception", "Something went wrong", e);
            }

            return result;
        }

        public async Task<AOResult<int>> InsertUserAsync(User user)
        {
            var result = new AOResult<int>();

            try
            {
                var sameUser = _mock.MockedUsers.FirstOrDefault(u => u.Email == user.Email);

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
            catch (Exception e)
            {
                result.SetError($"{nameof(InsertUserAsync)}: exception", "Something went wrong", e);
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
            catch (Exception e)
            {
                result.SetError($"{nameof(UpdateUserAsync)}: exception", "Something went wrong", e);
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
            catch (Exception e)
            {
                result.SetError($"{nameof(DeleteUserAsync)}: exception", "Something went wrong", e);
            }

            return result;
        }

        #endregion

        #region -- Private helpers --

        private async Task<IEnumerable<User>> GetUserMocksAsync()
        {
            await Task.Delay(300);

            return _mock.MockedUsers;
        }

        private async Task<int> InsertUserToMockCollectionAsync(User user)
        {
            var lastUserId = _mock.MockedUsers.Last().Id;
            user.Id = ++lastUserId;

            _mock.MockedUsers?.Add(user);

            await Task.Delay(100);

            return user.Id;
        }

        private async Task<int> UpdateUserInMockCollectionAsync(User user)
        {
            var oldUser = _mock.MockedUsers.FirstOrDefault(u => u.Id == user.Id);

            _mock.MockedUsers?.Remove(oldUser);

            _mock.MockedUsers?.Add(user);

            _mock.MockedUsers.ToList()?.Sort((u1, u2) => u1.Id.CompareTo(u2.Id));

            await Task.Delay(300);

            return user.Id;
        }

        private async Task<int> DeleteUserMockAsync(User user)
        {
            _mock.MockedUsers.Remove(user);

            await Task.Delay(300);

            return user.Id;
        }

        #endregion
    }
}
