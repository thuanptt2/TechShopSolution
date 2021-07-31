using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TechShopSolution.WebApp.Controllers
{
    public class ErrorController : Controller
    {
        [Route("Error/{statusCode}")]
        public IActionResult HttpStatusCodeHandler(int statusCode)
        {
            switch(statusCode)
            {
                case 401:
                    ViewBag.StatusCode = statusCode;
                    ViewBag.ErrorTitle = "Từ chối truy cập";
                    ViewBag.ErrorMsg = "Thông tin xác thực của quý khách không hợp lệ. Liên hệ với QTV để biết thêm thông tin";
                    break;
                case 403:
                    ViewBag.StatusCode = statusCode;
                    ViewBag.ErrorTitle = "Cấm truy cập";
                    ViewBag.ErrorMsg = "Oops, có vẻ quý khách đang truy cập vào nội dung mình không được phép xem";
                    break;
                case 404:
                    ViewBag.StatusCode = statusCode;
                    ViewBag.ErrorTitle = "Không tìm thấy";
                    ViewBag.ErrorMsg = "Chúng tôi rất tiếc trang bạn đang yêu cầu không được tìm thấy.";
                    break;
                case 408:
                    ViewBag.StatusCode = statusCode;
                    ViewBag.ErrorTitle = "Truy cập hết hạn";
                    ViewBag.ErrorMsg = "Máy chủ mất quá nhiều thời gian để xử lý yêu cầu. Vui lòng thử lại";
                    break;
                case 500:
                    ViewBag.StatusCode = statusCode;
                    ViewBag.ErrorTitle = "Lỗi từ máy chủ";
                    ViewBag.ErrorMsg = "Máy chủ đang gặp trục trặc, quý khách vui lòng thử lại sau.";
                    break;
                case 502:
                    ViewBag.StatusCode = statusCode;
                    ViewBag.ErrorTitle = "Lỗi từ máy chủ";
                    ViewBag.ErrorMsg = "Máy chủ cố truy cập đang gửi lại lỗi.";
                    break;
            }
            return View("Error");
        }
    }
}
