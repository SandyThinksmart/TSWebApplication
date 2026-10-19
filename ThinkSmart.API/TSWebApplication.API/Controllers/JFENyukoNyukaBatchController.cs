using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TSWebApplication.Application.IServices;

namespace TSWebApplication.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JFENyukoNyukaBatchController : ControllerBase
    {
        private readonly IJFENyukoNyukaBatchService _jFENyukoNyukaBatchService;

        public JFENyukoNyukaBatchController(IJFENyukoNyukaBatchService jFENyukoNyukaBatchService)
        {
            _jFENyukoNyukaBatchService = jFENyukoNyukaBatchService;
        }

        [HttpPost("ReadCSVData")]
        public async Task<IActionResult> ReadCSVData(IFormFile file)
        {
            try
            {
                var result = await _jFENyukoNyukaBatchService.ReadCsvData(file);

                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
