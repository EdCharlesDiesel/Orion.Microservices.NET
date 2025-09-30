using Microsoft.AspNetCore.Mvc;
using Orion.DataAccess.Postgres.Entities;
using Orion.DataAccess.Postgres.Tools;
using Orion.Domain.Tools;

namespace Orion.API.Person.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PersonPhoneController(IUnitOfWork unitOfWork) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var versions = await unitOfWork.PersonPhones.GetAllAsync();
            return Ok(versions);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var version = await unitOfWork.PersonPhones.GetByIdAsync(id);
            if (version == null) return NotFound();
            return Ok(version);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] DataAccess.Postgres.Entities.PersonPhone version)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await unitOfWork.PersonPhones.AddAsync(version);
                await unitOfWork.CompleteAsync();

                return CreatedAtAction(nameof(GetById), new { id = version.BusinessEntityID }, version);
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
        public async Task<IActionResult> Update(int id, [FromBody] DataAccess.Postgres.Entities.PersonPhone version)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var existing = await unitOfWork.PersonPhones.GetByIdAsync(id);
                if (existing == null)
                {
                    return NotFound($"Record with ID {id} not found.");
                }

                // Map fields (manual or via AutoMapper)
                // existing.PersonPhoneLine1 = version.PersonPhoneLine1;
                // existing.PersonPhoneLine2 = version.PersonPhoneLine2;
                // existing.City = version.City;
                // existing.StateProvinceID = version.StateProvinceID;
                // existing.PostalCode = version.PostalCode;
                // existing.SpatialLocation = version.SpatialLocation;
                // existing.rowguid = version.rowguid;
                existing.ModifiedDate = version.ModifiedDate;

                unitOfWork.PersonPhones.Update(existing);
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
                var existing = await unitOfWork.PersonPhones.GetByIdAsync(id);
                if (existing == null)
                {
                    return NotFound($"Record with ID {id} not found.");
                }

                unitOfWork.PersonPhones.Delete(existing);
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