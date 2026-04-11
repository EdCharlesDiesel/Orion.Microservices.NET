using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Orion.DataAccess.Postgres.Tools;

namespace Orion.API.DatabaseLog.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class DatabaseLogController(IUnitOfWork unitOfWork) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var versions = await unitOfWork.DatabaseLogs.GetAllAsync();
            return Ok(versions);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var version = await unitOfWork.DatabaseLogs.GetByIdAsync(id);
            if (version == null) return NotFound();
            return Ok(version);
        }

        [HttpPost]
        public async Task<IActionResult> Create(DataAccess.Postgres.Entities.DatabaseLog version)
        {
            await unitOfWork.DatabaseLogs.AddAsync(version);
            await unitOfWork.CompleteAsync();
            return CreatedAtAction(nameof(GetById), new { id = version.DatabaseLogID }, version);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, DataAccess.Postgres.Entities.DatabaseLog version)
        {
            var existing = await unitOfWork.DatabaseLogs.GetByIdAsync(id);
            if (existing == null) return NotFound();

            existing.DatabaseUser = version.DatabaseUser;
            existing.PostTime = version.PostTime;
            existing.Event = version.Event;
            existing.Object = version.Object;
            existing.Schema = version.Schema;
            existing.TSQL = version.TSQL;
            existing.XmlEvent = version.XmlEvent;

            unitOfWork.DatabaseLogs.Update(existing);
            await unitOfWork.CompleteAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var existing = await unitOfWork.DatabaseLogs.GetByIdAsync(id);
            if (existing == null) return NotFound();

            unitOfWork.DatabaseLogs.Delete(existing);
            await unitOfWork.CompleteAsync();

            return NoContent();
        }
    }
}