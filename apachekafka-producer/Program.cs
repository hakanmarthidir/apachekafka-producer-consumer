namespace apachekafka_producer
{
    public static class Program
    {

        static async Task Main(string[] args)
        {

            var producerService = new KafkaProducerService();
            await producerService.StartAsync(default).ConfigureAwait(false);

            Console.ReadLine();
        }
    }
}