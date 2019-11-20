using MultiClientClient.Model;
using MultiClientWindow;
using MultiClientWindow.Model;
using MultiClientWindow.Viewmodel;
using MultiServe.Net.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace MultiClientClient.Viewmodel
{
    class Sending
    {
        Login login;
        NetworkStream st;

        public  void Send(String msg)
        {

            st = Login.stream;
            string nick = Login.nick;
                String IP = AddressFamily.InterNetwork.ToString();
                var m = new Msg_Info() { Message = msg, From = nick , IP = IP, MsgTime = "[" +DateTime.UtcNow+"]" };
                var msgJson = JsonConvert.SerializeObject(m);
                var g = msgJson.Length + "?";
                while (g.Length < 4)
                {
                    g = "0" + g;
                }
                byte[] data = System.Text.Encoding.ASCII.GetBytes(g + msgJson);
            try
            {
                st.Write(data, 0, data.Length);
            }
            catch (IOException)
            {
                MSG_Collection.dataList.Add(new Msg_Info() { From = "Error", Message = "No Connection" });
            }
        }

        public void CreateRoom(string Name,string creator,bool checkpass,string password) {
            st = Login.stream;
            var msg = JsonConvert.SerializeObject(new Room_info() { name = Name, roomCreatorName = creator,isPassword=checkpass,password=password });
            byte[] bmsg = System.Text.Encoding.ASCII.GetBytes("CRC?" + msg);
           st.Write(bmsg,0,bmsg.Length);
        }
        public void ChangeRoom(string Room,string password)
        {
            st = Login.stream;
            var msg = JsonConvert.SerializeObject(new Room_info() { name = Login.nick, NewRoom = Room, password= password});
            byte[] bmsg = System.Text.Encoding.ASCII.GetBytes("URC?" + msg);
            st.Write(bmsg, 0, bmsg.Length);
        }


    }
}
