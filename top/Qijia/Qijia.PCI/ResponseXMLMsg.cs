using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Qijia.PCI
{
    [Serializable]
    public class ResponseXMLMsg
    {
        public string result { set; get; }

        public List<msg> msgs { set; get; }
    }
}
