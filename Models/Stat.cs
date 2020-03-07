
using System;  

namespace Terra.Models  
{  
    public class Stat
    {  
        public Guid StatId { get; set; }  
        public string CharacterId { get; set; }
        public int HitPoints { get; set; }
        public int ManaPoints { get; set; }
        public int Attack { get; set; }
        public int Defense { get; set; }
        public int Magic { get; set; }
        public int MagicDefense { get; set; }
        public int Agility { get; set; }
        public int Spirit { get; set; }
    }  
}
