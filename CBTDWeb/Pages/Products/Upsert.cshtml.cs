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
            // determine root path
            string webRootPath = _webHostEnvironment.WebRootPath;

            // retrieve the files
            var files = HttpContext.Request.Form.Files;

            // if the product is new (create)
            if (objProduct.Id == 0)
            {
                // validate the 'Description' before saving
                if (string.IsNullOrEmpty(objProduct.Description))
                {
                    ModelState.AddModelError("objProduct.Description", "Please enter a product description.");
                    // handle the validation error, e.g., return to the page with errors
                    return Page();
                }

                // was there an image uploaded?
                if (files.Count > 0)
                {
                    // create a unique identifier for the image name
                    string fileName = Guid.NewGuid().ToString();

                    // create variable to hold a path to images/products folder
                    var uploads = Path.Combine(webRootPath, @"images\products\");

                    // get and preserve the extension type
                    var extension = Path.GetExtension(files[0].FileName);

                    // create the full path of the item to stream
                    var fullPath = Path.Combine(uploads, fileName + extension);

                    // stream the binary files to server
                    using var fileStream = System.IO.File.Create(fullPath);
                    files[0].CopyTo(fileStream);

                    // associate the actual real URL path and save to DB
                    objProduct.ImageURL = @"\images\products\" + fileName + extension;
                }

                // add this new product internally
                _unitOfWork.Product.Add(objProduct);
            }
            // the item exists already, so we're updating it
            else
            {
                // get the product again from the DB because binding is on,
                // and we need to process the physical image separately from the
                // bound property holding the URL string
                var objProductFromDb = _unitOfWork.Product.Get(p => p.Id == objProduct.Id);

                // validate the 'Description' before updating
                if (string.IsNullOrEmpty(objProduct.Description))
                {
                    ModelState.AddModelError("objProduct.Description", "Please enter a product description.");
                    // handle the validation error, e.g., return to the page with errors
                    return Page();
                }

                // was there an image uploaded
                if (files.Count > 0)
                {
                    // create a unique identifier for the image name
                    string fileName = Guid.NewGuid().ToString();

                    // create variable to hold a path to images/products folder
                    var uploads = Path.Combine(webRootPath, @"images\products\");

                    // get and preserve the extension type
                    var extension = Path.GetExtension(files[0].FileName);

                    if (objProductFromDb.ImageURL != null)
                    {
                        var imagePath = Path.Combine(webRootPath, objProduct.ImageURL.TrimStart('\\'));

                        // if image exists physically
                        if (System.IO.File.Exists(imagePath))
                        {
                            System.IO.File.Delete(imagePath);
                        }
                    }
                    // create the full path of the item to stream
                    var fullPath = Path.Combine(uploads, fileName + extension);

                    // stream the binary files to server
                    using var fileStream = System.IO.File.Create(fullPath);
                    files[0].CopyTo(fileStream);

                    // associate the actual real URL path and save to DB
                    objProduct.ImageURL = @"\images\products\" + fileName + extension;
                }
                else // we're trying to add an image for the 1st time
                {
                    // Update only if a new image is selected
                    if (!string.IsNullOrEmpty(objProduct.ImageURL))
                    {
                        objProductFromDb.ImageURL = objProduct.ImageURL;
                    }
                }

                // detach the entity from the DbContext using the UnitOfWork
                _unitOfWork.DetachEntity(objProductFromDb);

                // update the existing product
                _unitOfWork.Product.Update(objProductFromDb);
            }

            // saving to database
            _unitOfWork.Commit();

            // redirect to another page
            return RedirectToPage("./Index");
        }
    }
}
