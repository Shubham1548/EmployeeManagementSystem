using EmployeeManagementSystem.Data;
using EmployeeManagementSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagementSystem.Controllers
{
    [Authorize]
    public class UsersController : Controller
    {
        private readonly ApplicationDBContext _context;

        public UsersController(ApplicationDBContext context)
        {
            _context = context;
        }

        public IActionResult Dashboard()
        {
            return View();
        }

        public IActionResult Index()
        {
            try
            {
                List<UserInfo> users = _context.users.ToList();
                return View(users);
            }
            catch (Exception e)
            {
                TempData["errorMessage"] = e;
                return View();
            }
        }

        public IActionResult Create()
        {
            UserInfo us = new UserInfo();
            var highestAdminNumber = _context.users.ToList()
                           .Where(entity => entity.UserCode.StartsWith("Emp"))
                             .Select(entity =>
                             {
                                 if (int.TryParse(entity.UserCode.Substring(4), out int number))
                                     return number;
                                 return 0;
                             })
                             .DefaultIfEmpty(0)
                             .Max();


            highestAdminNumber++;
            us.UserCode = "Emp0" + highestAdminNumber;
            us.UserRole = "Emp";
            return View(us);
        }

        [HttpPost]
        public IActionResult Create(UserInfo user)
        {
            try
            {
                user.UserPassword = user.FirstName+ "@123#";
                _context.users.Add(user);
                _context.SaveChanges();
                TempData["Success"] = "Employee Added Successfully";
                return RedirectToAction("Index");

            }
            catch (Exception e)
            {
                TempData["errorMessage"] = e;
                return View();
            }
        }

        public IActionResult Edit(int? id)
        {
            try
            {
                if (id == null || id == 0)
                {
                    return NotFound();
                }

                UserInfo? obj = _context.users.Where(u => u.UserId == id).SingleOrDefault();

                if (obj == null)
                {
                    return NotFound();
                }

                return View(obj);
            }
            catch (Exception e)
            {
                TempData["errorMessage"] = e;
                return View();
            }

        }
        [HttpPost]
        public IActionResult Edit(UserInfo obj)
        {
            try
            {
                var oldobj = _context.users.Find(obj.UserId);
                _context.users.Remove(oldobj);


                _context.users.Update(obj);
                _context.SaveChanges();
                TempData["Success"] = "Employee Updated Successfully";
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                TempData["errorMessage"] = e;
                return View();
            }

        }


        public IActionResult Delete(int? id)
        {
            try
            {
                if (id == null || id == 0)
                {
                    return NotFound();
                }
                UserInfo? obj = _context.users.Where(u => u.UserId == id).SingleOrDefault();
                if (obj == null)
                {
                    return NotFound();
                }
                return View(obj);
            }
            catch (Exception e)
            {
                TempData["errorMessage"] = e;
                return View();
            }

        }
        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePost(int? id)
        {
            try
            {
                UserInfo? obj = _context.users.Where(u => u.UserId == id).SingleOrDefault();
                if (obj == null)
                {
                    return NotFound();
                }
                _context.users.Remove(obj);
                _context.SaveChanges();
                TempData["Success"] = "Employee Deleted Successfully";
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                TempData["errorMessage"] = e;
                return View();
            }

        }
    }
}
