using Microsoft.Extensions.DependencyInjection;
using PressureTest.Services;
using PressureTest.Services.Interfaces;

namespace PressureTest
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            QuestPDF.Settings.License = QuestPDF.Infrastructure.LicenseType.Community;

            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var services = new ServiceCollection();
            ConfigureServices(services);

            using var provider = services.BuildServiceProvider();
            var mainForm = provider.GetRequiredService<Form1>();

            Application.Run(mainForm);
        }

        private static void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IModbusService, ModbusService>();
            services.AddSingleton<IPLCReadWorker, PLCReadWorker>();
            services.AddTransient<Form1>();
        }
    }
}