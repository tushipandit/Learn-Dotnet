using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using EmployeeManagement.Models;
using EmployeeManagement.ViewModels;

namespace EmployeeManagement.Controllers
{
    
  //  [Route("/home")]
   //[Route("[Controller]")]
  public class HomeController : Controller 
    {

        public IemployeeRepo _employeeRepo;

        public HomeController(IemployeeRepo employeerepo)
        {
            _employeeRepo = employeerepo;
        }
        
      //  [Route("")]
      // [Route("[action]")]
       // [Route("/index")]
       // [Route("~/")]
        public ViewResult Index()
        {

            var model = _employeeRepo.GetAllEmployee();
                // String n= _employeeRepo.getemployee(1).Name;
                // return _employeeRepo.getemployee(1).Name;
                // Console.WriteLine(n);
                return View(model);
            
        }
    
      //  [Route("/details/{id}")]
      // [Route("[action]/id?")]
        public ViewResult Details(int? id)
        {     
            HomeDetailsViewModel homeDetailsViewModel = new HomeDetailsViewModel()
            {
                employee = _employeeRepo.getemployee(id??1),
                PageTitle = "Employee Details"
                    
            };
          
         
          //  ViewData["PageTitle"] = "employee details";
         //   Console.Write(model); 
            return View("Test",homeDetailsViewModel);

        }

        [HttpGet]
        public ViewResult Create()
        {
            return View("createview");
        }
        
        
        [HttpPost]
        public IActionResult Create(employee emp)
        {
            if (ModelState.IsValid)
            {
                employee newemployee = _employeeRepo.Addemp(emp);
               // return RedirectToAction("Details", newemployee.Id);
                
            }
            return View("Test");

        }

    }
}