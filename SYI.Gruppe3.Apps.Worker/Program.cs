namespace SYI.Gruppe3.Apps.Worker
{
    public class Program
    {
        public static void Main(string[] args)
        {
            IHost host = Host.CreateDefaultBuilder(args)
                .ConfigureServices(services =>
                {
                    services.AddLogging((opt) =>
                    {
                        opt.ClearProviders();
                        opt.AddConsole();
                    });
                    services.AddHostedService<Worker>();
                })
                .Build();

            host.Run();


            Console.WriteLine("ICH BIN ONLINE");
        }
    }
}