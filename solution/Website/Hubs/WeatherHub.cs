using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace Website.Hubs
{
    public class WeatherHub : Hub
    {
        public async Task NotifyWebUsers(string user, string message)
        {
            System.Console.WriteLine($"Getting in NotifyWebUsers: {user}, {message}");

            // The first argument to SendAsync needs to match the string defined in the handler in our client-side JS code.
            await Clients.All.SendAsync("DisplayNotification", user, message);
        }
    }
}
