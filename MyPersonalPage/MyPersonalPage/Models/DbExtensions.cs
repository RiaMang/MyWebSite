using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyPersonalPage.Models
{
    public static class DbExtensions
    {
        public static bool Update<T>(ApplicationDbContext context, T item, params string[] changedPropertyNames) where T: class, new()
        {
            context.Set<T>().Attach(item);
            foreach (var propertyName in changedPropertyNames)
            {
                // If we cant find the property, this line will throw an exception,
                // which is good as we want to  know about it.
                context.Entry(item).Property(propertyName).IsModified = true;
            }
            return true;
        }
    }
}

// var updatedFields = new List<string>() {"Body", "UpdatedOn", "Published"};
// db.Update(comment, updatedFields.ToArray()); 
// db.SaveChanges();