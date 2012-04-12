using System;
namespace Qijia.Model
{
    public class Jia_Template
    {
        private String _TplId;
        public String TplId
        {
            get { return _TplId; }
            set { _TplId = value; }
        }
        private String _Tplname;
        public String Tplname
        {
            get { return _Tplname; }
            set { _Tplname = value; }
        }
        private Int32 _OrderIid;
        public Int32 OrderIid
        {
            get { return _OrderIid; }
            set { _OrderIid = value; }
        }
        private Int32 _Count;
        public Int32 Count
        {
            get { return _Count; }
            set { _Count = value; }
        }
        private String _TplImg;
        public String TplImg
        {
            get { return _TplImg; }
            set { _TplImg = value; }
        }
        private String _TplShort;
        public String TplShort
        {
            get { return _TplShort; }
            set { _TplShort = value; }
        }
        private String _TplHtml;
        public String TplHtml
        {
            get { return _TplHtml; }
            set { _TplHtml = value; }
        }
        private String _CateId;
        public String CateId
        {
            get { return _CateId; }
            set { _CateId = value; }
        }
    }
}
