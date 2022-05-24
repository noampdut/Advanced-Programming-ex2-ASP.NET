using ex2.Services;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace ex2.Hubs
{
    public class MyHub : Hub
    {
        private IUsersService usersService;

        public MyHub(IUsersService userService) {
        
            usersService = userService;
        }
        public async Task sendMessage(string contact)
        {
            //await Clients.Client(contact).SendAsync(text);
            await Clients.All.SendAsync("getNewMessage");
        }
    }
}
