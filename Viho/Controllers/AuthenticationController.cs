using Microsoft.AspNetCore.Mvc;
using System;
using Viho.Models;
using Viho.web.DataDB;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using System.Text;
using System.Security.Cryptography;
using System.Threading.Tasks;



namespace Viho.Controllers
{

    public class AuthenticationController : Controller
    {
        public IActionResult LoginSimple()
        {
            return View();
        }

        private string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));

                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < hashedBytes.Length; i++)
                {
                    builder.Append(hashedBytes[i].ToString("x2"));
                }

                return builder.ToString();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel login)
        {
            DbRentalContext db = new DbRentalContext();
            var status = db.TbUsers.FirstOrDefault(m => m.UEmail == login.UEmail);

            if (status != null)
            {
                // Hash the entered password
                string hashedPassword = HashPassword(login.UPass);
                // Compare the hashed password with the stored hashed password
                if (status.UPass == hashedPassword)
                {

                    // Fetch the role name based on the role ID
                    var role = db.TbRoles.FirstOrDefault(r => r.RlId == status.URoleid);
                    string roleName = role?.RlDesc;
                    // Fetch the account ID
                    var account = db.TbUsers.FirstOrDefault(a => a.UUsername == status.UUsername);
                    int accountId = account?.UId ?? 0;
                    // Create claims for the authenticated user
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, login.UEmail),
                        new Claim("URoleid", status.URoleid.ToString()), // Add URoleId as a claim
                        new Claim("RoleName", roleName),
                        new Claim("Username", status.UUsername),
                        new Claim("AccountId", accountId.ToString()),
                     };

                    var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    // Sign in the user
                    var principal = new ClaimsPrincipal(identity);
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);




                    // Redirect based on URoleId
                    if (status.URoleid == 3) // Admin
                    {
                        return RedirectToAction("AdminDashboard", "Dashboard");
                    }
                    else if (status.URoleid == 4) // Investor
                    {
                        return RedirectToAction("InvestorDashboard", "Dashboard");
                    }
                }
            }
            // Set a flag to indicate login failure
            TempData["LogInStatus"] = 0;
            return RedirectToAction("LoginWithImageTwo");
        }



        public async Task<IActionResult> Logout()
        {

            // Sign out the user
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

         

            return RedirectToAction("LoginWithImageTwo", "Authentication");
        }

        public IActionResult LoginWithImageTwo()
        {
            return View();
        }
        public IActionResult LoginWithValidation()
        {
            return View();
        }
        public IActionResult LoginWithTooltip()
        {
            return View();
        }
        public IActionResult LoginWithSweetalert()
        {
            return View();
        }
        public IActionResult RegisterSimple()
        {
            return View();
        }
        public IActionResult RegisterWithBgImage()
        {
            return View();
        }
        public IActionResult RegisterWithBgVideo()
        {
            return View();
        }
        public IActionResult UnlockUser()
        {
            return View();
        }
        public IActionResult ForgetPassword()
        {
            return View();
        }
        public IActionResult CreatPassword()
        {
            return View();
        }
        public IActionResult Maintenance()
        {
            return View();
        }
    }
}
