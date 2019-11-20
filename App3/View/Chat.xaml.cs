using MultiClientClient.Model;
using MultiClientClient.Viewmodel;
using MultiClientWindow.Viewmodel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
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
    public sealed partial class Chat : Page
    {
        public static CoreDispatcher dispatcher { get; private set; }
        public Chat()
        {
            this.InitializeComponent();
            var st = Login.stream;
            
        dispatcher = CoreWindow.GetForCurrentThread().Dispatcher;
            Task.Factory.StartNew(() =>
           {
               var rec = new Receiving();
               rec.Reading(this,st);
           });
        }
        static public async Task OnUiThread(Action action)
        {
            await dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => action());
        }

        private void Messages_Loaded(object sender, RoutedEventArgs e)
        {
            var listView = (ListView)sender;
            this.Messages.ItemsSource = MSG_Collection.dataList;
             

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Send();
        }
        private void Send()
        {
            new Sending().Send(For_Send.Text);
            For_Send.Text = "";
        }

        private void SendEnter(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                new Sending().Send(For_Send.Text);
                For_Send.Text = "";
            }
        }

        private void RoomList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            try
            {
                Receiving.json.TryGetValue(RoomList.SelectedItem.ToString(), out bool pw);
                if (pw) {
                    fly.ShowAt((FrameworkElement)sender);
                } else {


                    var a = RoomList.SelectedItem.ToString();
                    if (a != "" || a != "Chat Rooms: ")
                    {
                        new Sending().ChangeRoom(a, "");

                    }
                }
                
            }
            catch (NullReferenceException)
            { }
        }
        private void RPIT(object sender, RoutedEventArgs e)
        {
            if (!RoomPassword.IsEnabled)
                RoomPassword.IsEnabled = true;
            else
                RoomPassword.IsEnabled = false;

        }

        private void Create_Room(object sender, RoutedEventArgs e)
        {
           new Sending().CreateRoom(Room.Text, Login.nick, isPassword.IsChecked.Value, RoomPassword.Text);
            Room.Text = "";
            RoomPassword.Text = "";
            isPassword.IsChecked = false;
        }

        private void join(object sender, RoutedEventArgs e)
        {
            var a = RoomList.SelectedItem.ToString();
            new Sending().ChangeRoom(a, RPW.Text);
            RPW.Text = "";
        }

        private async void logout(object sender, RoutedEventArgs e)
        {
           await new Logout().ShowAsync();
        }
    }
}
