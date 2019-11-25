using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App3.ViewModel
{
    class TextOperations
    {
        public string intLength(int leng)
        {
            var length = leng.ToString() + "?";
            while (length.IndexOf("?") != 3)
            {
                length = "0" + length;
            }
            return length;
        }

        public string MessageLength(string leng)
        {
            var length = leng.Length+ "?" +leng;
            while (length.IndexOf("?")!= 3)
            {
                length = "0" + length;
            }
            return length;
        }
        public byte[] addByte(byte[] a1, byte[] a2)
        {
            byte[] newArray = new byte[a1.Length + a2.Length];
            System.Buffer.BlockCopy(a1, 0, newArray, 0, a1.Length);
            System.Buffer.BlockCopy(a2, 0, newArray,a1.Length,a2.Length);
            return newArray;
        }
        public int CheckLength(string v)
        {
            String d = v.Substring(v.IndexOf('?'),v.IndexOf('?',v.IndexOf('?') + 1));
            int bl = int.Parse(d);
            return bl + 2;
        }
    }
}
