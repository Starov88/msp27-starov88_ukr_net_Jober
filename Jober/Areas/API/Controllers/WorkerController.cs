using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Jober.Data;
using Jober.Models;
using Jober.Areas.API.Models;

namespace Jober.Areas.API.Controllers
{
    [Produces("application/json")]
    [Route("api/Worker")]
    public class WorkerController : Controller
    {
        private readonly ApplicationDbContext _context;

        public WorkerController(ApplicationDbContext context)
        {
            _context = context;
        }

        /*// GET: api/Workers
        [HttpGet]
        public IEnumerable<Worker> GetWorkers()
        {
            return _context.Workers;
        }*/

        /*// GET: api/Workers/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetWorker([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var worker = await _context.Workers.SingleOrDefaultAsync(m => m.Id == id);

            if (worker == null)
            {
                return NotFound();
            }

            return Ok(worker);
        }*/

        [HttpPut]
        public async Task<IActionResult> PutWorker([FromBody] PutWorkerSettings data)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            ApplicationUser userWorker = await GetUserWorkerFromRequest(data);
            if (userWorker == null)
                return Unauthorized();

            userWorker.Worker.WorkerSettingJson = data.GetWorkerSettingsJSON();
            if(string.IsNullOrEmpty(userWorker.Worker.WorkerSettingJson))
                BadRequest();

            _context.Entry(userWorker.Worker).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!WorkerExists(data.WorkerId))
                {
                    return NotFound();
                }
                else
                {
                    return NoContent();
                }
            }

            return Ok();
        }

        /*// POST: api/Workers
        [HttpPost]
        public async Task<IActionResult> PostWorker([FromBody] Worker worker)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Workers.Add(worker);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetWorker", new { id = worker.Id }, worker);
        }*/

        /*//DELETE: api/Workers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteWorker([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var worker = await _context.Workers.SingleOrDefaultAsync(m => m.Id == id);
            if (worker == null)
            {
                return NotFound();
            }

            _context.Workers.Remove(worker);
            await _context.SaveChangesAsync();

            return Ok(worker);
        }*/

        private bool WorkerExists(int id)
        {
            return _context.Workers.Any(e => e.Id == id);
        }

        private async Task<ApplicationUser> GetUserWorkerFromRequest(PutWorkerSettings data = null)
        {
            string userId;
            int workerId;

            if (data == null)
            {
                if (!Request.Query.ContainsKey("guid") && !Request.Query.ContainsKey("workerId"))
                {
                    return null;
                }

                userId = Request.Query["guid"];
                if (!int.TryParse(Request.Query["workerId"], out workerId))
                {
                    return null;
                }
            }
            else
            {
                userId = data.Guid;
                workerId = data.WorkerId;
            }

            ApplicationUser user = await _context.Users.Where(u => u.Id == userId && u.WorkerId == workerId)
                .Include(u => u.Worker)
                .SingleOrDefaultAsync();

            return user;
        }
    }
}