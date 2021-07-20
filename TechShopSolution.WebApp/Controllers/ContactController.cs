using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using TechShopSolution.ApiIntegration;
using TechShopSolution.ViewModels.Website.Contact;

namespace TechShopSolution.WebApp.Controllers
{
    public class ContactController : Controller
    {
        private readonly IContactApiClient _contactApiClient;
        public ContactController(IContactApiClient contactApiClient)
        {
            _contactApiClient = contactApiClient;
        }
        [Route("lien-he")]
        public async Task<IActionResult> Index()
        {
            var contact = await _contactApiClient.GetcontactInfos();
            if (TempData["result"] != null)
            {
                ViewBag.SuccessMsg = TempData["result"];
            }
            if (TempData["error"] != null)
            {
                ViewBag.ErrorMsg = TempData["error"];
            }
            return View(contact.ResultObject);
        }
        [HttpPost]
        public async Task<IActionResult> SendFeedback(FeedbackCreateRequest request)
        {
            var contact = await _contactApiClient.GetcontactInfos();
            var result = await _contactApiClient.SendFeedback(request);
            if (!result.IsSuccess)
            {
                TempData["error"] = result.Message;
                return RedirectToAction("Index");
            }
            string contentMailClient = System.IO.File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\mail-template", "FeedbackSendCustomer.html"));
            string contentMailAdmin = System.IO.File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\mail-template", "FeedbackSendAdmin.html"));
            contentMailClient = contentMailClient.Replace("{{cus_name}}", request.name);
            contentMailAdmin = contentMailAdmin.Replace("{{cus_name}}", request.name);
            contentMailAdmin = contentMailAdmin.Replace("{{cus_phone}}", request.phone);
            contentMailAdmin = contentMailAdmin.Replace("{{cus_email}}", request.email);
            TempData["result"] = "Gửi phản feedback thành công, chúng tôi sẽ tiếp nhận sớm nhất có thể.";
            await SendMail(request.email, "Gửi phản hồi thành công", contentMailClient);
            await SendMail( "thuanneuwu2@gmail.com", "Bạn có một phản hồi mới", contentMailAdmin);

            return RedirectToAction("Index");
        }
        public async Task SendMail(string _to, string _subject, string _body)
        {
            MailMessage message = new MailMessage();
            message.BodyEncoding = System.Text.Encoding.UTF8;
            message.SubjectEncoding = System.Text.Encoding.UTF8;
            message.IsBodyHtml = true;
            message.From = new MailAddress("Techshop Việt Nam <admin@techshopvn.xyz>");
            message.To.Add(new MailAddress(_to));
            message.Subject = _subject;
            message.Body = _body;

            using var smtpClient = new SmtpClient("mail.techshopvn.xyz", 587);
            smtpClient.EnableSsl = false;
            smtpClient.Credentials = new NetworkCredential("admin@techshopvn.xyz", "THANHTHUAn123");

            await smtpClient.SendMailAsync(message);
        }
    }
}
