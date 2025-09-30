using Microsoft.AspNetCore.Mvc;
using Orion.DataAccess.Postgres.Entities;
using Orion.DataAccess.Postgres.Tools;
using Orion.Domain.Tools;

namespace Orion.API.Sales.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class SalesOrderHeaderController(IUnitOfWork unitOfWork) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var versions = await unitOfWork.SalesOrderHeaders.GetAllAsync();
            return Ok(versions);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var version = await unitOfWork.SalesOrderHeaders.GetByIdAsync(id);
            if (version == null) return NotFound();
            return Ok(version);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] DataAccess.Postgres.Entities.SalesOrderHeader version)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await unitOfWork.SalesOrderHeaders.AddAsync(version);
                await unitOfWork.CompleteAsync();

                return CreatedAtAction(nameof(GetById), new { id = version.SalesOrderID }, version);
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
                    await unitOfWork.SaveErrorsAsync(error);
                }
                catch { /* prevent secondary failure */ }

                await unitOfWork.RollbackAsync();
                return StatusCode(500, "An internal error occurred while processing your request.");
            }
        }
        
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] DataAccess.Postgres.Entities.SalesOrderHeader version)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var existing = await unitOfWork.SalesOrderHeaders.GetByIdAsync(id);
                if (existing == null)
                {
                    return NotFound($"Record with ID {id} not found.");
                }

                // Map fields (manual or via AutoMapper)
                // existing.Name = version.Name;
                // existing.GroupName = version.GroupName;
                // existing.EmployeeSalesOrderHeaderHistories = new List<EmployeeSalesOrderHeaderHistory>();
                existing.ModifiedDate = version.ModifiedDate;

                unitOfWork.SalesOrderHeaders.Update(existing);
                await unitOfWork.CompleteAsync();

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

                    await unitOfWork.SaveErrorsAsync(error);
                }
                catch { }

                await unitOfWork.RollbackAsync();
                return StatusCode(500, "An internal error occurred while updating the record.");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var existing = await unitOfWork.SalesOrderHeaders.GetByIdAsync(id);
                if (existing == null)
                {
                    return NotFound($"Record with ID {id} not found.");
                }

                unitOfWork.SalesOrderHeaders.Delete(existing);
                await unitOfWork.CompleteAsync();

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

                await unitOfWork.SaveErrorsAsync(error);
                await unitOfWork.RollbackAsync();
                return StatusCode(500, "An internal error occurred while deleting the record.");
            }
        }

        
        
    }
}