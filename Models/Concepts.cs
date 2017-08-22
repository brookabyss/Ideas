using System;
using System.Collections.Generic;
namespace Ideas.Models
{
    public class Concepts
    {
        public int ConceptsId {get;set;}
        public string Content {get;set;}
        public int LikeCount {get;set;}
        public DateTime CreatedAt {get;set;}
        public DateTime UpdatedAt {get;set;}
        public List<Likes> Likes  {get;set;}
        public int UsersId {get;set;}
        public Users Users {get;set;}

        public Concepts(){
           Likes= new List<Likes>();
           
        }
    }
}