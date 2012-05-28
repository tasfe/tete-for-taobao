using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TeteTopApi.Entity
{
    public class AlipayDetail
    {
        /// <summary>
        /// 支付包红包GUID
        /// </summary>
        public string GUID { get; set; }

        /// <summary>
        /// 卡号
        /// </summary>
        public string Card { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string Pass { get; set; }

    }
}
