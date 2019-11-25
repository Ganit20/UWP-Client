using App3;
using App3.View;
using App3.ViewModel;
using MultiClientClient.Viewmodel;
using MultiClientWindow.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.Networking;
using Windows.Networking.Connectivity;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace MultiClientWindow.Viewmodel
{
    public class Login
    {
        public static NetworkStream stream;
        public CoreDispatcher p;
        public static MainPage form;
        public static TcpClient c;
        public static string nick;

        public async Task Connect(string IP, string Nickname, string Password, MainPage main)
           
        {
            nick = Nickname;
            const int port = 8000;
            string ip = IP;
            form = main;
            try
            {
                c = new TcpClient();
                form.Connecting.Visibility = 0;
                await c.ConnectAsync(ip, port);

                int i = 0;

                if (c.Connected)
                {
                    stream = c.GetStream();
                    form.Connecting.Text = "Connected....";
                    LoginMe(Nickname, Password);

                }
                else if (!c.Connected && i <= 2)
                {
                    form.Connecting.Visibility = (Visibility)1;
                    form.Not_connected.Visibility = 0;
                }

            }
            catch (TypeInitializationException)
            {
                form.Connecting.Visibility = (Visibility)1;
                form.Not_connected.Visibility = 0;

            }
            catch (SocketException)
            {
                form.Connecting.Visibility = (Visibility)1;
                form.Not_connected.Visibility = 0;

            }
        }
        void LoginMe(string Nickname, string Password)
        {          
           NetworkStream st = stream;
            string From = Nickname;
            string IP=  null ;
            var info = new user() { Name = From, password =Encoding.UTF8.GetString(new Encryption().Encrypt(Password)) };
            string U_info = JsonConvert.SerializeObject(info);
            var U_binfo = new Encryption().Encrypt("LOG?" + U_info);
            byte[] b = new TextOperations().addByte(Encoding.UTF8.GetBytes(new TextOperations().intLength(U_binfo.Length)) , U_binfo);
            var form1 = form;
           
            st.Write(b, 0, b.Length);
            Task.Factory.StartNew(async () =>
            {

                String response = String.Empty;
                Byte[] ByteLength = new byte[4];
                Array.Clear(ByteLength, 0, ByteLength.Length);
                stream.ReadTimeout = 10000;
                try
                {
                    Int32 bytes = stream.Read(ByteLength, 0, 4);
                }catch(IOException e) {
                    form.Connecting.Text = e.ToString();
                    form.Connecting.Visibility = 0;
                }

                    String v = System.Text.Encoding.UTF8.GetString(ByteLength, 0, 4);
                    String d = v.Substring(0, v.IndexOf('?', 0, 4));
                    int bl = int.Parse(d);
                    Byte[] data = new Byte[bl];
                    Int32 msg = stream.Read(data, 0, bl);
                    string strmsg = System.Text.Encoding.UTF8.GetString(data);
                    string vstrmsg = strmsg.Substring(0, strmsg.IndexOf('?'));
                    switch (vstrmsg)
                    {
                        case "LOG":
                            strmsg = strmsg.Substring(strmsg.IndexOf('?') + 1, strmsg.IndexOf("?", strmsg.IndexOf('?') + 1) - 4);
                            if (strmsg.Equals("TRUE"))
                                await MainPage.OnUiThread(() =>
                                 {

                                     form.Frame.Navigate(typeof(Chat));
                                 }); else {
                                await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                                 {
                                     form.Not_connected.Text = ("Wrong password or login");
                                     form.Not_connected.Visibility = 0;
                                     form.Connecting.Visibility = (Visibility)1;
                                 });
                            }



                            break;
                        case "BAN":
                            await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                            {
                                form.Not_connected.Text = "You have been banned from the server";
                                form.Not_connected.Visibility = 0;
                                form.Connecting.Visibility = (Visibility)1;

                            }); break;
                    }
                

            });
        }

    }
} 
                
            
        
    


