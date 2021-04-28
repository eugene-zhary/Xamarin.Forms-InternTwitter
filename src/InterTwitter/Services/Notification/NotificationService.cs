using InterTwitter.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace InterTwitter.Services.Notification
{
    public class NotificationService : INotificationService
    {
        private readonly IMockService _mock;

        public NotificationService(IMockService mock)
        {
            _mock = mock;
        }

        #region -- INotificationService implementation --

        public async Task<AOResult<IEnumerable<Models.Notification>>> GetNotificationsAsync(Expression<Func<Models.Notification, bool>> predicate)
        {
            var result = new AOResult<IEnumerable<Models.Notification>>();

            try
            {
                await Task.Delay(100);

                result.SetSuccess(_mock.MockedNotifications.Where(predicate.Compile()));
            }
            catch (Exception e)
            {
                result.SetError($"{nameof(GetNotificationsAsync)}: exception", "Something went wrong", e);
            }

            return result;
        }

        public async Task<AOResult<int>> InsertNotificationAsync(Models.Notification notification)
        {
            var result = new AOResult<int>();

            try
            {
                var lastNotificationId = _mock.MockedNotifications.Last().Id;
                notification.Id = ++lastNotificationId;

                _mock.MockedNotifications.Add(notification);

                await Task.Delay(100);

                result.SetSuccess(notification.Id);
            }
            catch (Exception e)
            {
                result.SetError($"{nameof(InsertNotificationAsync)}: exception", "Something went wrong", e);
            }

            return result;
        }

        public async Task<AOResult<int>> DeleteNotificationAsync(Models.Notification notification)
        {
            var result = new AOResult<int>();

            try
            {
                await Task.Delay(100);

                ((List<Models.Notification>)_mock.MockedNotifications).RemoveAll(u => u.PostId == notification.PostId &&
                                                                                      u.ActorId == notification.ActorId &&
                                                                                      u.NotificationType == notification.NotificationType);
                
                result.SetSuccess(notification.Id);
            }
            catch (Exception e)
            {
                result.SetError($"{nameof(GetNotificationsAsync)}: exception", "Something went wrong", e);
            }

            return result;
        }

        #endregion
    }
}
