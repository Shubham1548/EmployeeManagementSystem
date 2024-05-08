using EmployeeManagementSystem.Data;
using EmployeeManagementSystem.LoginModels;
using EmployeeManagementSystem.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mail;
using System.Security.Claims;
using ForgotPassword = EmployeeManagementSystem.Models.ForgotPassword;

namespace EmployeeManagementSystem.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDBContext _context;

        public AccountController(ApplicationDBContext context)
        {
            _context = context;
        }


        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginVM logs)
        {

            var user = _context.users.Where(u => u.EmailId == logs.userName).SingleOrDefault();

            if (user.EmailId != null)
            {
                if (user.UserRole == "Admin")
                {
                    var isValid = (user.EmailId == logs.userName && user.UserPassword == logs.password);    
                    if(isValid)
                    {
                        var identity = new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, logs.userName) },
                            CookieAuthenticationDefaults.AuthenticationScheme);

                        var pri = new ClaimsPrincipal(identity);
                        HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, pri);
                        HttpContext.Session.SetString("username", logs.userName);

                        return RedirectToAction("Dashboard", "Users");


                    }
                    else
                    {
                        ModelState.AddModelError("", "Invalid username or password");
                        return View(logs);
                    }
                   
                }
                else
                {
                    var isValid = (user.EmailId == logs.userName && user.UserPassword == logs.password);
                    if (isValid)
                    {
                        var identity = new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, logs.userName) },
                            CookieAuthenticationDefaults.AuthenticationScheme);

                        var pri = new ClaimsPrincipal(identity);
                        HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, pri);
                        HttpContext.Session.SetString("username", logs.userName);

                        return RedirectToAction("Dashboard", "Users");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Invalid username or password");
                        return View(logs);
                    }
                }
            }
            else
            {
                ModelState.AddModelError("", "Invalid user");
                return View(logs);
            }
        }


        public IActionResult Register(string? returnUrl = null)
        {
            RegisterVM rm = new RegisterVM();
            rm.Role = "Admin";

            ViewData["ReturnUrl"] = returnUrl;
            return View(rm);

        }

        [HttpPost]
        public IActionResult Register(RegisterVM regs)
        {
            try
            {

                if (regs.UniqueKey == "MUSOFT01")
                {
                    var highestAdminNumber = _context.users.ToList()
                              .Where(entity => entity.UserRole.StartsWith("Admin"))
                                .Select(entity =>
                                {
                                    if (int.TryParse(entity.UserRole.Substring(5), out int number))
                                        return number;
                                    return 0;
                                })
                                .DefaultIfEmpty(0)
                                .Max();


                    highestAdminNumber++;

                    UserInfo user = new UserInfo
                    {
                        UserCode = "Admin" + highestAdminNumber,
                        UserRole =  "Admin",
                        FirstName = regs.FirstName,
                        MiddleName = regs.MiddleName,
                        LastName = regs.LastName,
                        EmailId = regs.Email,
                        UserPassword = regs.confirmPassword,
                    };

                    var result = _context.users.Add(user);
                    _context.SaveChanges();


                    if (result != null)
                    {
                        return RedirectToAction("Dashboard", "Users");

                    }

                    return View(regs);
                }
                else
                {
                    TempData["Error"] = $"Please enter valid uniquekey";
                    return View(regs);
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error : {ex.Message}";
                return View(regs);
            }

        }

        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ForgotPassword(ForgotPasswordVM model)
        {
            if (ModelState.IsValid)
            {
             
                string otp = GenerateOTP();

                ForgotPassword ot = new ForgotPassword();

                if (SendEmail(model.Email, otp))
                {
                   
                    ot.TempOtp = otp;
                    ot.TempMail = model.Email;
                    _context.forgots.Add(ot);
                    _context.SaveChanges();

                   
                    return RedirectToAction("VerifyOTP");
                }
                else
                {
                    TempData["ErrorMessage"] = "Failed to send OTP. Please try again later.";
                }
            }

      
            return View(model);
        }

   
        private string GenerateOTP()
        {
            
            Random random = new Random();
            return random.Next(100000, 999999).ToString();
        }

     
        private bool SendEmail(string email, string otp)
        {
            try
            {
                SmtpClient client = new SmtpClient("smtp.gmail.com")
                {
                    Port = 587,
                    Credentials = new System.Net.NetworkCredential("morenikhil213@gmail.com", "imqv iimw mjur yran"),
                    EnableSsl = true,
                };

           
                MailMessage mailMessage = new MailMessage
                {
                    From = new MailAddress("morenikhil213@gmail.com"),
                    Subject = "Password Reset OTP",
                    Body = $"Your OTP for password reset is: {otp}",
                    IsBodyHtml = true,
                };
                mailMessage.To.Add(email);

          
                client.Send(mailMessage);


                return true;
            }
            catch (Exception ex)
            {
            
                Console.WriteLine(ex.Message);
                return false;
            }
        }

       
        public IActionResult VerifyOTP()
        { 
            
            return View();
        }

       
        [HttpPost]
        public IActionResult VerifyOTP(string otp)
        {
            var ot = _context.forgots.Where(u => u.TempOtp == otp).SingleOrDefault();

            
            if (ot != null)
            {
                string? savedOTP = ot.TempOtp;
                string? email = ot.TempMail;
              

                
                if (otp == savedOTP)
                {
                   
                    TempData["SuccessMessage"] = "OTP verified successfully.";
                    return RedirectToAction("ResetPassword", new { email });
                }
            }

            TempData["ErrorMessage"] = "Invalid OTP. Please try again.";
            return View();
        }


  
        public IActionResult ResetPassword(string email)
        {
            var ot = _context.forgots.Where(u => u.TempMail == email).SingleOrDefault();
   
            ViewBag.Email = email;
            _context.forgots.Remove(ot);
            _context.SaveChanges();
            return View();
        }

      
        [HttpPost]
        public IActionResult ResetPassword(string email, string newPassword)
        {
            var newpass = _context.users.FirstOrDefault(u => u.EmailId == email);
            if (newpass != null)
            {
                newpass.UserPassword = newPassword;
                _context.users.Update(newpass);
                _context.SaveChanges();
                TempData["SuccessMessage"] = "Password reset successfully. You can now log in with your new password.";
                return RedirectToAction("Login");
            }
            TempData["SuccessMessage"] = "Invalid Email ID";
            return RedirectToAction("Login");
        }


        public IActionResult Logout()
        {
            HttpContext.SignOutAsync( CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Account");
        }
    }

    
}
