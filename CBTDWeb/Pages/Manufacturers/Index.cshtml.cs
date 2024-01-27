using DataAccess;
using Infrastructure.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CBTDWeb.Pages.Manufacturers
{
    public class IndexModel : PageModel
    {
         
        private readonly UnitOfWork _unitOfWork;
        //our UI front end to support looping through  serval category object
        public IEnumerable<Manufacturer> objManufacturerList;

        public IndexModel(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            objManufacturerList = new List<Manufacturer>();
        }
        public IActionResult OnGet()
            //IActionResult returns 
            //1. Sever status code results
            //2. #1 and object results
            //3. redirection to anotehr razor page
            //4. file results 
            //5. contect results - a razor page
        {
            objManufacturerList = _unitOfWork.Manufacturer.GetAll();
            return Page();  
        }
    }
}
