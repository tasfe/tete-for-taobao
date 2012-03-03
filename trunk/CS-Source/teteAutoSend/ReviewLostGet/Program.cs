using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TeteTopApi.Logic;

namespace ReviewLostGet
{
    class Program
    {
        static void Main(string[] args)
        {
            LostMessageGet lostGet = new LostMessageGet();
            lostGet.Start();

            Console.ReadLine();
        }
    }
}
