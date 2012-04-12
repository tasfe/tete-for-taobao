using System;
namespace Qijia.Model
{
    public class Jia_ImgCustomer
    {
        private String _Guid;
        public String Guid
        {
            get { return _Guid; }
            set { _Guid = value; }
        }
        private String _ItemId;
        public String ItemId
        {
            get { return _ItemId; }
            set { _ItemId = value; }
        }
        private String _Tag;
        public String Tag
        {
            get { return _Tag; }
            set { _Tag = value; }
        }
        private String _JiaImg;
        public String JiaImg
        {
            get { return _JiaImg; }
            set { _JiaImg = value; }
        }
    }
}
