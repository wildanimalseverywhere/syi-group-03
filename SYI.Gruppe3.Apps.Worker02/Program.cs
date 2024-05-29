namespace SYI.Gruppe3.Apps.Worker02
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddLogging((opt) =>
            {
                opt.ClearProviders();
                opt.AddConsole();
            });
            builder.Services.AddHostedService<Worker>();

            var app = builder.Build();


           

            app.MapGet("/", (HttpContext httpContext) =>
            {
                return Task.FromResult(new
                {
                    alive = true, 
                    message = "hallo - ich bin die worker app und bin online"
                });
            });

            app.Run();
        }
    }
}
