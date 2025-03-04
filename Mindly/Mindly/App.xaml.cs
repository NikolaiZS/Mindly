using Supabase.Interfaces;
using System.Windows;

namespace Mindly
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private static SupabaseClient? _supabaseService;

        protected override async void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            await InitializeSupabase();
        }

        private async Task InitializeSupabase()
        {
            _supabaseService = new SupabaseClient(); // Создаём экземпляр
            await _supabaseService.InitializeAsync(); // Инициализируем клиент
        }

        public static SupabaseClient SupabaseService => _supabaseService;
    }
}