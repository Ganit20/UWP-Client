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
                var msg = JsonConvert.SerializeObject(new user() { Name = Nick, email = email, password = Password });
                msg = "REG?" + msg + "?END";
                byte[] g = System.Text.Encoding.ASCII.GetBytes(msg);
                st.Write(g, 0, g.Length);

                try
                {
                    String response = String.Empty;
                    Byte[] ByteLength = new byte[4];
                    Array.Clear(ByteLength, 0, ByteLength.Length);
                    Int32 bytes = st.Read(ByteLength, 0, 4);

                    if (st.DataAvailable)
                    {
                        try
                        {
                            String v = System.Text.Encoding.ASCII.GetString(ByteLength, 0, 4);
                            String d = v.Substring(0, v.IndexOf('?', 0, 4));
                            int bl = int.Parse(d);
                            Byte[] data = new Byte[bl];
                            Int32 mesg = st.Read(data, 0, bl);
                            string strmsg = System.Text.Encoding.ASCII.GetString(data);
                            string vstrmsg = strmsg.Substring(0, strmsg.IndexOf('?'));
                            if (vstrmsg.Equals("RDC"))
                            {
                                strmsg = strmsg.Substring(strmsg.IndexOf('?') + 1, strmsg.IndexOf("?", strmsg.IndexOf('?') + 1) - 4);
                                if (strmsg.Equals("Confirmed"))
                                {
                                    reg.Wrong.Visibility = (Visibility)1;
                                    reg.ok.Visibility = 0;
                                    st.Dispose();
                                    c.Dispose();
                                    
                                }
                                else if (strmsg.Equals("Nope"))
                                {
                                    reg.ok.Visibility = (Visibility)1;
                                    reg.Wrong.Visibility = 0;
                                    st.Dispose();
                                    c.Dispose();
                                }
                            }
                        } catch (TypeInitializationException)
                        {

                            reg.con.Visibility = 0;

                        } }
                    else
                    {
                        reg.con.Visibility = (Visibility)1;
                    }
                    }
                
                catch (SocketException)
                {           
                    reg.con.Visibility = 0;
                }
            } } } }
