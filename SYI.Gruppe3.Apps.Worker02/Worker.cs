using Azure.Messaging.ServiceBus;
using Newtonsoft.Json;
using SYI.Gruppe3.Apps.Worker02.Models;
using SYI.Gruppe3.Apps.Worker02.Processors;

namespace SYI.Gruppe3.Apps.Worker02
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private ServiceBusClient _client;
        private ServiceBusProcessor _processor;
        private string NAMESPACE_CONNECTION_STRING = Environment.GetEnvironmentVariable("NAMESPACE_CONNECTION_STRING");
        private string QUEUE_NAME = Environment.GetEnvironmentVariable("QUEUE_NAME");

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
            _logger.LogInformation("WORKER IST ONLINE! :-)");

        }
        private async Task HandleMessage(ProcessMessageEventArgs args)
        {
            string body = args.Message.Body.ToString();
            _logger.LogInformation($"Received: {body}");
            var jsonConverted = JsonConvert.DeserializeObject<QueueInputModel>(body);
            //delete if could be converted
            await args.CompleteMessageAsync(args.Message);

            _logger.LogInformation("processing file...");
            //process file
            var processor = new DataProcessor(jsonConverted.Container, jsonConverted.FileName);
            await processor.ProcessBlobStorageUrl(_logger);
        }

        Task ErrorHandler(ProcessErrorEventArgs args)
        {
            _logger.LogError($"Error: {args.Exception.ToString()}");
            return Task.CompletedTask;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                var clientOptions = new ServiceBusClientOptions()
                {
                    TransportType = ServiceBusTransportType.AmqpWebSockets
                };
                _client = new ServiceBusClient(NAMESPACE_CONNECTION_STRING, clientOptions);
                _processor = _client.CreateProcessor(QUEUE_NAME, new ServiceBusProcessorOptions());
                _processor.ProcessMessageAsync += HandleMessage;
                _processor.ProcessErrorAsync += ErrorHandler;
                await _processor.StartProcessingAsync();
                _logger.LogInformation("Queue subscription has been launched");
            }
            catch (Exception e)
            {
                _logger.LogError(e, "fehler bei der initialisierung des workers..");
                return;
            }
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Waiting for queue item...");
                await Task.Delay(10000, stoppingToken);
            }
        }
    }
}