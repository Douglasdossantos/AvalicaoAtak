using AvalicaoAtak.Interfaces;
using System.Net.NetworkInformation;

namespace AvalicaoAtak.Services
{
    public class PingService : IPingService
    {
        public async Task<bool> VerificarAsync(string ip)
        {
            var ping = new Ping();
            var reply = await ping.SendPingAsync(ip);
            return reply.Status == IPStatus.Success;
        }
    }
}
