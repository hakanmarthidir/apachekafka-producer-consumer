namespace apachekafka_consumer
{
    public static class Program
    {

        static async Task Main(string[] args)
        {

            var consumerService = new KafkaConsumerService();
            await consumerService.StartAsync(default);

            Console.ReadLine();
        }
    }
}