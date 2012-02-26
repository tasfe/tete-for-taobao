using System;
using System.Collections.Generic;
using System.Text;
using TeteTopApi;

namespace ReviewSend
{
    class Program
    {
        static void Main(string[] args)
        {
            Api top = new Api("12159997", "614e40bfdb96e9063031d1a9e56fbed5", "", "http://stream.api.taobao.com/stream");
            string result = top.ConnectServer();

            //Console.ReadLine();
        }
    }
}
