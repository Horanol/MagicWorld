using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts
{
    public class Armor:Item
    {
        public  int ATK { get; set; }
        public  int DEF { get; set; }
        public  int INT { get; set; }//智力
        public  int LUK { get; set; }//幸运值
        public  int HP { get; set; }
        public  int MP { get; set; }
        public  float CRI { get; set; }//暴击率
        public  float AGL { get; set; }//敏捷力
        public  float RCV { get; set; }//恢复力
        public  float HIT { get; set; }//命中率
        public  float AVD { get; set; }//回避
        public Quality ArmorQuality;
    }
}
