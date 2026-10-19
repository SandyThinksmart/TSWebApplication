using Microsoft.AspNetCore.Mvc;
using TSWebApplication.Application.IServices;
using TSWebApplication.Domain.Model;

namespace TSWebApplication.API.Controllers
{
    [Route("[Controller]")]
    [ApiController]
    public class MakerMaterialController : Controller
    {
        private readonly IMakerMaterialService _makerMaterialService;
        private readonly IAllCommonService _commonService;
      
        public MakerMaterialController(IMakerMaterialService makerMaterialService, IAllCommonService commonService)
        {

            _makerMaterialService = makerMaterialService;
            _commonService = commonService;
        }
        [HttpGet("GetMakerList")]
        public IActionResult GetMakerList()
        {
            var result = _makerMaterialService.GetMakersList();
            return Ok(new { IsSuccess = true, message = result });
        }
        [HttpPost("GetMukesakiList")]
        public IActionResult GetMukesakiList(string? yobina)
        {
            var result = _makerMaterialService.GetMukesakiList(yobina);
            return Ok(new { IsSuccess = true, message = result });
        }
        [HttpPost("GetMaterialDetails")]
        public IActionResult GetMaterialDetails([FromBody] MakerMaterialDetails details)
        {
            var result = _makerMaterialService.GetMaterialDetailsList(details);
            return Ok(new { IsSuccess = true, message = result });
        }
        [HttpPost("SaveMakerMaterialDetails")]
        public IActionResult SaveMakerMaterialDetails([FromBody] List<MakerMaterialDetails> details)
        {
           var result= _makerMaterialService.SaveMaterialDetails(details);
            return Ok(new { IsSuccess = true, message = result });
        }
        [HttpPost("UpdateMakerMaterialDetails")]
        public IActionResult UpdateMakerMaterialDetails([FromBody] List<MakerMaterialDetails> details)
        {
           var result= _makerMaterialService.UpdateMaterialDetails(details);
            return Ok(new { IsSuccess = true, message = result });
        }
        [HttpPost("DeleteMakerMaterialDetails")]
        public IActionResult DeleteMakerMaterialDetails(int groupNo)
        {
            _makerMaterialService.DeleteMaterialDetails(groupNo);
            return Ok(new { IsSuccess = true, message = "" });
        }
        
        [HttpPost("GetMakerMaterialListData")]
        public IActionResult GetMakerMaterialListData([FromBody] MakerMaterialDetails details)
        {
           var result= _makerMaterialService.GetListPageData(details);
            return Ok(new { IsSuccess = true, message = result });
        }
        [HttpPost("GetMaterialDetailsByGroupNo")]
        public IActionResult GetMaterialDetailsByGroupNo(int groupNo)
        {
            var result = _makerMaterialService.GetMaterialDetailsByGroupNo(groupNo);
            return Ok(new { IsSuccess = true, message = result });
        }
        
    }
}
