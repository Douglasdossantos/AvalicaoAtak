using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvalicaoAtak.Interfaces
{
    public interface ILogService
    {
        Task GravarLogAsync(string mensagem);
    }
}
