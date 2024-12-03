using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using WeCareAssignment.Data;
using WeCareAssignment.Models;

namespace WeCareAssignment.Controllers
{
    public class ChildrenController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ChildrenController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Child.Include(c => c.Parents).Include(c => c.Teachers).Include(c => c.DailyActivities);
            return View("Index", await applicationDbContext.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var child = await _context.Child.Include(c => c.Parents).Include(c => c.Teachers).Include(c => c.DailyActivities).FirstOrDefaultAsync(m => m.childId == id);
            if (child == null)
                return NotFound();

            return View("Details", child);
        }

        public IActionResult Create()
        {
            ViewData["ParentId"] = new SelectList(_context.Parent.OrderBy(c => c.ParentName), "ParentId", "ParentName");
            ViewData["TeacherId"] = new SelectList(_context.Teacher.OrderBy(c => c.TeacherName), "TeacherId", "TeacherName");
            ViewData["DailyActivityId"] = new SelectList(_context.DailyActivity.OrderBy(c => c.DailyActivityName), "DailyActivityId", "DailyActivityName");
            return View("Create");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("childId,childName,ParentId,TeacherId,DailyActivityId")] Child child)
        {
            if (ModelState.IsValid)
            {
                _context.Add(child);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["ParentId"] = new SelectList(_context.Parent.OrderBy(c => c.ParentName), "ParentId", "ParentName", child.ParentId);
            ViewData["TeacherId"] = new SelectList(_context.Teacher.OrderBy(c => c.TeacherName), "TeacherId", "TeacherName", child.TeacherId);
            ViewData["DailyActivityId"] = new SelectList(_context.DailyActivity.OrderBy(c => c.DailyActivityName), "DailyActivityId", "DailyActivityName", child.DailyActivityId);
            return View("Create", child);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var child = await _context.Child.FindAsync(id);
            if (child == null)
                return NotFound();

            ViewData["ParentId"] = new SelectList(_context.Parent.OrderBy(c => c.ParentName), "ParentId", "ParentName", child.ParentId);
            ViewData["TeacherId"] = new SelectList(_context.Teacher.OrderBy(c => c.TeacherName), "TeacherId", "TeacherName", child.TeacherId);
            ViewData["DailyActivityId"] = new SelectList(_context.DailyActivity.OrderBy(c => c.DailyActivityName), "DailyActivityId", "DailyActivityName", child.DailyActivityId);
            return View("Edit", child);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("childId,childName,ParentId,TeacherId,DailyActivityId")] Child child)
        {
            if (id != child.childId)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(child);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ChildExists(child.childId))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }

            ViewData["ParentId"] = new SelectList(_context.Parent.OrderBy(c => c.ParentName), "ParentId", "ParentName", child.ParentId);
            ViewData["TeacherId"] = new SelectList(_context.Teacher.OrderBy(c => c.TeacherName), "TeacherId", "TeacherName", child.TeacherId);
            ViewData["DailyActivityId"] = new SelectList(_context.DailyActivity.OrderBy(c => c.DailyActivityName), "DailyActivityId", "DailyActivityName", child.DailyActivityId);
            return View("Edit", child);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var child = await _context.Child.FirstOrDefaultAsync(m => m.childId == id);
            if (child == null)
                return NotFound();

            return View("Delete", child);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var child = await _context.Child.FindAsync(id);
            if (child != null)
                _context.Child.Remove(child);

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ChildExists(int id)
        {
            return _context.Child.Any(e => e.childId == id);
        }
    }
}
