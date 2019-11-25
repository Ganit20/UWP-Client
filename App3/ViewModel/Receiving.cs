using MultiClientClient.Model;
using MultiClientWindow;
using MultiServe.Net.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using MultiClientWindow.Viewmodel;
using System.Threading;
using Windows.UI.Xaml;
using App3.View;
using App3;

namespace MultiClientClient.Viewmodel
{
    public class Receiving
    {
        public static Dictionary<string, bool> json;
        Chat view;
        public async void ReadingAsync(Chat obj, NetworkStream st)
        {
            view = obj;
            NetworkStream stream = st;
            while (true)
            {
                try
                {
                    string response = string.Empty;
                    byte[] ByteLength = new byte[4];
                    Array.Clear(ByteLength, 0, ByteLength.Length);
                    Int32 bytes = stream.Read(ByteLength, 0, 4);

                    if (stream.DataAvailable)
                    {
                        try
                        {
                            
                            string strmsg = System.Text.Encoding.UTF8.GetString(ByteLength);
                            string d = strmsg.Substring(0, strmsg.IndexOf('?'));
                            int bl = int.Parse(d);
                            byte[] a = new byte[bl];
                            stream.Read(a, 0, a.Length);
                            var mesage = new Encryption().Decrypt(a);
                            strmsg = mesage.Substring(0,mesage.IndexOf('?', 0, 4));
                            Task.Factory.StartNew(() =>
                            {
                                switch (strmsg)
                                {

                                    case "BAN":
                                         new Messages().MessageAsync(view, mesage); break;
                                    case "USE":
                                        new Messages().UserList(strmsg, view, mesage);
                                        break;
                                    case "RCC":
                                        new Messages().RoomList(strmsg, view, mesage);
                                        break;
                                    case "MSG":
                                         new Messages().MessageAsync(view, mesage);
                                        break;
                                    case "SSG":
                                        new Messages().ChangeRoom(mesage, view, a);
                                        break;

                                }
                            });
                        }
                        catch (System.ArgumentOutOfRangeException) { }
                    }
                }
                catch (IOException)
                {
                    Console.Write("Connection Error");
                }
            }
        }
        //void Register(string strmsg, Dispatcher dispatcher, Register r)
        //{
        //    strmsg = strmsg.Substring(strmsg.IndexOf('?') + 1, strmsg.IndexOf("?", strmsg.IndexOf('?') + 1) - 4);
        //    if (strmsg.Equals("Confirmed"))
        //    {
        //        dispatcher.Invoke(() =>
        //        {
        //            r.label4.Text = "You are registered";
                    
        //        });
        //    }
        //    else if (strmsg.Equals("Nope"))
        //    {
        //        dispatcher.Invoke(() =>
        //        {
        //            r.label4.Text = "Nickname or E-mail is in use try again or log in";
        //        });
        //    }
        //}
        void CheckConnection(object sender, object e)    
        {
            if(!Login.c.Connected)
            {
                //disp.Invoke(() =>
                //{
                //    f.Nickname.Enabled = true;
                //    f.button1.Enabled = true;
                //    f.button4.Enabled = true;
                //    f.textBox4.Enabled = true;
                //    f.textBox5.Enabled = true;
                //    f.textBox1.Text += "\r\n Can't connect to server";
                //});
            }
        }
   }       
}

        
