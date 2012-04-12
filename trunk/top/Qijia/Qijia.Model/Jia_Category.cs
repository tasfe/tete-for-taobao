using System;
namespace Qijia.Model
{
    public class Jia_Category
    {
        private String _CateId;
        public String CateId
        {
            get { return _CateId; }
            set { _CateId = value; }
        }
        private String _CateName;
        public String CateName
        {
            get { return _CateName; }
            set { _CateName = value; }
        }
        private String _HeadId;
        public String HeadId
        {
            get { return _HeadId; }
            set { _HeadId = value; }
        }
        private String _Tree;
        public String Tree
        {
            get { return _Tree; }
            set { _Tree = value; }
        }
        private Int32 _Level;
        public Int32 Level
        {
            get { return _Level; }
            set { _Level = value; }
        }
    }
}
