using DataAccess;
using Infrastructure.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CBTDWeb.Pages.Categories
{
    public class IndexModel : PageModel
    {
        //local instance of Unit Of Work Service
        private readonly UnitOfWork _unitOfWork;
        //our UI front end to support looping through  serval category object
        public IEnumerable<Category> objCategoryList;

        public IndexModel(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;  
            objCategoryList = new List<Category>();
        }
        public IActionResult OnGet()
            //IActionResult returns 
            //1. Sever status code results
            //2. #1 and object results
            //3. redirection to anotehr razor page
            //4. file results 
            //5. contect results - a razor page
        {
            objCategoryList = _unitOfWork.Category.GetAll();
            return Page();  
        }
    }
}
