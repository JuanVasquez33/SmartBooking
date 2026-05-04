using Confluent.Kafka;
using Microsoft.EntityFrameworkCore;
using SmartBooking.Core.Entities;
using SmartBooking.Infrastructure.Persistence;
using System.Text.Json;
namespace SmartBooking.Application.Services
{

    public class KafkaConsumerWorker : BackgroundService
    {
        private const string TOPIC = "smartbooking-logs";
        private readonly IServiceScopeFactory _scopeFactory;

        public KafkaConsumerWorker(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken ct)
        {
            var config = new ConsumerConfig
            {
                BootstrapServers = "localhost:9092",
                GroupId = "smartbooking-group",
                AutoOffsetReset = AutoOffsetReset.Earliest
            };

            using var consumer = new ConsumerBuilder<string, string>(config).Build();
            consumer.Subscribe(TOPIC);

            while (!ct.IsCancellationRequested)
            {
                var result = consumer.Consume(TimeSpan.FromSeconds(1));
                if (result == null) continue;

                using var scope = _scopeFactory.CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                var doc = JsonDocument.Parse(result.Message.Value).RootElement;
                db.AppLogs.Add(new AppLog
                {
                    Nivel = doc.GetProperty("Nivel").GetString()!,
                    Proceso = doc.GetProperty("Proceso").GetString()!,
                    Mensaje = doc.GetProperty("Mensaje").GetString()!,
                    UsuarioId = doc.GetProperty("UsuarioId").ValueKind != JsonValueKind.Null
                                 ? doc.GetProperty("UsuarioId").GetInt32() : null,
                    CreadoEn = DateTime.UtcNow
                });

                await db.SaveChangesAsync(ct);
            }
        }
    }
}
