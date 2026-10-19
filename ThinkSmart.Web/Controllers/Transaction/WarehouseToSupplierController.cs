using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using TSWebApplication.Application.IServices;
using TSWebApplication.Domain.Model;
using TSWebApplication.Domain.Resources;
using TSWebApplication.Web.Models;

namespace TSWebApplication.Web.Controllers.Transaction
{

    public class WarehouseToSupplierController : Controller
    {
        private readonly IWarehouseToSupplierService _IWarehouseToSupplierService;
        private readonly IViewRenderService _viewRenderService;

        public WarehouseToSupplierController(IViewRenderService viewRenderService,IWarehouseToSupplierService IWarehouseToSupplierService)
        {
            _IWarehouseToSupplierService = IWarehouseToSupplierService;
            _viewRenderService = viewRenderService;
        }

        public IActionResult TensouEntryEditDelete()
        {
            return View();
        }
        public IActionResult TensouListPage()
        {   
            List<MaterialTensouDetails> result = new List<MaterialTensouDetails>();
           //var result = _IWarehouseToSupplierService.GetDataOnGridView(request);
            return View(result);


        }
        //public IActionResult GridSearch([FromBody] List<MaterialTensouDetails> details)
        //{
        //    string view = _viewRenderService.RenderToString("WarehouseToSupplier/TensouGrid", details);
        //    return Json(new { IsSuccess = true, message = view });
        //}
        public IActionResult GridSearch([FromBody] List<MaterialTensouDetails> details)
        {
            string view = _viewRenderService.RenderToString("WarehouseToSupplier/TensouGrid1", details);
            return Json(new { IsSuccess = true, message = view });
        }
    }

    
}
