Monitor de Dispositivos - .NET Worker Service
Objetivo

Este projeto foi desenvolvido como um teste técnico com o objetivo de simular o monitoramento de dispositivos de rede (como câmeras IP e CLPs), realizando verificações periódicas de conectividade e registrando logs com o status dos dispositivos.

Tecnologias Utilizadas
.NET 8
Worker Service
Injeção de Dependência (Dependency Injection)
ILogger (Logging nativo do .NET)
System.Net.NetworkInformation (Ping)
xUnit (Testes Unitários)
JSON (Configuração e dados)

Funcionamento da Aplicação

O sistema funciona como um serviço em segundo plano (Worker Service):

Ao iniciar, o Worker carrega as configurações do appsettings.json
Lê a lista de dispositivos do arquivo dispositivos.json
A cada intervalo configurado:
Realiza ping nos dispositivos
Verifica se estão ONLINE ou OFFLINE
Captura o tempo de resposta
Registra o resultado em log
Em caso de erro:
O sistema não para
O erro é registrado em log

________________________

Exemplo de Log Gerado
2026-04-04 22:40:01 - Camera WiFi - ONLINE 
2026-04-04 22:40:11 - Camera WiFi - OFFLINE
2026-04-04 22:40:21 - Google DNS - ONLINE

OBS: foi utilizado alguns Ip's coletados no meu Roteador e outro de uso geral para exemplificar como o do Cloudflare e google DNS para ter validado alguns que usam e outro que estão Offline 

Os logs são armazenados em arquivos diários:

2026-04-04-log.txt
__________________________________________
Configuração
appsettings.json

Define o intervalo de monitoramento:

{
  "IntervaloSegundos": 10
}

dispositivos.json

Lista de dispositivos monitorados:

[
  {
    "Nome": "Camera WiFi",
    "Ip": "192.168.200.102"
  },
  {
    "Nome": "Google DNS",
    "Ip": "8.8.8.8"
  }
]

Como Executar o Projeto
Pré-requisitos
.NET 8 ou superior instalado
Visual Studio 2022

Executando via Visual Studio
Abrir a solução no Visual Studio
Definir o projeto como Startup
Pressionar F5 ou Ctrl + F5

Executando via CLI
dotnet build
dotnet run

Testes Unitários

O projeto contém testes unitários utilizando xUnit.

Para executar:

dotnet test

Testes implementados:

Leitura de dispositivos (DeviceService)
Verificação de conectividade (PingService)
Escrita de logs (LogService)

Autor

Desenvolvido por Douglas Santos Costa

__________________________________________________________________________________________________________________________________________

Documantação de como foi usado a O chat GPT para orientar no desenvolvimento :
preciso desenvolver uma aplicação em .net do tipo Windows Service ou Worker Service, onde deve rodar em segundo plano  com uma verificação periodica

Boa escolha — pra esse tipo de cenário hoje o mais recomendado é usar Worker Service em vez de Windows Service “raiz”. Ele já vem preparado pra rodar em background, com injeção de dependência, logging e agendamento simples.

Vou te explicar direto ao ponto e com exemplo prático 👇

✅ Diferença rápida
Windows Service (clássico) → mais antigo, mais complexo de manter
Worker Service (.NET moderno) → ✔ recomendado pela Microsoft

👉 O Worker Service pode rodar:

como console (pra debug)
como serviço do Windows
até em Docker
🚀 Criando o projeto

No terminal:

dotnet new worker -n MeuWorker
cd MeuWorker
📁 Estrutura básica

Você vai ter um arquivo principal tipo:

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;

    public Worker(ILogger<Worker> logger)
    {
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogInformation("Executando em: {time}", DateTimeOffset.Now);

            await Task.Delay(5000, stoppingToken); // roda a cada 5 segundos
        }
    }
}
⏱️ Fazendo verificação periódica (seu caso)

Exemplo mais realista:

protected override async Task ExecuteAsync(CancellationToken stoppingToken)
{
    while (!stoppingToken.IsCancellationRequested)
    {
        try
        {
            await VerificarAlgo();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro na execução");
        }

        await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
    }
}

