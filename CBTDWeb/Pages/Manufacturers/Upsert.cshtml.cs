using DataAccess;
using Infrastructure.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CBTDWeb.Pages.Manufacturers
{
    public class UpsertModel : PageModel
    {
        private readonly AppicationDbContext _db;
        [BindProperty]
        public Manufacturer objManufacturer { get; set; }  
        public UpsertModel(AppicationDbContext db) 
        {
            _db = db;
            objManufacturer = new Manufacturer();  
        }
        public IActionResult OnGet(int? id)
        {
            //am I in edit mode?
            if (id != 0)
            {
                objManufacturer = _db.Manufacturers.Find(id);
            }
            if(objManufacturer == null)
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
            if (objManufacturer.Id == 0)
            {
                _db.Manufacturers.Add(objManufacturer); //not saved yet
                TempData["success"] = "Category added Successfully";
            }
            //if exsits
            else
            {
                _db.Manufacturers.Update(objManufacturer);
                TempData["success"] = "Category updated Successfully";
            }
            _db.SaveChanges();
            return RedirectToPage("./Index");
        }
    }
}
