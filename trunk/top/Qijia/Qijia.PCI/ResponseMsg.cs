using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Qijia.PCI
{
    [Serializable]
    public class ResponseMsg
    {
        public string result { set; get; }

        public Dictionary<string, object> msgs { set; get; }
    }
}
