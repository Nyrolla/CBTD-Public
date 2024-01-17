using DataAccess;
using Infrastructure.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CBTDWeb.Pages.Manufacturers
{
    public class DeleteModel : PageModel
    {
        private readonly AppicationDbContext _db;
        [BindProperty]
        public Manufacturer objManufacturer { get; set; }  
        public DeleteModel(AppicationDbContext db) 
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
            if (!ModelState.IsValid)
            {
                TempData["error"] = "Data Error";
                return Page();
            }
            else
            {
                _db.Manufacturers.Remove(objManufacturer);
                TempData["success"] = "Manufacturer Successfully Delete";
            }
            _db.SaveChanges();
            return RedirectToPage("./Index");
        }
    }
}
