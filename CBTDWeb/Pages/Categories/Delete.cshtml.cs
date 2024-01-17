using DataAccess;
using Infrastructure.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CBTDWeb.Pages.Categories
{
    public class DeleteModel : PageModel
    {
        private readonly AppicationDbContext _db;
        [BindProperty]
        public Category objCategory { get; set; }  
        public DeleteModel(AppicationDbContext db) 
        {
            _db = db;   
            objCategory = new Category();  
        }
        public IActionResult OnGet(int? id)
        {
            //am I in edit mode?
            if (id != 0)
            {
                objCategory = _db.Categories.Find(id);
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
                _db.Categories.Remove(objCategory);
                TempData["success"] = "Category  Successfully Delete";
            }
            _db.SaveChanges();
            return RedirectToPage("./Index");
        }
    }
}
