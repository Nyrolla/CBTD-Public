using DataAccess;
using Infrastructure.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CBTDWeb.Pages.Products
{
    public class DeleteModel : PageModel
    {
        private readonly UnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;

        [BindProperty]
        public Product objProduct { get; set; }

        public DeleteModel(UnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult OnGet(int? id)
        {
            objProduct = _unitOfWork.Product.GetById(id);

            if (objProduct == null)
            {
                return NotFound();
            }

            return Page();
        }

        public IActionResult OnPost(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Retrieve the product from the database using the provided id
            objProduct = _unitOfWork.Product.GetById(id);

            if (objProduct == null)
            {
                return NotFound();
            }

            // Proceed with deleting the product
            string webRootPath = _webHostEnvironment.WebRootPath;

            var oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, objProduct.ImageURL.TrimStart('\\'));

            if (System.IO.File.Exists(oldImagePath))
            {
                System.IO.File.Delete(oldImagePath);
            }

            _unitOfWork.Product.Delete(objProduct);
            TempData["success"] = "Product Deleted Successfully";
            _unitOfWork.Commit();

            return RedirectToPage("./Index");
        }
    }
}
