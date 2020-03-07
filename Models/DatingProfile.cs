using System;  
using System.Collections.Generic;

namespace Terra.Models  
{  
    public class DatingProfile
    {  
        public Guid Id { get; set; }
        public string CharacterId { get; set; }  
        public string Bio { get; set; }
        public string Age { get; set; } 
        public string Gender { get; set; }
        public List<DatingResponse> Responses { get; set; }
        public List<Picture> Pictures { get; set; }
    }  

    public class DatingResponse
    {
        public Guid Id { get; set ;}
        public string CharacterId { get; set; }
        public string Reponse { get; set; }
    }
}