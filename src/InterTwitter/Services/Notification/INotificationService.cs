using InterTwitter.Helpers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq.Expressions;

namespace InterTwitter.Services.Notification
{
    public interface INotificationService
    {
        Task<AOResult<IEnumerable<Models.Notification>>> GetNotificationsAsync(Expression<Func<Models.Notification, bool>> predicate);

        Task<AOResult<int>> InsertNotificationAsync(Models.Notification notification);

        Task<AOResult<int>> DeleteNotificationAsync(Models.Notification notification);
    }
}