private Task VerificarAlgo()
{
    _logger.LogInformation("Verificando processo...");

    // aqui você pode:
    // - consultar banco
    // - chamar API
    // - ler arquivos
    // - processar fila (RabbitMQ 👀)

    return Task.CompletedTask;
}
⚙️ Configurando como Windows Service

No Program.cs:

using Microsoft.Extensions.Hosting;

var builder = Host.CreateDefaultBuilder(args);

builder.UseWindowsService(); // 👈 ESSENCIAL

builder.ConfigureServices((context, services) =>
{
    services.AddHostedService<Worker>();
});

var host = builder.Build();
host.Run();
📦 Publicando
dotnet publish -c Release -r win-x64 --self-contained false
🧰 Instalando como serviço no Windows

No PowerShell (admin):

sc create MeuServico binPath= "C:\caminho\MeuWorker.exe"

Iniciar:

sc start MeuServico
🧠 Boas práticas (importante!)

✔ Use ILogger (nada de Console.WriteLine em produção)
✔ Trate exceções (senão o serviço pode parar)
✔ Evite loop sem delay (vai travar CPU)
✔ Use CancellationToken sempre
✔ Pode integrar com:

banco (EF Core)
filas (RabbitMQ — que você já quer aprender 👀)
APIs externas
💡 Dica avançada (nível sênior)

Se quiser algo mais robusto para agendamento:

usar Hangfire
ou cron jobs com bibliotecas tipo Cronos
🔥 Exemplo de cenário real

Você pode usar isso para:

verificar pedidos pendentes
processar fila de mensagens
enviar e-mails
monitorar serviços (tipo ping 👀 você já viu isso!)

Se quiser, posso montar contigo um projeto completo com:

✅ Worker + banco (EF Core)
✅ integração com RabbitMQ
✅ logs estruturados
✅ config via appsettings.json

Só me fala o que exatamente você quer que esse serviço faça 👍

vou desenvolver usando  o visual studio 2022

Perfeito, Douglas — usando o Visual Studio 2022 fica ainda mais fácil montar isso 👍
Vou te guiar passo a passo bem prático.

🚀 Criando o Worker Service no Visual Studio
1. Criar o projeto
Abra o Visual Studio
Clique em Create a new project
Procure por:

👉 Worker Service

Nome: MeuWorkerService
Framework: .NET 8 (ou o que estiver usando)
📁 Estrutura que o VS cria

Você já vai receber algo assim:

Program.cs
Worker.cs

E o Worker já herda de BackgroundService (ou seja: já está pronto pra rodar em background 😄)

⏱️ Ajustando para execução periódica

Abra o Worker.cs e deixe assim:

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;

    public Worker(ILogger<Worker> logger)
    {
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Serviço iniciado");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await Processar();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao executar processo");
            }

            await Task.Delay(TimeSpan.FromSeconds(30), stoppingToken);
        }
    }

    private Task Processar()
    {
        _logger.LogInformation("Executando verificação em: {time}", DateTime.Now);

        // Aqui você coloca sua lógica:
        // - consulta banco
        // - chama API
        // - processa fila
        // - etc

        return Task.CompletedTask;
    }
}
▶️ Testando no próprio Visual Studio

👉 Só apertar F5

Ele vai rodar como console, e você verá logs tipo:

Executando verificação em: 06/04/2026 21:XX
⚙️ Configurar como Windows Service

Abra o Program.cs e ajuste:

using Microsoft.Extensions.Hosting;

var builder = Host.CreateDefaultBuilder(args);

builder.UseWindowsService(); // 👈 importante

builder.ConfigureServices((context, services) =>
{
    services.AddHostedService<Worker>();
});

var host = builder.Build();
host.Run();
📦 Publicar pelo Visual Studio
Clique com botão direito no projeto
Publish
Escolha:
Folder
Release
win-x64
🧰 Instalar como serviço

Depois de publicado:

Abra o PowerShell como administrador:

