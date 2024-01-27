using DataAccess;
using Infrastructure.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace CBTDWeb.Pages.Products
{
    public class UpsertModel : PageModel
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly UnitOfWork _unitOfWork;
        [BindProperty]
        public Product objProduct { get; set; }
        public IEnumerable<SelectListItem> CategoryList { get; set; }
        public IEnumerable<SelectListItem> ManufacturerList { get; set; }

        public UpsertModel(UnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            objProduct = new Product();
            CategoryList = new List<SelectListItem>();
            ManufacturerList = new List<SelectListItem>();
            _webHostEnvironment = webHostEnvironment;

        }
        public IActionResult OnGet(int? id)
        {
            //populate our select list
            CategoryList = _unitOfWork.Category.GetAll()
                .Select(c => new SelectListItem
                {
                    Text = c.Name,
                    Value = c.Id.ToString()
                });
            ManufacturerList = _unitOfWork.Manufacturer.GetAll()
                .Select(c => new SelectListItem
                {
                    Text = c.Name,
                    Value = c.Id.ToString()
                });
            //Are we in create mode
            if(id == null || id == 0)
            {
                return Page();
            }
            //edit mode

            if(id!=0) //retrieve from DB
            {
                objProduct = _unitOfWork.Product.GetById(id);
            }
            if (objProduct == null) //Maybe nothing returned from DB
            {
                return NotFound();
            }
            return Page();
        }
        public IActionResult OnPost(int? id) 
        {
            //determine root path
            string webRootPath = _webHostEnvironment.WebRootPath;
            //Retreive the files 
            var files = HttpContext.Request.Form.Files;

            //if the progect is new (create)
            if (objProduct.Id == 0)
            {
                //was there an image uploaded?
                if (files.Count > 0)
                {
                    //create an unique identifier for image name
                    string fileName = Guid.NewGuid().ToString();

                    //create variable to hold a path to images/products folder
                    var uploads = Path.Combine(webRootPath, @"images\products\");

                    //get and preserve the extension type
                    var extension = Path.GetExtension(files[0].FileName);

                    //create the full path of the item to stream
                    var fullPath = uploads + fileName + extension;

                    //Stream the binary files to server
                    using var fileStream = System.IO.File.Create(fullPath);
                    files[0].CopyTo(fileStream);

                    //associate the actual real URL path and save to DB
                    objProduct.ImageURL = @"\images\products\" + fileName + extension;
                }
                //add this new product internally
                _unitOfWork.Product.Add(objProduct);
            }
            //the item exists already, so we're updating it
            else
            {
                /*
                 * get the product again from the DB because binding is on, and we 
                 * need to process the phyical iamge separately from the binded
                 * property holding the URL string
                 */
                var objProductFromDb = _unitOfWork.Product.Get(p => p.Id == objProduct.Id);
                //Was there an image uplaoded
                if (files.Count > 0)
                {
                    //create an unique identifier for image name
                    string fileName = Guid.NewGuid().ToString();

                    //create variable to hold a path to images/products folder
                    var uploads = Path.Combine(webRootPath, @"images\products\");

                    //get and preserve the extension type
                    var extension = Path.GetExtension(files[0].FileName);

                    if(objProductFromDb.ImageURL != null)
                    {
                        var imagePath = Path.Combine(webRootPath,objProduct.ImageURL.TrimStart('\\'));

                        //if image exist physically
                        if (System.IO.File.Exists(imagePath))
                        {
                            System.IO.File.Delete(imagePath);
                        }
                    }
                    //create the full path of the item to stream
                    var fullPath = uploads + fileName + extension;

                    //Stream the binary files to server
                    using var fileStream = System.IO.File.Create(fullPath);
                    files[0].CopyTo(fileStream);

                    //associate the actual real URL path and save to DB
                    objProduct.ImageURL = @"\images\products\" + fileName + extension;

                }
                else //We're trying to add image for the 1st time
                {
                    objProductFromDb.ImageURL = objProduct.ImageURL;

                }
                // Detach the entity from the DbContext using the UnitOfWork
                _unitOfWork.DetachEntity(objProductFromDb);

                //Update the existing product
                _unitOfWork.Product.Update(objProduct);
            }
            //saving to database
            _unitOfWork.Commit();

            //redirect to another page
            return RedirectToPage("./Index");
        }
    }
}
