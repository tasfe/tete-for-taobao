using System;
namespace Qijia.Model
{
    public class Jia_ApiSucLog
    {
        private String _Guid;
        public String Guid
        {
            get { return _Guid; }
            set { _Guid = value; }
        }
        private String _ApiName;
        public String ApiName
        {
            get { return _ApiName; }
            set { _ApiName = value; }
        }
        private DateTime _ActDate;
        public DateTime ActDate
        {
            get { return _ActDate; }
            set { _ActDate = value; }
        }
        private String _Data;
        public String Data
        {
            get { return _Data; }
            set { _Data = value; }
        }
    }
}
