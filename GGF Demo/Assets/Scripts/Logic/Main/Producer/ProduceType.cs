using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic
{
    /// <summary>
    /// 生成类型
    /// </summary>
    public enum ProduceType
    {
        random,//随机位置
        hight,//远离中线的位置
        mid,//一侧中间的位置
        low,//靠近中线的位置

    }
}
