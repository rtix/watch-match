using Microsoft.AspNetCore.SignalR;

namespace WatchMatchApi.Services
{
    public class GuestUserIdProvider : IUserIdProvider
    {
        public string? GetUserId(HubConnectionContext connection)
        {
            var httpContext = connection.GetHttpContext();
            return httpContext?.Request.Query["access_token"].FirstOrDefault();
        }
    }
}
