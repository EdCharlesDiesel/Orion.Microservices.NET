using Microsoft.AspNetCore.Mvc;
using Orion.DataAccess.Postgres.Tools;

namespace Orion.API.ErrorLog.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ErrorLogController(IUnitOfWork unitOfWork) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var versions = await unitOfWork.ErrorLogs.GetAllAsync();
            return Ok(versions);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var version = await unitOfWork.ErrorLogs.GetByIdAsync(id);
            if (version == null) return NotFound();
            return Ok(version);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] DataAccess.Postgres.Entities.ErrorLog version)
        {
            await unitOfWork.ErrorLogs.AddAsync(version);
            await unitOfWork.CompleteAsync();
            return CreatedAtAction(nameof(GetById), new { id = version.ErrorLogID }, version);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, DataAccess.Postgres.Entities.ErrorLog version)
        {
            var existing = await unitOfWork.ErrorLogs.GetByIdAsync(id);
            if (existing == null) return NotFound();

            existing.ErrorLine = version.ErrorLine;
            existing.ErrorMessage = version.ErrorMessage;
            existing.ErrorNumber = version.ErrorNumber;
            existing.ErrorProcedure = version.ErrorProcedure;
            existing.ErrorSeverity = version.ErrorSeverity;
            existing.ErrorState = version.ErrorState;
            existing.ErrorTime = version.ErrorTime;
            existing.UserName = version.UserName;

            unitOfWork.ErrorLogs.Update(existing);
            await unitOfWork.CompleteAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var existing = await unitOfWork.ErrorLogs.GetByIdAsync(id);
            if (existing == null) return NotFound();

            unitOfWork.ErrorLogs.Delete(existing);
            await unitOfWork.CompleteAsync();

            return NoContent();
        }
    }
}