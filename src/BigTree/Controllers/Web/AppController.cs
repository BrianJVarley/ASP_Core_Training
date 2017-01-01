using Microsoft.AspNetCore.Mvc;
using BigTree.ViewModels;
using BigTree.Services;
using Microsoft.Extensions.Configuration;
using BigTree.Models;
using System.Linq;
using Microsoft.Extensions.Logging;
using System;
using Microsoft.AspNetCore.Authorization;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace BigTree.Controllers.Web
{
    public class AppController : Controller
    {
        private IMailService _mailService;

        private IConfigurationRoot _config;

        private IWorldRepository _repository;
        private ILogger _logger;




        public AppController(IMailService mailService, IConfigurationRoot config, IWorldRepository repository, ILogger<AppController> logger)
        {
            _mailService = mailService;
            _config = config;
            _repository = repository;
            _logger = logger;

        }

        // GET: /<controller>/
        public IActionResult Index()
        {

            return View();
        }



        // GET: /<controller>/
        [Authorize] //comment out for testing
        public IActionResult Trips()
        {

            try
            {
                var data = _repository.GetAllTrips();
                return View(data);

            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to get trips in Index page: {ex.Message}");
                return Redirect("/error");
            }

        }


        public IActionResult Contact()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Contact(ContactViewModel model)
        {
            if(model.Email.Contains("aol.com"))
            {
                ModelState.AddModelError("", "aol.com addresses not supported!");
            }

            if(ModelState.IsValid)
            {
                _mailService.SendMail(_config["MailSettings:ToAddress"], model.Email, "From BigTree", model.Message);
                ModelState.Clear();
                ViewBag.UserMessage = "Message Sent";
            }


            return View();
        }

        public IActionResult About()
        {
            return View();
        }
    }

}
