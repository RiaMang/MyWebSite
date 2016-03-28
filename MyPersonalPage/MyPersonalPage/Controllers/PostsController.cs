using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MyPersonalPage.Models;
using PagedList;
using PagedList.Mvc;
using Microsoft.AspNet.Identity;
using System.IO;


namespace MyPersonalPage.Controllers
{
    
    public class PostsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        
        [AllowAnonymous]
        [RequireHttps]
        public ActionResult Index(int ? page, string Query, string cat)
        {
            int pageSize = 3; // display 3 blog posts at a time on this page.
            int pageNumber = (page ?? 1);
            var t = db.Posts.AsQueryable();
            ViewBag.Query = Query;
            ViewBag.Cat = cat;

            if(cat != null && cat != "")
                {  
                    t= t.Where(m=>m.Cat.Equals(cat) );
                }
               
            if(Query != null && Query != "")
            {  
                t = t.Where(p => p.Title.Contains(Query) || Query == "" || Query == null ||
                                        p.Body.Contains(Query) || p.Comments.Any(c => c.Body.Contains(Query) || c.Author.DisplayName.Contains(Query)));
            }
            var posts = t.OrderByDescending(p => p.Created).ToPagedList(pageNumber, pageSize);
            return View(posts);
        }
             

        // GET: Posts
        [Authorize(Roles = "Admin")]
        public ActionResult Admin()
        {
            return View(db.Posts.ToList());
        }

        // GET: Posts/Details/5
        // GET: Blog/{slug}
        [AllowAnonymous]
        public ActionResult Details(string Slug)
        {
            if (String.IsNullOrWhiteSpace(Slug))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post post = db.Posts.FirstOrDefault(p => p.Slug == Slug);
            if (post == null)
            {
                return HttpNotFound();
            }
                    
            return View(post);
        }

        // GET: Posts/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Posts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]                      
        public ActionResult Create(Post post, HttpPostedFileBase image, string tags)
        {
            if(image!= null && image.ContentLength > 0)
            {
                //check the file name to make sure its an image
                var ext = Path.GetExtension(image.FileName).ToLower();
                if (ext != ".png" && ext != ".jpg" && ext != ".jpeg" && ext != ".gif" && ext != ".bmp")
                    ModelState.AddModelError("image", "Invalid Format."); 
            }
            
            if (ModelState.IsValid)
            {
                var Slug = StringUtilities.URLFriendly(post.Title);
                if(String.IsNullOrWhiteSpace(Slug))
                {
                    ModelState.AddModelError("Title", "Invalid title.");
                    return View(post);
                }
                if( db.Posts.Any(p=>p.Slug == Slug))
                {
                    ModelState.AddModelError("Title", "The title must be unique.");
                    return View(post);
                }
                else
                {
                    if (image != null)
                    {
                        //relative server path
                        var filePath = "/Uploads/";
                        // path on physical drive on server
                        var absPath = Server.MapPath("~" + filePath); //Server.MapPath($"~{filepath}");
                        // media url for relative path
                        post.MediaURL = filePath + image.FileName; // $"{filepath}{image.FileName}";
                        //save image
                        image.SaveAs(Path.Combine(absPath, image.FileName));

                    }
                    //char [] delim = { ' ' };
                    //var stags = tags.Split(delim);
                    //var s = from t in stags
                    //        db.Posts.Tags.Add(t);

                    post.Created = System.DateTimeOffset.Now;
                    post.Slug = Slug;

                    db.Posts.Add(post);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
          
            return View(post);
        }


        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult CreateComment([Bind(Include = "PostId,Body,Cat")] Comment comment)
        {
            var slug = db.Posts.Find(comment.PostId).Slug;
            if(ModelState.IsValid)
            {
                comment.AuthorId = User.Identity.GetUserId();
                comment.Created = System.DateTimeOffset.Now;
                
                db.Comments.Add(comment);
                db.SaveChanges();
                //return RedirectToAction("Details",new { Slug = slug});
            }
            return RedirectToAction("Details", new { Slug = slug });
        }

        // GET: Posts/Edit/5
        [Authorize (Roles="Admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post post = db.Posts.Find(id);
            if (post == null)
            {
                return HttpNotFound();
            }
            return View(post);
        }

        // POST: Posts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Updated,Title,Body,MediaURL")] Post post)
        {
            if (ModelState.IsValid)
            {
                db.Posts.Attach(post);
                post.Updated = System.DateTimeOffset.Now;

                db.Entry(post).Property(p => p.Body).IsModified = true;
                db.Entry(post).Property("Title").IsModified = true;
                db.Entry(post).Property(p => p.Updated).IsModified = true;
                db.Entry(post).Property(p => p.MediaURL).IsModified = true;
                
                //db.Entry(post).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(post);
        }

        // GET: Posts/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post post = db.Posts.Find(id);
            if (post == null)
            {
                return HttpNotFound();
            }
            return View(post);
        }

        // POST: Posts/Delete/5
        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Post post = db.Posts.Find(id);
            db.Posts.Remove(post);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

       
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
