using System;
namespace Ideas.Models
{
    public class Likes
    {
        public int LikesId {get;set;}
        
        public int UsersId {get;set;}
        public Users Users {get;set;}

        public int ConceptsId {get;set;}
        public Concepts Concepts {get;set;}


        public DateTime CreatedAt {get;set;}
        public DateTime UpdatedAt {get;set;}
    }
}