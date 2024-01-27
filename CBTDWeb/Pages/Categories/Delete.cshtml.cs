using DataAccess;
using Infrastructure.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CBTDWeb.Pages.Categories
{
    public class DeleteModel : PageModel
    {
        private readonly UnitOfWork _unitOfWork;
        [BindProperty]
        public Category objCategory { get; set; }  
        public DeleteModel(UnitOfWork unitOfWork) 
        {
            _unitOfWork = unitOfWork;   
            objCategory = new Category();  
        }
        public IActionResult OnGet(int? id)
        {
            //am I in edit mode?
            if (id != 0)
            {
                objCategory = _unitOfWork.Category.GetById(id); 
            }
            if(objCategory == null)
            {
                return NotFound();
            }
            return Page();  
        }
        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                TempData["error"] = "Data Error";
                return Page();
            }
            else
            {
                _unitOfWork.Category.Delete(objCategory);
                TempData["success"] = "Category  Successfully Delete";
            }
            _unitOfWork.Commit();
            return RedirectToPage("./Index");
        }
    }
}
