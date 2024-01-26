using DataAccess;
using Infrastructure.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CBTDWeb.Pages.Manufacturers
{
    public class UpsertModel : PageModel
    {
        private readonly UnitOfWork _unitOfWork;
        [BindProperty]
        public Manufacturer objManufacturer { get; set; }  
        public UpsertModel(UnitOfWork unitOfWork) 
        {
            _unitOfWork = unitOfWork;
            objManufacturer = new Manufacturer();  
        }
        public IActionResult OnGet(int? id)
        {
            //am I in edit mode?
            if (id != 0)
            {
                objManufacturer = _unitOfWork.Manufacturer.GetById(id); 
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
                _unitOfWork.Manufacturer.Add(objManufacturer); //not saved yet
                TempData["success"] = "Category added Successfully";
            }
            //if exsits
            else
            {
                _unitOfWork.Manufacturer.Update(objManufacturer);
                TempData["success"] = "Category updated Successfully";
            }
            _unitOfWork.Commit();   
            return RedirectToPage("./Index");
        }
    }
}
