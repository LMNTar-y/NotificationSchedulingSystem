using Microsoft.AspNetCore.Mvc;
using NotificationSchedulingSystem.Business.Models;
using NotificationSchedulingSystem.Business.Services;

namespace NotificationSchedulingSystem.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CompanyController : ControllerBase
{
    private readonly ICompanyService _companyService;

    public CompanyController(ICompanyService companyService)
    {
        _companyService = companyService ??
                          throw new ArgumentException(
                              $"{GetType().Name} Initialization failure due to: {nameof(companyService)}");
    }

    [HttpGet]
    public async Task<ActionResult> GetAllCountries()
    {
        try
        {
            var result = await _companyService.GetAllAsync();
            return Ok(new ResponseWrapper<IEnumerable<CompanyResponse>>()
            {
                Result = result
            });
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status400BadRequest, new ResponseWrapper<object>
            {
                Errors = new List<Error>()
                {
                    new() { Message = ex.Message }
                }
            });
        }
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult> GetById(Guid id)
    {
        try
        {
            var result = await _companyService.GetScheduleByCompanyId(id);
            return Ok(new ResponseWrapper<CompanyResponse>()
            {
                Result = result
            });
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status400BadRequest, new ResponseWrapper<object>
            {
                Errors = new List<Error>()
                {
                    new() { Message = ex.Message }
                }
            });
        }
    }

    [HttpPost()]
    public async Task<ActionResult> AddCompany([FromBody] CompanyRequest newCompany)
    {
        try
        {
            var result = await _companyService.AddAsync(newCompany);
            return StatusCode(StatusCodes.Status201Created, new ResponseWrapper<object>()
            {
                Result = result
            });
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status400BadRequest, new ResponseWrapper<object>
            {
                Errors = new List<Error>()
                {
                    new() { Message = ex.Message }
                }
            });
        }
    }
}