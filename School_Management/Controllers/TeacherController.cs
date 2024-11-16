using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using School_Management.Data;
using School_Management.Models;

namespace School_Management.Controllers
{
    public class TeacherController : Controller
    {
        private readonly SchoolDbContext _context;
        public TeacherController(SchoolDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Teacherlist()
        {
            try
            {
                var teacher = await _context.Teachers.ToListAsync();
                return View(teacher);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while retrieving teachers.";                
            }
            return View(new List<Teacher>());
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult TeacherCreate()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> TeacherCreate(Teacher model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _context.Teachers.Add(model);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Teacher Added Successfully!";
                    return RedirectToAction("Teacherlist");
                }
                TempData["ErrorMessage"] = "Invalid Teacher data.";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while adding the Teacher.";
            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> TeacherEdit(int? id)
        {
            if (id == null) return NotFound();
            var teacher = await _context.Teachers.FindAsync(id);
            if (teacher == null) return NotFound();
            return View(teacher);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> TeacherEdit(int id, Teacher teacher)
        {
            if (id != teacher.Id)
            {
                TempData["ErrorMessage"] = "Teacher ID mismatch.";
                return RedirectToAction("Teacherlist");
            }

            try
            {
                if (ModelState.IsValid)
                {
                    _context.Update(teacher);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Teacher updated successfully!";
                    return RedirectToAction("Teacherlist");
                }
                TempData["ErrorMessage"] = "Invalid Teacher data.";
            }
            catch (DbUpdateException ex)
            {
                TempData["ErrorMessage"] = "An error occurred while updating the Teacher.";
            }
            return View(teacher);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> TchrDelete(int? id)
        {
            if (id == null) return NotFound();
            var teacher = await _context.Teachers.FindAsync(id);
            if (teacher == null) return NotFound();
            return View(teacher);
        }

        [HttpPost, ActionName("TchrDelete")]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> TchrDeleteConfirmed(int id)
        {
            var teacher = await _context.Teachers.FindAsync(id);

            if (teacher == null)
            {
                TempData["ErrorMessage"] = "Record not found.";
                return RedirectToAction(nameof(Teacherlist));
            }

            try
            {
                _context.Teachers.Remove(teacher);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Record deleted successfully.";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error deleting record: " + ex.Message;
            }

            return RedirectToAction(nameof(Teacherlist));
        }
    }
}
