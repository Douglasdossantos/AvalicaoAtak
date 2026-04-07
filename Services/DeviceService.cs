using AvalicaoAtak.Interfaces;
using AvalicaoAtak.Model;
using System.Text.Json;

namespace AvalicaoAtak.Services
{
    public class DeviceService : IDeviceService
    {
        private readonly string _path;

        public DeviceService()
        {
            _path = Path.Combine(AppContext.BaseDirectory, "dispositivos.json");
        }

        public async Task<List<Dispositivo>> ObterDispositivosAsync()
        {
            var json = await File.ReadAllTextAsync(_path);
            return JsonSerializer.Deserialize<List<Dispositivo>>(json);
        }
    }
}
