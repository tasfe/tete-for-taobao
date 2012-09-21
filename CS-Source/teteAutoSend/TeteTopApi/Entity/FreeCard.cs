using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TeteTopApi.Entity
{
    public class FreeCard
    {
        /// <summary>
        /// 是否记录地区免费
        /// </summary>
        public string IsFreeAreaList { get; set; }

        /// <summary>
        /// 免费地区
        /// </summary>
        public string AreaList { get; set; }

        /// <summary>
        /// 包邮卡名称
        /// </summary>
        public string Name { get; set; }
    }
}
