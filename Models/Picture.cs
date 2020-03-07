
using System;  

namespace Terra.Models  
{  
    public class Picture
    {  
        public Guid PictureId { get; set; }  
        public string CharacterId { get; set; }
        public string Url { get; set; }
        public int Primary { get; set; }
    }  
}
