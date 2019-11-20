using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace MultiClientClient.Model
{
    public class Msg_Info
    {

        public String Message { get; set; }
        public String From { get; set; }
        public String MsgTime { get; set; }

        public String IP { get; set; }
    }
    public class MSG_Collection : Collection<Msg_Info>
    {
        public static ObservableCollection<Msg_Info> dataList = new ObservableCollection<Msg_Info>();
        public string Messages { get; set; }
    }
}
