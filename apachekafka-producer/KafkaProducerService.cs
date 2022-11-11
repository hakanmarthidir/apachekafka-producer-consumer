using Confluent.Kafka;
using System.Text.Json;
using shared;
using Microsoft.Extensions.Hosting;

namespace apachekafka_producer
{
    //services.AddSingleton<IHostedService, KafkaProducerService>();
    public class KafkaProducerService : IHostedService
    {
        private readonly string topicName = "test";
        private readonly ProducerConfig _producerConfig;

        public KafkaProducerService()
        {
            _producerConfig = new ProducerConfig() { BootstrapServers = "localhost:9092" };
        }
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using (var producer = new ProducerBuilder<string, string>(_producerConfig).Build())
            {
                try
                {
                    for (int i = 0; i < 1000; i++)
                    {
                        var messageKey = Guid.NewGuid().ToString();
                        string activityMessage = JsonSerializer.Serialize(new UserActivity() { EmailAddress = "test@test.com", ActivityType = 1 });

                        var deliverResult = await producer.ProduceAsync(topicName, new Message<string, string>() { Value = activityMessage, Key = messageKey }, cancellationToken)
                            .ConfigureAwait(false);

                        if (deliverResult.Status == PersistenceStatus.Persisted)
                        {
                            Console.WriteLine($"Message Persisted.. {deliverResult.TopicPartitionOffset} {deliverResult.TopicPartition} {deliverResult.Partition.Value} ");
                        }                        
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}