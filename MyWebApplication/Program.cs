using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;

namespace MyWebApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            var host = new WebHostBuilder()
                        .UseKestrel()
                        .UseStartup<Startup>()
                        .Build();

            host.Run();
        }
    }
}
