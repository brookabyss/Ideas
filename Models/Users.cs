using System;
using System.Collections.Generic;
namespace Ideas.Models
{
    public class Users
    {
        public int UsersId {get;set;}
        public string FirstName {get;set;}
        public string Alias {get;set;}
        public string Email {get;set;}
        public string Password {get;set;}
        public DateTime CreatedAt {get;set;}
        public DateTime UpdatedAt {get;set;}
        public List<Likes> Likes  {get;set;}
        public List<Concepts> Concepts {get;set;}

        public Users(){
           Likes= new List<Likes>();
           Concepts= new List<Concepts>();
        }
    }
}