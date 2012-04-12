using System;
namespace Qijia.Model
{
    public class Jia_Shop
    {
        private String _Nick;
        public String Nick
        {
            get { return _Nick; }
            set { _Nick = value; }
        }
        private String _ShopId;
        public String ShopId
        {
            get { return _ShopId; }
            set { _ShopId = value; }
        }
        private Int32 _IsExpired;
        public Int32 IsExpired
        {
            get { return _IsExpired; }
            set { _IsExpired = value; }
        }
        private DateTime _ExpireDate;
        public DateTime ExpireDate
        {
            get { return _ExpireDate; }
            set { _ExpireDate = value; }
        }
        private DateTime _AddDate;
        public DateTime AddDate
        {
            get { return _AddDate; }
            set { _AddDate = value; }
        }
    }
}
