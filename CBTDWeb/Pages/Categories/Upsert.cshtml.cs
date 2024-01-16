using DataAccess;
using Infrastructure.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CBTDWeb.Pages.Categories
{
    public class UpsertModel : PageModel
    {
        private readonly AppicationDbContext _db;
        [BindProperty]
        public Category objCategory { get; set; }  
        public UpsertModel(AppicationDbContext db) 
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
            if(!ModelState.IsValid)
            {
                TempData["error"] = "Data Incomplete";
                return Page();
            }
            //if this is a new category
            if (objCategory.Id == 0)
            {
                _db.Categories.Add(objCategory); //not saved yet
                TempData["success"] = "Category added Successfully";
            }
            //if exsits
            else
            {
                _db.Categories.Update(objCategory);
                TempData["success"] = "Category updated Successfully";
            }
            _db.SaveChanges();
            return RedirectToPage("./Index");
        }
    }
}
