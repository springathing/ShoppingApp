using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace mblyakherShoppingApp.Models
{
    public class Universal : Controller
    {
        public ApplicationDbContext db = new ApplicationDbContext(); // access to database

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            if (User.Identity.IsAuthenticated)
            {
                var user = db.Users.Find(User.Identity.GetUserId()); // variables accessible for each view
                                        // imagine the stuff in the parenthesis being a GUID
                ViewBag.FirstName = user.FirstName;
                ViewBag.LastName = user.LastName;
                ViewBag.FullName = user.FullName;

                ViewBag.CartItems = db.CartItems.AsNoTracking().Where(c => c.CustomerId == user.Id).ToList().Count().ToString() + " Item(s)";
                // AsNoTracking() leaves no trail so accessing the database isn't left open
                // OnActionExecuting, everything happening in controller is done at same time, so on action executing gets executed after your controller 
                decimal Total = 0;
                foreach (var item in db.CartItems.AsNoTracking().Where(c => c.CustomerId == user.Id).ToList())
                {
                    Total += item.Count * item.Item.Price;
                }
                ViewBag.CartTotal = Total;
            }
            else
            {
                ViewBag.CartItems = "Please Log In";
            }
        }
    }
}