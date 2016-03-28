using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Web;

namespace MyPersonalPage.Models
{
    public static class HelperExtensions
    {
        //public static T GetUserPic<T>(this IIdentity user)
        //{
        //    var claimsIdentity = (ClaimsIdentity)user;
        //    var PicClaim = claimsIdentity.Claims.FirstOrDefault(c => c.Type == "Pic");
        //    if (PicClaim != null)
        //        return (T)Convert.ChangeType(PicClaim.Value, typeof(T));
        //    else
        //        return default(T);
        //}

        public static string GetUserPic(this IIdentity user)
        {
            var claimsIdentity = (ClaimsIdentity)user;
            var PicClaim = claimsIdentity.Claims.FirstOrDefault(c => c.Type == "Pic");
            if (PicClaim != null)
                return PicClaim.Value;
            else
                return null;
        }

        public static string GetDisplayName(this IIdentity user)
        {
            var claimsIdentity = (ClaimsIdentity)user;
            var DNameClaim = claimsIdentity.Claims.FirstOrDefault(c => c.Type == "DName");
            if (DNameClaim != null)
                return DNameClaim.Value;
            else
                return null;
        }

    }
}