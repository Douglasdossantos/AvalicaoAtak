using AvalicaoAtak.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvalicaoAtak.Interfaces
{
    public interface IDeviceService
    {
        Task<List<Dispositivo>> ObterDispositivosAsync();
    }
}
