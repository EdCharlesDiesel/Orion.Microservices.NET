using Microsoft.AspNetCore.Mvc;
using Orion.DataAccess.Postgres.Tools;

namespace Orion.API.TransactionHistoryArchive.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionHistoryArchiveController(IUnitOfWork unitOfWork) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var versions = await unitOfWork.TransactionHistoryArchives.GetAllAsync();
            return Ok(versions);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var version = await unitOfWork.TransactionHistoryArchives.GetByIdAsync(id);
            if (version == null) return NotFound();
            return Ok(version);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] DataAccess.Postgres.Entities.TransactionHistoryArchive version)
        {
            await unitOfWork.TransactionHistoryArchives.AddAsync(version);
            await unitOfWork.CompleteAsync();
            return CreatedAtAction(nameof(GetById), new { id = version.TransactionID }, version);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, DataAccess.Postgres.Entities.TransactionHistoryArchive version)
        {
            var existing = await unitOfWork.TransactionHistoryArchives.GetByIdAsync(id);
            if (existing == null) return NotFound();

            existing.ActualCost = version.ActualCost;
            existing.ProductID = version.ProductID;
            existing.Quantity = version.Quantity;
            existing.ReferenceOrderID = version.ReferenceOrderID;
            existing.TransactionDate = version.TransactionDate;
            existing.ReferenceOrderLineID = version.ReferenceOrderLineID;
            existing.TransactionType = version.TransactionType;
            existing.ReferenceOrderID = version.ReferenceOrderID;
            existing.ModifiedDate = version.ModifiedDate;

            unitOfWork.TransactionHistoryArchives.Update(existing);
            await unitOfWork.CompleteAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var existing = await unitOfWork.TransactionHistoryArchives.GetByIdAsync(id);
            if (existing == null) return NotFound();

            unitOfWork.TransactionHistoryArchives.Delete(existing);
            await unitOfWork.CompleteAsync();

            return NoContent();
        }
    }
}