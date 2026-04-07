using AvalicaoAtak.Interfaces;

namespace AvalicaoAtak
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IDeviceService _deviceService;
        private readonly IPingService _pingService;
        private readonly ILogService _logService;
        private readonly IConfiguration _configuration;

        public Worker(
            ILogger<Worker> logger,
            IDeviceService deviceService,
            IPingService pingService,
            ILogService logService,
            IConfiguration configuration)
        {
            _logger = logger;
            _deviceService = deviceService;
            _pingService = pingService;
            _logService = logService;
            _configuration = configuration;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var intervalo = _configuration.GetValue<int>("IntervaloSegundos");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var dispositivos = await _deviceService.ObterDispositivosAsync();

                    foreach (var dispositivo in dispositivos)
                    {
                        var online = await _pingService.VerificarAsync(dispositivo.Ip);
                        var status = online ? "ONLINE" : "OFFLINE";

                        var mensagem = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - {dispositivo.Nome} - {status}";

                        await _logService.GravarLogAsync(mensagem);
                        _logger.LogInformation(mensagem);
                    }
                }
                catch (Exception ex)
                {
                    await _logService.GravarLogAsync("ERRO: " + ex.Message);
                    _logger.LogError(ex, "Erro no monitoramento");
                }

                await Task.Delay(intervalo * 1000, stoppingToken);
            }
        }
    }
}
