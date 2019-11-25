using App3.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace App3.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Register : Page
    {
        public Register()
        {
            this.InitializeComponent();
        }

        private async void RegisterAsync(object sender, RoutedEventArgs e)
        {
            if(NicknameBox.Text!="" && PasswordBox.Password!="" && Email.Text!="")
            await new  RegisterC().registerMeAsync(NicknameBox.Text,PasswordBox.Password, Email.Text, "51.137.130.21", this);
        }

        private void LoginEnter(object sender, KeyRoutedEventArgs e)
        {
            
                object m = new object();
            RoutedEventArgs c = new RoutedEventArgs();
            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                RegisterAsync(m,c);
            }
        }
        private void Back(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MainPage));
        }
    }
}
