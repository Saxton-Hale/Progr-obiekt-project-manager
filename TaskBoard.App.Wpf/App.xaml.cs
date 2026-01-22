using System.Configuration;
using System.Data;
using System.IO;
using System.Windows;
using TaskBoard.App.Wpf.ViewModels;
using TaskBoard.App.Wpf.Views;
using TaskBoard.Domain.Interfaces;
using TaskBoard.Infrastructure.FileStorage;
using TaskBoard.Infrastructure.Repositories;

namespace TaskBoard.App.Wpf
{
    public partial class App : Application
    {
        public static IUnitOfWork UnitOfWork { get; private set; } = null!;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            var dir = Path.Combine(appData, "TaskBoard");
            var path = Path.Combine(dir, "boards.json");

            var dataStore = new JsonDataStore();
            UnitOfWork = new UnitOfWork(dataStore, path);

            var window = new MainWindow();
            MainWindow = window;
            window.Show();
        }
    }

}
