using Microsoft.AspNetCore.Mvc;
using Orion.DataAccess.Postgres.Entities;
using Orion.DataAccess.Postgres.Tools;
using Orion.Domain.Tools;

namespace Orion.API.Dashboard.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController: ControllerBase
    {
        private readonly IDashboardService _dashboardService;
        private readonly IUnitOfWork _unitOfWork;
        public DashboardController(IUnitOfWork unitOfWork,IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
            _unitOfWork = unitOfWork;
        }
        private string GetCurrentUserId()
        {
            return User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value ?? string.Empty;
        }
        
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var versions = await _unitOfWork.Departments.GetAllAsync();
            return Ok(versions);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var version = await _unitOfWork.Departments.GetByIdAsync(id);
            if (version == null) return NotFound();
            return Ok(version);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] DataAccess.Postgres.Entities.Department version)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _unitOfWork.Departments.AddAsync(version);
                await _unitOfWork.CompleteAsync();

                return CreatedAtAction(nameof(GetById), new { id = version.DepartmentID }, version);
            }
            catch (Exception exception)
            {
                try
                {
                    var error = new ErrorLog()
                    {
                        ErrorTime = DateTimeOffset.UtcNow,
                        UserName = HttpContext?.User?.Identity?.Name ?? "System",
                        ErrorMessage = exception.Message,
                        ErrorNumber = exception.HResult,
                        ErrorProcedure = exception.TargetSite?.Name,
                        ErrorLine = StaticTools.TryGetErrorLine(exception)
                    };
                    await _unitOfWork.SaveErrorsAsync(error);
                }
                catch { /* prevent secondary failure */ }

                await _unitOfWork.RollbackAsync();
                return StatusCode(500, "An internal error occurred while processing your request.");
            }
        }
        
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] DataAccess.Postgres.Entities.Department version)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var existing = await _unitOfWork.Departments.GetByIdAsync(id);
                if (existing == null)
                {
                    return NotFound($"Record with ID {id} not found.");
                }

                // Map fields (manual or via AutoMapper)
                existing.Name = version.Name;
                existing.GroupName = version.GroupName;
                existing.EmployeeDepartmentHistories = new List<EmployeeDepartmentHistory>();
                existing.ModifiedDate = version.ModifiedDate;

                _unitOfWork.Departments.Update(existing);
                await _unitOfWork.CompleteAsync();

                return Ok(existing);
            }
            catch (Exception exception)
            {
                try
                {
                    var error = new ErrorLog()
                    {
                        ErrorTime = DateTimeOffset.UtcNow,
                        UserName = HttpContext?.User?.Identity?.Name ?? "System",
                        ErrorMessage = exception.Message,
                        ErrorNumber = exception.HResult,
                        ErrorProcedure = exception.TargetSite?.Name,
                        ErrorLine = StaticTools.TryGetErrorLine(exception)
                    };

                    await _unitOfWork.SaveErrorsAsync(error);
                }
                catch { }

                await _unitOfWork.RollbackAsync();
                return StatusCode(500, "An internal error occurred while updating the record.");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var existing = await _unitOfWork.Departments.GetByIdAsync(id);
                if (existing == null)
                {
                    return NotFound($"Record with ID {id} not found.");
                }

                _unitOfWork.Departments.Delete(existing);
                await _unitOfWork.CompleteAsync();

                return NoContent();
            }
            catch (Exception exception)
            {

                var error = new ErrorLog()
                {
                    ErrorLogID = 22,
                    ErrorTime = DateTimeOffset.UtcNow,
                    UserName = HttpContext?.User?.Identity?.Name ?? "System",
                    ErrorMessage = exception.Message,
                    ErrorNumber = exception.HResult,
                    ErrorProcedure = exception.TargetSite?.Name,
                    ErrorLine = StaticTools.TryGetErrorLine(exception)
                };

                await _unitOfWork.SaveErrorsAsync(error);
                await _unitOfWork.RollbackAsync();
                return StatusCode(500, "An internal error occurred while deleting the record.");
            }
        }


        [HttpGet("stats")]
        public async Task<IActionResult> GetStats()
        {
            var stats = await _dashboardService.GetDashboardStatsAsync(GetCurrentUserId());
            return Ok(stats);
        }

        [HttpGet("items")]
        public async Task<IActionResult> GetItems([FromQuery] string? category = null, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var items = await _dashboardService.GetDashboardItemsAsync(GetCurrentUserId(), category, page, pageSize);
            return Ok(items);
        }

        [HttpGet("items/{id}")]
        public async Task<IActionResult> GetItem(int id)
        {
            var item = await _dashboardService.GetDashboardItemAsync(id, GetCurrentUserId());
            if (item == null)
                return NotFound();

            return Ok(item);
        }

        [HttpPost("items")]
        public async Task<IActionResult> CreateItem([FromBody] CreateDashboardItemRequest request)
        {
            var item = await _dashboardService.CreateDashboardItemAsync(request, GetCurrentUserId());
            return CreatedAtAction(nameof(GetItem), new { id = item.Id }, item);
        }

        [HttpPut("items/{id}")]
        public async Task<IActionResult> UpdateItem(int id, [FromBody] UpdateDashboardItemRequest request)
        {
            var item = await _dashboardService.UpdateDashboardItemAsync(id, request, GetCurrentUserId());
            if (item == null)
                return NotFound();

            return Ok(item);
        }

        [HttpDelete("items/{id}")]
        public async Task<IActionResult> DeleteItem(int id)
        {
            var result = await _dashboardService.DeleteDashboardItemAsync(id, GetCurrentUserId());
            if (!result)
                return NotFound();

            return NoContent();
        }
    }
}