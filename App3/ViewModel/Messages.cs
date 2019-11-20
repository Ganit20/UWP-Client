using App3.View;
using MultiClientClient.Model;
using MultiClientClient.Viewmodel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Core;
using Windows.UI.Xaml.Controls;

namespace MultiClientWindow.Viewmodel
{
    class Messages
    {
        public void UserList(string strmsg, Chat chat)
        {
            strmsg = strmsg.Substring(strmsg.IndexOf('?') + 1, strmsg.IndexOf("?", strmsg.IndexOf('?') + 1) - 4);
            Task.Factory.StartNew(async () =>
            {
            string[] room = strmsg.Split('~');
            int number = 1;
                await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                    chat.UserList.Items.Clear();
                    foreach (string item in room)
                    {
                        if (item != "") { chat.UserList.Items.Add(item); }
                    }
                });
            number++;
        });}

     
        
        public void RoomList(string strmsg, Chat chat)
        {
            Task.Factory.StartNew(async () =>
            {
                strmsg = strmsg.Substring(strmsg.IndexOf('?') + 1, strmsg.IndexOf("?", strmsg.IndexOf('?') + 1) - 4);
                Receiving.json = JsonConvert.DeserializeObject<Dictionary<string, bool>>(strmsg);
                await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                    chat.RoomList.Items.Clear();
                    foreach (KeyValuePair<string, bool> e in Receiving.json)
                    {
                        chat.RoomList.Items.Add(e.Key);
                    }
                });
            });
        }
        public void Message(string strmsg, Chat chat)
        {
            Task.Factory.StartNew(async () =>
            {
                try
                {
                    strmsg = strmsg.Substring(strmsg.IndexOf('?', 0, 4) + 1, strmsg.IndexOf("?END") - 4);
                    var rec = JsonConvert.DeserializeObject<Msg_Info>(strmsg);
                    await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                    {
                        MSG_Collection.dataList.Add( new Msg_Info() { Message =rec.Message , From = rec.From   ,MsgTime=rec.MsgTime} );
                        chat.Messages.ScrollIntoView(chat.Messages.Items[MSG_Collection.dataList.Count - 1]);
                    });
                }
                catch (FormatException) { }
                catch (JsonReaderException) { }
                catch (JsonSerializationException) { }
            });
        }
        public void ChangeRoom(string strmsg, Chat chat)
        {
            Task.Factory.StartNew(async () =>
            {
                try
                {
                    strmsg = strmsg.Substring(strmsg.IndexOf('?', 0, 4) + 1, strmsg.IndexOf("?END") - 4);
                    await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                    {
                        MSG_Collection.dataList.Add(new Msg_Info() { Message = strmsg, From = "Info: ", MsgTime =  DateTime.UtcNow.ToString() });
                    });
                }
                catch (FormatException) { }
                catch (JsonReaderException) { }
                catch (JsonSerializationException) { }
            });
        }
    }
}

