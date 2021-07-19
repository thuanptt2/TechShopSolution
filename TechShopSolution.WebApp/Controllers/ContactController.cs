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
            contentMailAdmin = contentMailClient.Replace("{{cus_name}}", request.name);
            contentMailAdmin = contentMailClient.Replace("{{cus_phone}}", request.phone);
            contentMailAdmin = contentMailClient.Replace("{{cus_email}}", request.email);
            TempData["result"] = "Gửi phản feedback thành công, chúng tôi sẽ tiếp nhận sớm nhất có thể.";
            await SendMail("thuanneuwu2@gmail.com", request.email, "Gửi phản hồi thành công", contentMailClient, "thuanneuwu2@gmail.com", "thanhthuan123");
            await SendMail("thuanneuwu2@gmail.com", "thuanneuwu2@gmail.com", "Bạn có một phản hồi mới", contentMailAdmin, "thuanneuwu2@gmail.com", "thanhthuan123");

            return RedirectToAction("Index");
        }
        public async Task SendMail(string _from, string _to, string _subject, string _body, string _gmail, string _password)
        {
            MailMessage message = new MailMessage(_from, _to, _subject, _body);
            message.BodyEncoding = System.Text.Encoding.UTF8;
            message.SubjectEncoding = System.Text.Encoding.UTF8;
            message.IsBodyHtml = true;

            message.ReplyToList.Add(new MailAddress(_from));
            message.Sender = new MailAddress(_from);

            using var smtpClient = new SmtpClient("smtp.gmail.com");
            smtpClient.Port = 587;
            smtpClient.EnableSsl = true;
            smtpClient.Credentials = new NetworkCredential(_gmail, _password);

            await smtpClient.SendMailAsync(message);
        }
    }
}