sc create MeuWorkerService binPath= "C:\caminho\publicado\MeuWorkerService.exe"

Iniciar:

sc start MeuWorkerService
🧠 Dicas importantes (baseadas em experiência real)

👉 Evite isso:

while(true)

👉 Sempre use:

CancellationToken

👉 Controle o tempo com:

Task.Delay(...)

👉 Log é essencial (depois você pode evoluir pra Serilog)

🔥 Próximo nível (vale muito pra você)

Como você já está estudando:

.NET moderno
RabbitMQ
APIs

👉 Esse Worker pode virar:

consumidor de fila (mensageria)
processador de pedidos
integração entre sistemas

Se quiser, no próximo passo eu posso te ajudar a transformar esse Worker em algo mais profissional, tipo:

💡 Worker + banco + EF Core
💡 Worker consumindo RabbitMQ
💡 Worker com configuração via appsettings.json
💡 Worker com retry automático

Só me fala qual cenário você quer implementar 🚀

esse configuração de ips e dispositivos deve está em um arquivo json

Perfeito — isso é exatamente o jeito correto e profissional de fazer 👍
No Worker Service, o padrão é usar o appsettings.json para esse tipo de configuração.

Vou te mostrar como montar isso do jeito certo 👇

📄 1. Criando o JSON de configuração

No seu projeto, edite (ou crie) o appsettings.json:

{
  "Dispositivos": [
    {
      "Nome": "Servidor 1",
      "Ip": "192.168.0.10",
      "Porta": 80
    },
    {
      "Nome": "Roteador",
      "Ip": "192.168.0.1",
      "Porta": 80
    }
  ],
  "IntervaloSegundos": 30
}
📦 2. Criar classe para mapear o JSON

Crie uma classe:

public class Dispositivo
{
    public string Nome { get; set; }
    public string Ip { get; set; }
    public int Porta { get; set; }
}

public class Configuracao
{
    public List<Dispositivo> Dispositivos { get; set; }
    public int IntervaloSegundos { get; set; }
}
⚙️ 3. Configurar leitura no Program.cs

No Program.cs:

builder.ConfigureServices((context, services) =>
{
    services.Configure<Configuracao>(
        context.Configuration
    );

    services.AddHostedService<Worker>();
});
🧠 4. Injetar no Worker

Agora no Worker.cs:

using Microsoft.Extensions.Options;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly Configuracao _config;

    public Worker(ILogger<Worker> logger, IOptions<Configuracao> config)
    {
        _logger = logger;
        _config = config.Value;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            foreach (var dispositivo in _config.Dispositivos)
            {
                VerificarDispositivo(dispositivo);
            }

            await Task.Delay(
                TimeSpan.FromSeconds(_config.IntervaloSegundos),
                stoppingToken
            );
        }
    }

    private void VerificarDispositivo(Dispositivo dispositivo)
    {
        _logger.LogInformation(
            "Verificando {nome} ({ip}:{porta})",
            dispositivo.Nome,
            dispositivo.Ip,
            dispositivo.Porta
        );

        // aqui você pode usar Ping, HttpClient, Socket, etc
    }
}
🔄 5. Atualização automática do JSON (opcional avançado)

Se quiser que o serviço recarregue o JSON sem reiniciar:

No Program.cs:

builder.ConfigureAppConfiguration((hostingContext, config) =>
{
    config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
});

👉 E trocar IOptions por:

IOptionsMonitor<Configuracao>
💡 Resultado final

Você terá:

✅ Lista de dispositivos configurável
✅ Intervalo ajustável sem recompilar
✅ Código limpo e desacoplado
✅ Padrão usado em produção real

🔥 Próximo passo (recomendado pra você)

Como você mencionou antes sobre Ping 👀

👉 Dá pra evoluir isso pra:

testar conectividade com Ping
registrar status (UP/DOWN)
salvar histórico em banco
disparar alertas

Se quiser, eu posso montar contigo agora:

👉 verificação real com Ping
👉 timeout + retry
👉 log de falha detalhado
👉 salvar resultado no banco

