using Microsoft.AspNetCore.Mvc;
using BigTree.ViewModels;
using BigTree.Services;
using Microsoft.Extensions.Configuration;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace BigTree.Controllers.Web
{
    public class AppController : Controller
    {
        private IMailService _mailService;

        public IConfigurationRoot _config;

        public AppController(IMailService mailService, IConfigurationRoot config)
        {
            _mailService = mailService;
            _config = config;
        }

        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
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
                _mailService.SendMail(_config["MailSetting:ToAddress"], model.Email, "From BigTree", model.Message);
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
