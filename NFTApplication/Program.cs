using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQ.Consumer
{
    static class Program
    {
        static void Main(string[] args)
        {
          //  CreateHostBuilder(args).Build().Run();
            // EF Core uses this method at design time to access the DbContext


          //  int Num = 10000;
            var factory = new ConnectionFactory
            {
                Uri = new Uri("amqp://guest:guest@localhost:5672")
            };

            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();

            Consume(channel);

         
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddRazorPages();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
            }
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapRazorPages();

            app.Run();


        }
        //public static IHostBuilder CreateHostBuilder(string[] args)
        //      => Host.CreateDefaultBuilder(args)
        //          .ConfigureDefaults(
        //              webBuilder => webBuilder.UseStartup<Startup>());





        public record Data(int ID, string Name, string Message);

        //DirectExchangeConsumer
        public static void Consume(IModel channel)
        {
            channel.ExchangeDeclare("demo-direct-exchange", ExchangeType.Direct);
            channel.QueueDeclare("demo-direct-queue",
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null);
            channel.QueueBind("demo-direct-queue", "demo-direct-exchange", "account.init");

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (sender, args) =>
            {
                var body = args.Body.ToArray();
                var message = JsonConvert.DeserializeObject<Data>(Encoding.UTF8.GetString(body));


                // Call 
                string source = @"E:\Template\";
                string target = @"E:\Project_" + message.ID;

                // Template Copy
                Copy(source, target);
                // worker.js Replace Project ID 


                var P = Process.Start(target + "\\Install.bat");
                P.WaitForExit();

                // Check fro Resutlfile 
                var _target = target.Substring(3);
                var T = File.ReadAllText(target + "\\worker.js");

                File.WriteAllText(target + "\\worker.js", T.Replace("{Project_ID}", _target));


                // Install Command 

                var proc = Process.Start(target + "\\Install.bat");
                proc.WaitForExit();
                Console.WriteLine("Bat file installed");


                // Test Testfile 

                var Test = File.ReadAllText(target + "\\testresult.txt");
                if (Test.Contains("Completed"))
                {
                    //  var teststart = Process.Start("cmd.exe", "node test.js");
                    // teststart.WaitForExit();
                    Console.WriteLine("Completed");
                }
                // start worker 
                var start = Process.Start("node", "worker.js");

            };

            channel.BasicConsume("demo-direct-queue", true, consumer);
            Console.WriteLine("Consumer Started");
            Console.ReadLine();

        }

        public static void Copy(string sourceDirectory, string targetDirectory)
        {
            var diSource = new DirectoryInfo(sourceDirectory);
            var diTarget = new DirectoryInfo(targetDirectory);

            CopyAll(diSource, diTarget);
        }

        public static void CopyAll(DirectoryInfo source, DirectoryInfo target)
        {
            Directory.CreateDirectory(target.FullName);

            // Copy each file into the new directory.
            foreach (FileInfo fi in source.GetFiles())
            {
                Console.WriteLine(@"Copying {0}\{1}", target.FullName, fi.Name);
                fi.CopyTo(Path.Combine(target.FullName, fi.Name), true);
            }

            // Copy each subdirectory using recursion.
            foreach (DirectoryInfo diSourceSubDir in source.GetDirectories())
            {
                DirectoryInfo nextTargetSubDir =
                    target.CreateSubdirectory(diSourceSubDir.Name);
                CopyAll(diSourceSubDir, nextTargetSubDir);
            }
        }

        //QueueConsumer
        public static void _Consume(IModel channel)
        {
            channel.QueueDeclare("demo-queue",
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null);
            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (sender, args) =>
            {
                var body = args.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                Console.WriteLine(message);
            };

            channel.BasicConsume("demo-queue", true, consumer);
            Console.WriteLine("Consumer Started");
            Console.ReadLine();

        }




    }
}

