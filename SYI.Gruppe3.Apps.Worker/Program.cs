namespace SYI.Gruppe3.Apps.Worker
{
    public class Program
    {
        public static void Main(string[] args)
        {
             

            var builder = WebApplication.CreateBuilder(args)
                .ConfigureServices(services =>
                {

                })
                .Build();

            host.Run();


            Console.WriteLine("ICH BIN ONLINE");
        }
    }
}