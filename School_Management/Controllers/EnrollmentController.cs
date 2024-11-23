using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using School_Management.Data;
using School_Management.Models;

namespace School_Management.Controllers
{
    public class EnrollmentController : Controller
    {
        private readonly SchoolDbContext _context;            
        public EnrollmentController (SchoolDbContext context)
        {
            _context = context;
        }
        public async Task <IActionResult> EnrollmentList()
        {
            var enrollments = await _context.Enrollments
                .Include(e => e.Student)
                .Include(e => e.Class)
                .Include(e => e.Subject)
                .ToListAsync();
            
            return View(enrollments);
        }

        public IActionResult EnrollmentCreate()
        {
            ViewBag.StudentList = new SelectList(_context.Students, "Id", "FirstName");
            ViewBag.ClassList = new SelectList(_context.Classes, "Id", "ClassName");
            ViewBag.SubjectList = new SelectList(_context.Subjects, "Id", "SubjectName");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> EnrollmentCreate(Enrollment enrollment)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _context.Add(enrollment);
                    await _context.SaveChangesAsync();

                    TempData["SuccessMessage"] = "Student enrolled successfully!";
                    return RedirectToAction(nameof(EnrollmentList));
                }
            }

            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while enrolling the student.";
            }

            // Repopulate dropdowns on error
            ViewBag.StudentList = new SelectList(_context.Students, "Id", "FirstName");
            ViewBag.ClassList = new SelectList(_context.Classes, "Id", "ClassName");
            ViewBag.SubjectList = new SelectList(_context.Subjects, "Id", "SubjectName");
            return View(enrollment);
        }
    }
}
