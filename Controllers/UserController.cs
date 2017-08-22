using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Ideas.Models;
using System.Linq;
using MySQL.Data.EntityFrameworkCore.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Ideas.Controllers
{
    public class UserController : Controller
    {

        private LikeContext _context;
        public UserController(LikeContext context){
            _context=context;
        }
        // GET: /Home/
        [HttpGet]
        [Route("")]
        public IActionResult Index()
        {
            ViewBag.errors=new List<string>();
            return View();
        }

        public Users GetUser(){
            int? Id=HttpContext.Session.GetInt32("UserId");
            Users CurrentUser= _context.Users.SingleOrDefault(a=>a.UsersId==(int)Id);
            return CurrentUser;
        }



          [HttpPost]
        [Route("user/Register")]
        public IActionResult Register(UsersView newUser){
            ViewBag.errors=new List<string>();
            System.Console.WriteLine(ModelState.IsValid);
           
            if(ModelState.IsValid){
                 Users Usercheck= _context.Users.SingleOrDefault(user=>user.Email==newUser.Email);
                 if(Usercheck==null){
                     System.Console.WriteLine(newUser.FirstName);
                    Users createdUser= new Users{
                        FirstName=newUser.FirstName,
                        Alias=newUser.Alias,
                        Email=newUser.Email,
                        Password= newUser.Password,
                        CreatedAt= DateTime.Now,
                        UpdatedAt=DateTime.Now,
                    };
                    _context.Users.Add(createdUser);
                    _context.SaveChanges();
                    Users ReturnedUser = _context.Users.SingleOrDefault(user => user.Email == createdUser.Email);
                    System.Console.WriteLine($"Email from returned {ReturnedUser.Email}");
                    HttpContext.Session.SetInt32("UserId",(int)ReturnedUser.UsersId);  
                    HttpContext.Session.SetString("name",ReturnedUser.FirstName);
                    return RedirectToAction("Show","Bright");

                 }
                 else{
                     ViewBag.errors.Add("Email already exits");
                     return View("Index");
                 }
                    
        }
            else{
                foreach(var obj in ModelState.Values){
                    if(obj.Errors.Count>0){
                        foreach(var err in  obj.Errors){
                        ViewBag.errors.Add(err.ErrorMessage);
                        }
                    }
                    
                }
                
                return View("Index");
            }
        }

        [HttpPost]
        [Route("user/Login'")]
        public IActionResult Login(string Email, string Password){
           
            ViewBag.errors=new List<string>();
            int errors= 0;
           
                Users ReturnedUser= _context.Users.SingleOrDefault(user=>user.Email==Email);
               
                if(ReturnedUser!=null && ReturnedUser.Password==Password){
                    HttpContext.Session.SetInt32("UserId",(int)ReturnedUser.UsersId);
                    HttpContext.Session.SetString("name",ReturnedUser.FirstName);
                    return RedirectToAction("Show","Bright");
               }
               else{
                   ViewBag.errors.Add("Username or password is incorrect");
                   return View("Index");
               }
            }
        //
      
        //User Info

        [HttpGet]
        [Route("user/info/{ChosenId}")]
        public IActionResult UserInfo(int ChosenId){
        ViewBag.errors=new List<string>();
           List<Likes> likes= _context.Likes.Where(a=>a.UsersId==ChosenId).ToList();
           List<Concepts> concepts= _context.Concepts.Where(a=>a.UsersId==ChosenId).ToList();
           Users ReturnedUser= _context.Users.SingleOrDefault(user=>user.UsersId==ChosenId);
            ViewBag.ChosenUser=ReturnedUser;
            ViewBag.ConceptsCount=concepts.Count;
            ViewBag.LikesCount=likes.Count;
            return View("Info");
        }

        
        [HttpGet]
        [Route("user/logout")]
        public IActionResult Logout(){
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }


    }
}
