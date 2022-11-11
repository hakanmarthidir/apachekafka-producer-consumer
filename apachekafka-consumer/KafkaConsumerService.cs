using Confluent.Kafka;
using Microsoft.Extensions.Hosting;
using shared;
using System.Text.Json;

namespace apachekafka_consumer
{
    //services.AddSingleton<IHostedService, KafkaConsumerService>();

    public class KafkaConsumerService : IHostedService
    {
        private readonly string topicName = "test";
        private readonly ConsumerConfig _consumerConfig;
        public KafkaConsumerService()
        {
            _consumerConfig = new ConsumerConfig()
            {
                BootstrapServers = "localhost:9092", //"host1:9092,host2:9092",
                GroupId = "test-consumer-group",
                AutoOffsetReset = AutoOffsetReset.Earliest,
                EnableAutoCommit = true, //default
                EnableAutoOffsetStore = true
            };
        }


        public Task StartAsync(CancellationToken cancellationToken)
        {
            using (var consumerBuilder = new ConsumerBuilder<string, string>(_consumerConfig).Build())
            {
                consumerBuilder.Subscribe(topicName);
                var cancelToken = new CancellationTokenSource();
                try
                {
                    while (true)
                    {
                        var consumeResult = consumerBuilder.Consume(cancelToken.Token);
                        if (consumeResult != null)
                        {
                            var deserializedValue = JsonSerializer.Deserialize<UserActivity>(consumeResult.Message.Value);
                            var message = $"{consumeResult.Message.Key} - {deserializedValue.Id} {deserializedValue.EmailAddress}";
                            Console.WriteLine($"consumed this : {message}");
                        }
                    }
                }
                catch (Exception)
                {
                    consumerBuilder.Close();
                }
            }


            return Task.CompletedTask;

        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}