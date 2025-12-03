using Microsoft.AspNetCore.SignalR;
using WatchMatchApi.Exceptions;

namespace WatchMatchApi.Hubs
{
    public class RoomHubFilter : IHubFilter
    {
        public async Task OnConnectedAsync(
            HubLifetimeContext context,
            Func<HubLifetimeContext, Task> next)
        {
            var http = context.Context.GetHttpContext();
            var roomId = http?.Request.Query["roomId"].ToString();
            var userId = context.Context.UserIdentifier;

            if (string.IsNullOrWhiteSpace(roomId))
                throw new MissingRoomIdException();

            if (string.IsNullOrWhiteSpace(userId))
                throw new MissingUserIdException();

            context.Context.Items["RoomId"] = roomId;
            context.Context.Items["UserId"] = userId;

            await next(context);
        }

        public async ValueTask<object?> InvokeMethodAsync(
            HubInvocationContext invocationContext,
            Func<HubInvocationContext, ValueTask<object?>> next)
        {
            return await next(invocationContext);
        }
    }


}
