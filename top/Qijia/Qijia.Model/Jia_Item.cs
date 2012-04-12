using System;
namespace Qijia.Model
{
    public class Jia_Item
    {
        private String _Nick;
        public String Nick
        {
            get { return _Nick; }
            set { _Nick = value; }
        }
        private String _ItemId;
        public String ItemId
        {
            get { return _ItemId; }
            set { _ItemId = value; }
        }
        private String _TplId;
        public String TplId
        {
            get { return _TplId; }
            set { _TplId = value; }
        }
        private DateTime _UpdateDate;
        public DateTime UpdateDate
        {
            get { return _UpdateDate; }
            set { _UpdateDate = value; }
        }
        private String _PropertyText;
        public String PropertyText
        {
            get { return _PropertyText; }
            set { _PropertyText = value; }
        }
    }
}
