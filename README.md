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

Exemplo de Log Gerado
2026-04-04 22:40:01 - Camera WiFi - ONLINE - 3ms
2026-04-04 22:40:11 - Camera WiFi - OFFLINE
2026-04-04 22:40:21 - Google DNS - ONLINE - 18ms

Os logs são armazenados em arquivos diários:

logs/log-2026-04-04.txt

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