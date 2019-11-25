using App3;
using App3.ViewModel;
using MultiClientClient.Model;
using MultiClientWindow;
using MultiClientWindow.Model;
using MultiClientWindow.Viewmodel;
using MultiServe.Net.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using Windows.Networking;
using Windows.Networking.Connectivity;

namespace MultiClientClient.Viewmodel
{
    class Sending
    {
        Login login;
        NetworkStream st;

        public  void Send(String msg)
        {

            st = Login.stream;
            string IP= null;
            string nick = Login.nick;
           

            

            var m = new Msg_Info() { Message = msg, From = nick , IP = IP, MsgTime = "[" +DateTime.UtcNow+"]" };
                var msgJson = JsonConvert.SerializeObject(m);
            var U_binfo = new Encryption().Encrypt(msgJson);
            byte[] b = new TextOperations().addByte(Encoding.UTF8.GetBytes("MSG?" + new TextOperations().intLength(U_binfo.Length)), U_binfo);

            try
            {
                st.Write(b, 0, b.Length);
            }
            catch (IOException)
            {
                MSG_Collection.dataList.Add(new Msg_Info() { From = "Error", Message = "No Connection" });
            }
        }

        public void CreateRoom(string Name,string creator,bool checkpass,string password) {
            st = Login.stream;
            var msg = JsonConvert.SerializeObject(new Room_info() { name = Name, roomCreatorName = creator,isPassword=checkpass,password=password });
            var U_binfo = new Encryption().Encrypt(msg);
            byte[] b = new TextOperations().addByte(Encoding.UTF8.GetBytes("CRC?" + new TextOperations().intLength(U_binfo.Length)), U_binfo);

            st.Write(b,0,b.Length);
        }
        public void ChangeRoom(string Room,string password)
        {
            st = Login.stream;
            var msg = JsonConvert.SerializeObject(new Room_info() { name = Login.nick, NewRoom = Room, password= password});
            var U_binfo = new Encryption().Encrypt(msg);
            byte[] b = new TextOperations().addByte(Encoding.UTF8.GetBytes("URC?" + new TextOperations().intLength(U_binfo.Length)), U_binfo);
            st.Write(b, 0, b.Length);
        }


    }
}
