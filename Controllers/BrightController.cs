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
    public class BrightController : Controller
    {

        private LikeContext _context;
        public BrightController(LikeContext context){
            _context=context;
        }

        // Get All Concepts
         public List<Concepts> GetConcepts (){
            List<Concepts> concepts= _context.Concepts
                                    .Include(a=>a.Users)
                                    .OrderByDescending(a=>a.LikeCount).ToList();
                                    return concepts;

        }
        

        [HttpGet]
        [Route("bright_ideas")]
        public IActionResult Show()
        {
            int? loggedId=HttpContext.Session.GetInt32("UserId");
            if(loggedId==null){
                return RedirectToAction("Index","User");
            }
            ViewBag.errors=new List<string>();
            ViewBag.name= HttpContext.Session.GetString("name");
            ViewBag.Concepts= GetConcepts();
            int? Id=HttpContext.Session.GetInt32("UserId");
            ViewBag.UsersId= (int)Id;
            return View("Show");
        }
        // create Concept
        [HttpPost]
        [Route("concepts/create")]
        public IActionResult CreateConcept(string Content){

            int? loggedId=HttpContext.Session.GetInt32("UserId");
            if(loggedId==null){
                return RedirectToAction("Index","User");
            }


            ViewBag.errors=new List<string>();
            if(Content.Length<=0){
                ViewBag.errors.Add("Content Can't be empty");
            }
            int? Id=HttpContext.Session.GetInt32("UserId");
            
               Concepts concept= new Concepts{
                    UsersId=(int)Id,
                    Content=Content,
                    CreatedAt= DateTime.Now,
                    UpdatedAt=DateTime.Now,
                };
                 _context.Concepts.Add(concept);
                 _context.SaveChanges();
                return RedirectToAction("Show");

        }
        // LikeConcept

        [HttpGet]
        [Route("concepts/like/{ConceptsId}")]
        public IActionResult LikeConcept(int ConceptsId){

            int? loggedId=HttpContext.Session.GetInt32("UserId");
            if(loggedId==null){
                return RedirectToAction("Index","User");
            }
            System.Console.WriteLine("****************************Inside Like Concept");
                int? UsersId=HttpContext.Session.GetInt32("UserId");

                Concepts concept= _context.Concepts.SingleOrDefault(a=>a.ConceptsId==ConceptsId);
                Likes likes= new Likes{
                    UsersId=(int)UsersId,
                    ConceptsId= ConceptsId,
                    CreatedAt= DateTime.Now,
                    UpdatedAt=DateTime.Now,
                };
                 _context.Likes.Add(likes);
                  concept.LikeCount+=1;
                 _context.SaveChanges();
                return RedirectToAction("Show");

        }

        // DeleteConcept 
        [HttpGet]
        [Route("concepts/delete/{ConceptsId}")]
        public IActionResult DeleteConcept(int ConceptsId){

            int? loggedId=HttpContext.Session.GetInt32("UserId");
            if(loggedId==null){
                return RedirectToAction("Index","User");
            }
            System.Console.WriteLine("****************************Inside Delete Concept");
                int? UsersId=HttpContext.Session.GetInt32("UserId");
                Concepts concept= _context.Concepts.SingleOrDefault(a=>a.ConceptsId==ConceptsId);
                if((int)UsersId==concept.UsersId){
                    _context.Concepts.Remove(concept);
                    List <Likes> likes= _context.Likes.Where(a=>a.ConceptsId==concept.ConceptsId).ToList();
                    foreach(var like in likes){
                        _context.Likes.Remove(like);
                         
                    }
                    _context.SaveChanges();
                }
                return RedirectToAction("Show");

        }

         // To Concept Likes 
        [HttpGet]
        [Route("concepts/likes/{ConceptsId}")]
        public IActionResult ToLikes(int ConceptsId){
            int? loggedId=HttpContext.Session.GetInt32("UserId");
            if(loggedId==null){
                return RedirectToAction("Index","User");
            }
            ViewBag.errors=new List<string>();
            System.Console.WriteLine("****************************likes Concept");
                int? UsersId=HttpContext.Session.GetInt32("UserId");
                Concepts concept= _context.Concepts.Include(a=>a.Users).SingleOrDefault(a=>a.ConceptsId==ConceptsId);
               List<Likes> likes= _context.Likes
                                    .Include(a=>a.Users)
                                    .Where(a=>a.ConceptsId==ConceptsId)

                                    .ToList();

              var likers= likes.GroupBy(p => new {p.UsersId, p.ConceptsId} )
                                .Select(g => g.First())
                                .ToList();
              
                
                ViewBag.Likes= likers;
                ViewBag.Concept= concept;
                return View("Likes");

        }









    }

    
    
}
