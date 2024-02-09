using DataAccess;
using Infrastructure.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CBTDWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : Controller
    {
        private readonly UnitOfWork _unitOfWork;
        public ProductController(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        [HttpGet]
        public ActionResult Get() {
            return Json(new { data = _unitOfWork.Product.GetAll(null, null, "Category,Manufacturer") });
        }
    }
}
