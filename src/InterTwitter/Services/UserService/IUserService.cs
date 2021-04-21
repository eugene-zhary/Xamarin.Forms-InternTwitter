using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using InterTwitter.Helpers;
using InterTwitter.Models;

namespace InterTwitter.Services.UserService
{
    public interface IUserService
    {
        Task<AOResult<IEnumerable<User>>> GetUsersAsync();

        Task<AOResult<User>> GetUserAsync(int id);
        Task<AOResult<User>> GetUserAsync(Expression<Func<User, bool>> predicate);

        Task<AOResult<int>> InsertUserAsync(User user);
        Task<AOResult<int>> UpdateUserAsync(User user);

        Task<AOResult<int>> DeleteUserAsync(User user);
    }
}
