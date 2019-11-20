using App3.View;
using MultiClientWindow.Viewmodel;
using System;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace App3
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {

            this.InitializeComponent();
            ApplicationView.GetForCurrentView().SetPreferredMinSize(new Size { Width = 1200, Height = 700 });
            ApplicationView.PreferredLaunchViewSize = new Size(1200, 700);
            ApplicationView.PreferredLaunchWindowingMode = ApplicationViewWindowingMode.PreferredLaunchViewSize;

        }

        public static CoreDispatcher dispatcher { get; private set; }

        private void Login(object sender, RoutedEventArgs e)
        {
            LoginStart();
        }

        private void Register(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(Register));
        }
        static public async Task OnUiThread(Action action)
        {
            await dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => action());
        }

        private void LoginEnter(object sender, Windows.UI.Xaml.Input.KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
            {  
                    LoginStart();
            }
        }

        void LoginStart()
        {
            Not_connected.Visibility = (Visibility)1;
            Connecting.Visibility = (Visibility)1;
            
            dispatcher = CoreWindow.GetForCurrentThread().Dispatcher;
            if (!NicknameBox.Text.Equals("") && !PasswordBox.Password.Equals(""))
            {
                new Login().Connect("127.0.0.1", NicknameBox.Text, PasswordBox.Password, this);
                NullLogin.Visibility = (Visibility)1;
            }
            else
            {
                NullLogin.Visibility = 0;
            }
        }
    }
}
