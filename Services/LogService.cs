using AvalicaoAtak.Interfaces;

namespace AvalicaoAtak.Services
{
    public class LogService : ILogService
    {
        private readonly string _caminho = $"{DateTime.Now.ToString("ddMMyyyy")}-log.txt";

        public async Task GravarLogAsync(string mensagem)
        {
            await File.AppendAllTextAsync(_caminho, mensagem + Environment.NewLine);
        }
    }
}
