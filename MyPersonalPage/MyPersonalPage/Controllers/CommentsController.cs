using MyPersonalPage.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace MyPersonalPage.Controllers
{
    [RequireHttps]
    [Authorize(Roles="Admin,Moderator")]
    public class CommentsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
       
        // GET: 
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comment comment = db.Comments.Find(id);
            if (comment == null)
            {
                return HttpNotFound();
            }
            return View(comment);
            
        }
        
        [HttpPost]
        public ActionResult Edit([Bind(Include="Id,PostId, Body, UpdatedReason")] Comment comment)
        {
            
            if (ModelState.IsValid)
            {
                db.Comments.Attach(comment);
                comment.Updated = DateTimeOffset.Now;
                db.Entry(comment).Property(p => p.Updated).IsModified = true;
                db.Entry(comment).Property(p => p.Body).IsModified = true;
                db.Entry(comment).Property("UpdatedReason").IsModified = true;
                //db.Entry(post).State = EntityState.Modified;
                db.SaveChanges();
                Post post = db.Posts.Find(comment.PostId);
                
                return RedirectToAction("Details", "Posts", new {slug = post.Slug });
            }
            return View(comment);
        }

        // GET: 
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comment comment = db.Comments.Find(id);
            if (comment == null)
            {
                return HttpNotFound();
            }
            return View(comment);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Comment comment = db.Comments.Find(id);
            Post post = db.Posts.Find(comment.PostId);
            db.Comments.Remove(comment);
            db.SaveChanges();
            
            return RedirectToAction("Details", "Posts", new { slug = post.Slug });
            
        }
    }
}