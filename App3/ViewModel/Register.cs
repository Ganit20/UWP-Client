using App3.View;
using MultiClientClient.Viewmodel;
using MultiClientWindow.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Core;
using Windows.UI.Xaml;

namespace App3.ViewModel
{
    class RegisterC
    {
        public async Task registerMeAsync(string Nick, string Password, string email, string IP, Register reg)
        {

            const int port = 8000;
            string ip = IP;
            var c = new TcpClient();
            await c.ConnectAsync(ip, port);
            if (c.Connected)
            {
                var st = c.GetStream();
                var msg = JsonConvert.SerializeObject(new user() { Name = Nick, email = email, password =Encoding.UTF8.GetString(new Encryption().Encrypt(Password)) });
                msg = "REG?" + msg;
                var emsg = new Encryption().Encrypt(msg);
                byte[] g = System.Text.Encoding.UTF8.GetBytes(new TextOperations().intLength(emsg.Length));
                var dcd = new TextOperations().addByte(g, emsg);
                st.Write(dcd, 0, dcd.Length);
                try
                {
                    string response = string.Empty;
                    byte[] ByteLength = new byte[4];
                    Array.Clear(ByteLength, 0, ByteLength.Length);
                    st.ReadTimeout = 10000;
                    Int32 bytes = st.Read(ByteLength, 0, 4);

                 
                        try
                        {
                            string v = System.Text.Encoding.UTF8.GetString(ByteLength, 0, 4);
                            string d = v.Substring(0, v.IndexOf('?', 0, 4));
                            int bl = int.Parse(d);
                            byte[] data = new byte[bl];
                            Int32 mesg = st.Read(data, 0, bl);
                            string strmsg = new Encryption().Decrypt(data);
                            string vstrmsg = strmsg.Substring(0, strmsg.IndexOf('?'));
                        if (vstrmsg.Equals("RDC"))
                        {
                            strmsg = strmsg.Substring(strmsg.IndexOf('?') + 1);
                            if (strmsg.Equals("Confirmed"))
                            {
                                await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                                 {
                                     reg.Wrong.Visibility = (Visibility)1;
                                     reg.ok.Visibility = 0;
                                     st.Dispose();
                                     c.Dispose();
                                 });
                        }
                        else if (strmsg.Equals("Nope"))
                        {
                                await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                                {
                                    reg.ok.Visibility = (Visibility)1;
                                    reg.Wrong.Visibility = 0;
                                });
                                
                                    st.Dispose();
                                    c.Dispose();
                                }
                            }
                        } catch (TypeInitializationException)
                        {

                            reg.con.Visibility = 0;

                        } 
                    }
                
                catch (SocketException)
                {           
                    reg.con.Visibility = 0;
                }
            } } } }