Só falar: "quero monitoramento real" 🚀

por favor trazer  essa consulta de Ips usando o ping

Perfeito — vamos transformar seu Worker em um monitor real de dispositivos usando Ping 👍
Você já até viu a classe antes (Ping), agora vamos aplicar corretamente.

🌐 O que vamos fazer

Para cada IP do appsettings.json:

enviar ping
verificar se respondeu
medir tempo de resposta
logar status (ONLINE / OFFLINE)

Usando a classe do .NET: System.Net.NetworkInformation.Ping

📦 Código completo do Ping

Atualize seu Worker.cs:

using System.Net.NetworkInformation;
using Microsoft.Extensions.Options;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly Configuracao _config;

    public Worker(ILogger<Worker> logger, IOptions<Configuracao> config)
    {
        _logger = logger;
        _config = config.Value;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Monitor de dispositivos iniciado");

        while (!stoppingToken.IsCancellationRequested)
        {
            foreach (var dispositivo in _config.Dispositivos)
            {
                await VerificarDispositivo(dispositivo);
            }

            await Task.Delay(
                TimeSpan.FromSeconds(_config.IntervaloSegundos),
                stoppingToken
            );
        }
    }

    private async Task VerificarDispositivo(Dispositivo dispositivo)
    {
        try
        {
            using var ping = new Ping();

            var resposta = await ping.SendPingAsync(
                dispositivo.Ip,
                3000 // timeout em ms
            );

            if (resposta.Status == IPStatus.Success)
            {
                _logger.LogInformation(
                    "🟢 {nome} ({ip}) ONLINE - Tempo: {tempo}ms",
                    dispositivo.Nome,
                    dispositivo.Ip,
                    resposta.RoundtripTime
                );
            }
            else
            {
                _logger.LogWarning(
                    "🔴 {nome} ({ip}) OFFLINE - Status: {status}",
                    dispositivo.Nome,
                    dispositivo.Ip,
                    resposta.Status
                );
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Erro ao verificar {nome} ({ip})",
                dispositivo.Nome,
                dispositivo.Ip
            );
        }
    }
}
📊 Possíveis retornos do Ping

O IPStatus pode retornar:

Success → respondeu 👍
TimedOut → não respondeu ⏱️
DestinationHostUnreachable → host inacessível 🚫
🧠 Melhorias importantes (nível profissional)
✅ 1. Evitar falso negativo (retry)
for (int i = 0; i < 3; i++)
{
    var resposta = await ping.SendPingAsync(dispositivo.Ip, 3000);

    if (resposta.Status == IPStatus.Success)
        return;
}
✅ 2. Paralelizar (muito importante)

Se tiver muitos dispositivos:

var tarefas = _config.Dispositivos
    .Select(d => VerificarDispositivo(d));

await Task.WhenAll(tarefas);
✅ 3. Salvar histórico (futuro)

Você pode evoluir para:

salvar no banco (SQL Server/PostgreSQL)
criar dashboard
alertar quando cair
🔥 Exemplo de saída
🟢 Servidor 1 (192.168.0.10) ONLINE - Tempo: 12ms
🔴 Roteador (192.168.0.1) OFFLINE - Status: TimedOut
⚠️ Atenção (importante mesmo)

Ping pode falhar mesmo com o dispositivo online:

firewall bloqueando ICMP
roteador não responde ping
cloud (AWS/Azure) geralmente bloqueia

👉 nesses casos o ideal é:

testar porta (TCP)
ou usar HTTP
🚀 Próximo nível pra você

Se quiser deixar isso MUITO mais forte:

✔ testar porta (tipo 80, 443)
✔ integrar com RabbitMQ (fila de eventos)
✔ alertas (email / webhook)
✔ dashboard web

Se quiser, no próximo passo eu posso te ajudar a evoluir isso para:

👉 monitoramento estilo Zabbix simplificado
👉 salvar logs no banco
👉 API pra visualizar status

Só me fala: "quero nível profissional" 😄
