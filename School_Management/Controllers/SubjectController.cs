using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using School_Management.Data;
using School_Management.Models;

namespace School_Management.Controllers
{
    public class SubjectController : Controller
    {
        private readonly SchoolDbContext _context;
        public SubjectController(SchoolDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Subjectlist()
        {
            try
            {
                var subject = await _context.Subjects.ToListAsync();
                return View(subject);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while retrieving Subjects.";
            }
            return View(new List<Subject>());
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult SubjectCreate()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> SubjectCreate(Subject model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _context.Subjects.Add(model);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Subject Added Successfully!";
                    return RedirectToAction("Subjectlist");
                }
                TempData["ErrorMessage"] = "Invalid Subject data.";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while adding the Subject.";
            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> SubjectEdit(int? id)
        {
            if (id == null) return NotFound();
            var subject = await _context.Subjects.FindAsync(id);
            if (subject == null) return NotFound();
            return View(subject);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> SubjectEdit(int id, Subject subject)
        {
            if (id != subject.Id)
            {
                TempData["ErrorMessage"] = "Subject ID mismatch.";
                return RedirectToAction("Subjectlist");
            }

            try
            {
                if (ModelState.IsValid)
                {
                    _context.Update(subject);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Subject updated successfully!";
                    return RedirectToAction("Subjectlist");
                }
                TempData["ErrorMessage"] = "Invalid Subject data.";
            }
            catch (DbUpdateException ex)
            {
                TempData["ErrorMessage"] = "An error occurred while updating the Subject.";
            }
            return View(subject);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> SbjctDelete(int? id)
        {
            if (id == null) return NotFound();
            var subject = await _context.Subjects.FindAsync(id);
            if (subject == null) return NotFound();
            return View(subject);
        }

        [HttpPost, ActionName("SbjctDelete")]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SbjctDeleteConfirmed(int id)
        {
            var subject = await _context.Subjects.FindAsync(id);

            if (subject == null)
            {
                TempData["ErrorMessage"] = "Record not found.";
                return RedirectToAction(nameof(Subjectlist));
            }

            try
            {
                _context.Subjects.Remove(subject);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Record deleted successfully.";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error deleting record: " + ex.Message;
            }

            return RedirectToAction(nameof(Subjectlist));
        }
    }
}
