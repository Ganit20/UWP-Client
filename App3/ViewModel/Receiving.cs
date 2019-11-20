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

namespace MultiClientClient.Viewmodel
{
    public class Receiving
    {
        public static Dictionary<string, bool> json;
        Chat view;
        public void Reading(Chat obj, NetworkStream st)
        {
            //DispatcherTimer timer = new DispatcherTimer();
            //timer.Tick += CheckConnection;
            //timer.Interval = new TimeSpan(0,0,3);
            view = obj;
            //timer.Start();
            NetworkStream stream = st;
            while (true)
            {
                try
                {
                    String response = String.Empty;
                    Byte[] ByteLength = new byte[4];
                    Array.Clear(ByteLength, 0, ByteLength.Length);
                    Int32 bytes = stream.Read(ByteLength, 0, 4);

                    if (stream.DataAvailable)
                    {
                        try
                        {
                            String v = System.Text.Encoding.ASCII.GetString(ByteLength, 0, 4);
                            String d = v.Substring(0, v.IndexOf('?', 0, 4));
                            int bl = int.Parse(d);
                            Byte[] data = new Byte[bl];
                            Int32 msg = stream.Read(data, 0, bl);
                            string strmsg = System.Text.Encoding.ASCII.GetString(data);
                            string vstrmsg = strmsg.Substring(0, strmsg.IndexOf('?'));
                            //if (vstrmsg.Equals("RDC"))
                            //{
                            //    Register r = (Register)form;
                            //    Register(strmsg, dispatcher, r);
                            //}
                            //else f = (Form1)form;
                            switch (vstrmsg)
                            {

                                case "BAN":
                                    new Messages().Message(strmsg,view); break;
                                case "USE":
                                    new Messages().UserList(strmsg, view);
                                    break;
                                case "RCC":
                                    new Messages().RoomList(strmsg, view );
                                    break;
                                case "MSG":
                                    new Messages().Message(strmsg, view);
                                    break;
                                case "SSG":
                                    new Messages().ChangeRoom(strmsg, view);
                                    break;

                            }
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

        
