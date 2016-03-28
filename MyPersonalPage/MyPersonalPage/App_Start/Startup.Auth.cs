﻿using System;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.Google;
using Owin;
using MyPersonalPage.Models;
using Owin.Security.Providers.LinkedIn;
using System.Configuration;
using Microsoft.Owin.Security.Facebook;
using System.Threading.Tasks;
using System.Security.Claims;

namespace MyPersonalPage
{
    public partial class Startup
    {
        // For more information on configuring authentication, please visit http://go.microsoft.com/fwlink/?LinkId=301864
        public void ConfigureAuth(IAppBuilder app)
        {
            // Configure the db context, user manager and signin manager to use a single instance per request
            app.CreatePerOwinContext(ApplicationDbContext.Create);
            app.CreatePerOwinContext<ApplicationUserManager>(ApplicationUserManager.Create);
            app.CreatePerOwinContext<ApplicationSignInManager>(ApplicationSignInManager.Create);

            // Enable the application to use a cookie to store information for the signed in user
            // and to use a cookie to temporarily store information about a user logging in with a third party login provider
            // Configure the sign in cookie
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/Login"),
                Provider = new CookieAuthenticationProvider
                {
                    // Enables the application to validate the security stamp when the user logs in.
                    // This is a security feature which is used when you change a password or add an external login to your account.  
                    OnValidateIdentity = SecurityStampValidator.OnValidateIdentity<ApplicationUserManager, ApplicationUser>(
                        validateInterval: TimeSpan.FromMinutes(30),
                        regenerateIdentity: (manager, user) => user.GenerateUserIdentityAsync(manager))
                }
            });            
            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

            // Enables the application to temporarily store user information when they are verifying the second factor in the two-factor authentication process.
            app.UseTwoFactorSignInCookie(DefaultAuthenticationTypes.TwoFactorCookie, TimeSpan.FromMinutes(5));

            // Enables the application to remember the second login verification factor such as phone or email.
            // Once you check this option, your second step of verification during the login process will be remembered on the device where you logged in from.
            // This is similar to the RememberMe option when you log in.
            app.UseTwoFactorRememberBrowserCookie(DefaultAuthenticationTypes.TwoFactorRememberBrowserCookie);

            // Uncomment the following lines to enable logging in with third party login providers
            //app.UseMicrosoftAccountAuthentication(
            //    clientId: "",
            //    clientSecret: "");

            //app.UseTwitterAuthentication(
            //   consumerKey: "",
            //   consumerSecret: "");
            
            var x = new FacebookAuthenticationOptions();
            x.AppId = ConfigurationManager.AppSettings["FBAppId"];
            x.AppSecret = ConfigurationManager.AppSettings["FBAppSecret"];
            x.Provider = new FacebookAuthenticationProvider()
            {
                OnAuthenticated = (context) =>
                {
                    //Get the access token from FB and store it in the database and
                    //use FacebookC# SDK to get more information about the user
                    var profilePicturePath = "http://graph.facebook.com/{context.Id}/picture?type=large";  //$"http://graph.facebook.com/{context.Id}/picture?type=large";
                    context.Identity.AddClaim(new Claim("picture", profilePicturePath));
                    
                    //context.Identity.AddClaim(new System.Security.Claims.Claim("FacebookAccessToken", context.AccessToken));
                    return Task.FromResult(0);
                }
            };
            x.SignInAsAuthenticationType = DefaultAuthenticationTypes.ExternalCookie;
            app.UseFacebookAuthentication(x);
            //app.UseFacebookAuthentication(

            //   // appId: "809058965810116",
            //   // appSecret: "4154c5b32e0ad0b1f69e69d084d4712c"); 

            //    appId: ConfigurationManager.AppSettings["FBAppId"],
            //   appSecret: ConfigurationManager.AppSettings["FBAppSecret"]); // for both
            var googleOAuth2AuthenticationOptions = new GoogleOAuth2AuthenticationOptions
            {
                ClientId = ConfigurationManager.AppSettings["GClientId"],
                ClientSecret = ConfigurationManager.AppSettings["GClientSecret"],
                //CallbackPath = new PathString("/Account/ExternalGoogleLoginCallback"),
                Provider = new GoogleOAuth2AuthenticationProvider()
                {
                    OnAuthenticated = (context) =>
                    {
                        context.Identity.AddClaim(new Claim("picture", context.User.GetValue("image").ToString()));
                        //context.Identity.AddClaim(new Claim("profile", context.User.GetValue("profile").ToString()));
                        //context.Identity.AddClaim(new Claim("image", ((string)context.User.GetValue("image").First).Replace("sz=50", "sz=250")));
                        return Task.FromResult(0);
                    }
                }
            };

            googleOAuth2AuthenticationOptions.Scope.Add("email");

            app.UseGoogleAuthentication(googleOAuth2AuthenticationOptions);
        //    app.UseGoogleAuthentication(new GoogleOAuth2AuthenticationOptions()
        //    {
        //        ClientId = ConfigurationManager.AppSettings["GClientId"],
        //        ClientSecret = ConfigurationManager.AppSettings["GClientSecret"] // for both
                

        //});

            app.UseLinkedInAuthentication(ConfigurationManager.AppSettings["LIClientId"], ConfigurationManager.AppSettings["LIClientSecret"]); //Both
        }
    }
}