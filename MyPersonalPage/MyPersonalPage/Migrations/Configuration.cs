namespace MyPersonalPage.Migrations
{
    using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using MyPersonalPage.Models;
using System;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<MyPersonalPage.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(MyPersonalPage.Models.ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
            var roleManager = new RoleManager<IdentityRole>(
                new RoleStore<IdentityRole>(context));

            if(!context.Roles.Any(r=>r.Name == "Admin"))
            {
                roleManager.Create(new IdentityRole { Name = "Admin" });
            }

            var userManager = new UserManager<ApplicationUser>(
                new UserStore<ApplicationUser>(context));
            ApplicationUser user;
            if(!context.Users.Any(r=>r.Email == "admin@coderfoundry.com"))
            {
                user= new ApplicationUser
                {
                    UserName = "admin@coderfoundry.com",
                    Email = "admin@coderfoundry.com",
                    FirstName = "Ria",
                    LastName = "Manglani",
                    DisplayName = "Ria Mang",
                };
                userManager.Create(user, "Abc123!");
            }
            else 
            {
                user = context.Users.Single(u => u.Email == "admin@coderfoundry.com");
            }
            if(!userManager.IsInRole(user.Id,"Admin"))
            {
                userManager.AddToRole(user.Id,"Admin");
            }


            // new from here --- for role
            var roleManager1 = new RoleManager<IdentityRole>(
                new RoleStore<IdentityRole>(context));

            if (!context.Roles.Any(r => r.Name == "Moderator"))
            {
                roleManager1.Create(new IdentityRole { Name = "Moderator" });
            }

            // adding new user
            var userManager1 = new UserManager<ApplicationUser>(
                new UserStore<ApplicationUser>(context));

            ApplicationUser user1;
            if (!context.Users.Any(r => r.Email == "lreaves@coderfoundry.com"))
            {
                user1 = new ApplicationUser
                    {
                        UserName = "lreaves@coderfoundry.com",
                        Email = "lreaves@coderfoundry.com",
                        FirstName = "L",
                        LastName = "Reaves",
                        DisplayName = "LReaves",
                        flag = "true"
                    };
                userManager1.Create(user1, "Password-1");
            }
            else
            {
                user1 = context.Users.Single(u => u.Email == "lreaves@coderfoundry.com");
            }
            if (!userManager1.IsInRole(user1.Id, "Moderator"))
            {
                userManager1.AddToRole(user1.Id, "Moderator");
            }

            // BDavis
            var userManager2 = new UserManager<ApplicationUser>(
                new UserStore<ApplicationUser>(context));

            ApplicationUser user2;
            if (!context.Users.Any(r => r.Email == "bdavis@coderfoundry.com"))
            {
                user2 = new ApplicationUser
                {
                    UserName = "bdavis@coderfoundry.com",
                    Email = "bdavis@coderfoundry.com",
                    FirstName = "Bobby",
                    LastName = "Davis",
                    DisplayName = "Bobby Davis",
                    flag = "true"
                };
                userManager2.Create(user2, "Password-1");
            }
            else
            {
                user2 = context.Users.Single(u => u.Email == "bdavis@coderfoundry.com");
            }
            if (!userManager2.IsInRole(user2.Id, "Moderator"))
            {
                userManager2.AddToRole(user2.Id, "Moderator");
            }

            // Andrew Jensen
            var userManager3 = new UserManager<ApplicationUser>(
                new UserStore<ApplicationUser>(context));

            ApplicationUser user3;
            if (!context.Users.Any(r => r.Email == "ajensen@coderfoundry.com"))
            {
                user3 = new ApplicationUser
                {
                    UserName = "ajensen@coderfoundry.com",
                    Email = "ajensen@coderfoundry.com",
                    FirstName = "Andrew",
                    LastName = "Jensen",
                    DisplayName = "Andrew Jensen",
                    flag = "true"
                };
                userManager3.Create(user3, "Password-1");
            }
            else
            {
                user3 = context.Users.Single(u => u.Email == "ajensen@coderfoundry.com");
            }
            if (!userManager3.IsInRole(user3.Id, "Moderator"))
            {
                userManager3.AddToRole(user3.Id, "Moderator");
            }

            // TJ Jones
            var userManager4 = new UserManager<ApplicationUser>(
                new UserStore<ApplicationUser>(context));

            ApplicationUser user4;
            if (!context.Users.Any(r => r.Email == "tjones@coderfoundry.com"))
            {
                user4 = new ApplicationUser
                {
                    UserName = "tjones@coderfoundry.com",
                    Email = "tjones@coderfoundry.com",
                    FirstName = "TJ",
                    LastName = "Jones",
                    DisplayName = "TJ Jones",
                    flag = "true"
                };
                userManager4.Create(user4, "Password-1");
            }
            else
            {
                user4 = context.Users.Single(u => u.Email == "tjones@coderfoundry.com");
            }
            if (!userManager4.IsInRole(user4.Id, "Moderator"))
            {
                userManager4.AddToRole(user4.Id, "Moderator");
            }

            // Thomas Parrish
            var userManager5 = new UserManager<ApplicationUser>(
                new UserStore<ApplicationUser>(context));

            ApplicationUser user5;
            if (!context.Users.Any(r => r.Email == "tparrish@coderfoundry.com"))
            {
                user5 = new ApplicationUser
                {
                    UserName = "tparrish@coderfoundry.com",
                    Email = "tparrish@coderfoundry.com",
                    FirstName = "Thomas",
                    LastName = "Parrish",
                    DisplayName = "Thomas Parrish",
                    flag = "true"
                };
                userManager5.Create(user5, "Password-1");
            }
            else
            {
                user5 = context.Users.Single(u => u.Email == "tparrish@coderfoundry.com");
            }
            if (!userManager5.IsInRole(user5.Id, "Moderator"))
            {
                userManager5.AddToRole(user5.Id, "Moderator");
            }

                        
        }
    }
}
