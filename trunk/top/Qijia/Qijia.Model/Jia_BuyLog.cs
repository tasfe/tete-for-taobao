using System;
namespace Qijia.Model
{
    public class Jia_BuyLog
    {
        private String _Guid;
        public String Guid
        {
            get { return _Guid; }
            set { _Guid = value; }
        }
        private String _Nick;
        public String Nick
        {
            get { return _Nick; }
            set { _Nick = value; }
        }
        private String _Type;
        public String Type
        {
            get { return _Type; }
            set { _Type = value; }
        }
        private decimal _Price;
        public decimal Price
        {
            get { return _Price; }
            set { _Price = value; }
        }
        private DateTime _BuyDate;
        public DateTime BuyDate
        {
            get { return _BuyDate; }
            set { _BuyDate = value; }
        }
        private Int32 _IsOld;
        public Int32 IsOld
        {
            get { return _IsOld; }
            set { _IsOld = value; }
        }
        private DateTime _AddDate;
        public DateTime AddDate
        {
            get { return _AddDate; }
            set { _AddDate = value; }
        }
    }
}
