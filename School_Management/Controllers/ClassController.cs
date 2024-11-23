using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using School_Management.Data;
using School_Management.Models;
namespace School_Management.Controllers
{
    public class ClassController : Controller
    {
        private readonly SchoolDbContext _context;

        public ClassController(SchoolDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> ClassList()
        {
            var classes = await _context.Classes.Include(c => c.Teacher).ToListAsync();
            return View(classes);
        }

        private void PopulateTeacherDropdown()
        {
            ViewBag.TeacherList = new SelectList(_context.Teachers, "Id", "FirstName");
        }

        public async Task<IActionResult> ClassDetails(int? id)
        {
            if (id == null) return NotFound();

            var @class = await _context.Classes
                .Include(c => c.Teacher)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (@class == null) return NotFound();

            return View(@class);
        }

        public IActionResult ClassCreate()
        {
            PopulateTeacherDropdown();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ClassCreate(Class @class)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _context.Add(@class);
                    await _context.SaveChangesAsync();

                    TempData["SuccessMessage"] = "Class created successfully!";
                    return RedirectToAction(nameof(ClassList)); 
                }

                // Capture and log validation errors
                var errors = ModelState.Values
                                        .SelectMany(v => v.Errors)
                                        .Select(e => e.ErrorMessage)
                                        .ToList();

                TempData["ErrorMessage"] = "Please correct the highlighted errors and try again.";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while creating the class.";
                
            }

            // Repopulate the dropdown if ModelState is invalid or an exception occurs
            PopulateTeacherDropdown();
            return View(@class);
        }


        public async Task<IActionResult> ClassEdit(int? id)
        {
            if (id == null) return NotFound();

            var @class = await _context.Classes.FindAsync(id);
            if (@class == null) return NotFound();

            PopulateTeacherDropdown();
            return View(@class);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ClassEdit(int id, Class @class)
        {
            if (id != @class.Id)
            {
                return NotFound();
            }

            try
            {
                var existingClass = await _context.Classes.FindAsync(id);
                if (existingClass == null)
                {
                    return NotFound();
                }

                existingClass.ClassName = @class.ClassName;
                existingClass.Section = @class.Section;
                existingClass.TeacherId = @class.TeacherId;

                await _context.SaveChangesAsync();
                TempData["Success"] = "Class updated successfully!";
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"An error occurred: {ex.Message}";
            }

            return RedirectToAction(nameof(ClassList));
        }

        public async Task<IActionResult> ClassDelete(int? id)
        {
            if (id == null) return NotFound();

            var @class = await _context.Classes
                .Include(c => c.Teacher)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (@class == null) return NotFound();

            return View(@class);
        }

        [HttpPost, ActionName("ClassDelete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ClassDeleteConfirmed(int id)
        {
            var @class = await _context.Classes.FindAsync(id);
            if (@class != null)
            {
                _context.Classes.Remove(@class);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Class deleted successfully!";
            }
            return RedirectToAction(nameof(ClassList));
        }
    }
}
