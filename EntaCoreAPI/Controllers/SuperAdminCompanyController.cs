using Application.DTOs.SuperAdmin;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EntaCoreAPI.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/super-admin/company")]
    public class SuperAdminCompanyController : Controller
    {
        private readonly ISuperAdminCompanyService _service;

        public SuperAdminCompanyController(ISuperAdminCompanyService service)
        {
            _service = service;
        }

        [HttpPost("get-company-list")]
        public async Task<IActionResult> GetCompanyList([FromHeader] string Authorization, [FromBody] CompanyFilters filter)
        {
            var result =await _service.GetCompanyListAsync(filter);
            return Ok(result);
        }

        [HttpPost("comapany-ins-updt")]
        public async Task<IActionResult> CompanyInsUpdt([FromHeader] string Authorization, [FromBody] CompanyInsUpdtRequest request)
        {
            var result = request.CompanyID == 0 ? _service.CreateCompanyAsync(request) : _service.UpdateCompanyAsync(request);
            return Ok(result);
        }
    }
}
