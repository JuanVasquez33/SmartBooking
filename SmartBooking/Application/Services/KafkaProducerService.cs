using Confluent.Kafka;
using System.Text.Json;
namespace SmartBooking.Application.Services
{

    public class KafkaProducerService
    {
        private readonly IProducer<string, string> _producer;
        private const string TOPIC = "smartbooking-logs";

        public KafkaProducerService()
        {
            var config = new ProducerConfig { BootstrapServers = "localhost:9092" };
            _producer = new ProducerBuilder<string, string>(config).Build();
        }

        public async Task PublicarAsync(string nivel, string proceso, string mensaje, int? usuarioId = null)
        {
            var payload = JsonSerializer.Serialize(new
            {
                Nivel = nivel,
                Proceso = proceso,
                Mensaje = mensaje,
                UsuarioId = usuarioId,
                CreadoEn = DateTime.UtcNow
            });

            await _producer.ProduceAsync(TOPIC,
                new Message<string, string> { Key = proceso, Value = payload });
        }
    }
}
