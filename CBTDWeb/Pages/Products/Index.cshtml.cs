using DataAccess;
using Infrastructure.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CBTDWeb.Pages.Products
{
    public class IndexlModel : PageModel
    {
        /* Puposely commented out Datatables will handle this with API call and JS
        private readonly UnitOfWork _unitOfWork;
        public IEnumerable<Product> objProductList;
        public IndexlModel(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            objProductList = new List<Product>();
        }
        public IActionResult OnGet()
        {
            objProductList = _unitOfWork.Product.GetAll();
            return Page();
        }
        */
    }
}
