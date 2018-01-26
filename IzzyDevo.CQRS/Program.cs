using System;
using System.Threading.Tasks;
using IzzyDevo.CQRS.Infrastructure;

namespace IzzyDevo.CQRS
{
    public static class Program
    {
        // https://blogs.msdn.microsoft.com/mazhou/2017/05/30/c-7-series-part-2-async-main/
        public static void Main(string[] args) =>
            MainAsync(args).GetAwaiter().GetResult();

        public static async Task MainAsync(string[] args)
        {
            var mediator = new MediatorBootstrapper().BuildMediator();

            Console.WriteLine("You can browse Raven Studio now. When you're done, click any key to cotinue.");
            Console.ReadKey();

            try
            {
                await Runner.Run(mediator);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            Console.WriteLine("Click any key to cotinue.");
            Console.ReadKey();
        }
    }
}