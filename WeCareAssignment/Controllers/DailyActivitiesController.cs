using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using WeCareAssignment.Data;
using WeCareAssignment.Models;

namespace WeCareAssignment.Controllers
{
    public class DailyActivitiesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DailyActivitiesController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View("Index", await _context.DailyActivity.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var dailyActivity = await _context.DailyActivity.FirstOrDefaultAsync(m => m.DailyActivityId == id);
            if (dailyActivity == null)
                return NotFound();

            return View("Details", dailyActivity);
        }

        public IActionResult Create()
        {
            return View("Create");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DailyActivityId,DailyActivityName")] DailyActivity dailyActivity)
        {
            if (ModelState.IsValid)
            {
                _context.Add(dailyActivity);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View("Create", dailyActivity);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var dailyActivity = await _context.DailyActivity.FindAsync(id);
            if (dailyActivity == null)
                return NotFound();

            return View("Edit", dailyActivity);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("DailyActivityId,DailyActivityName")] DailyActivity dailyActivity)
        {
            if (id != dailyActivity.DailyActivityId)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(dailyActivity);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DailyActivityExists(dailyActivity.DailyActivityId))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View("Edit", dailyActivity);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var dailyActivity = await _context.DailyActivity.FirstOrDefaultAsync(m => m.DailyActivityId == id);
            if (dailyActivity == null)
                return NotFound();

            return View("Delete", dailyActivity);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var dailyActivity = await _context.DailyActivity.FindAsync(id);
            if (dailyActivity != null)
                _context.DailyActivity.Remove(dailyActivity);

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DailyActivityExists(int id)
        {
            return _context.DailyActivity.Any(e => e.DailyActivityId == id);
        }
    }
}
