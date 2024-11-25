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

        [HttpGet]
        public async Task<IActionResult> EnrollmentEdit(int? id)
        {
            if (id == null)
            {
                TempData["ErrorMessage"] = "Invalid enrollment ID.";
                return RedirectToAction(nameof(EnrollmentList)); // Redirect to a list page or another view
            }

            var enrollment = await _context.Enrollments.FindAsync(id);
            if (enrollment == null)
            {
                TempData["ErrorMessage"] = "Enrollment not found.";
                return RedirectToAction(nameof(EnrollmentList));
            }

            // Populate dropdowns
            ViewBag.StudentList = new SelectList(_context.Students, "Id", "FirstName", enrollment.StudentId);
            ViewBag.ClassList = new SelectList(_context.Classes, "Id", "ClassName", enrollment.ClassId);
            ViewBag.SubjectList = new SelectList(_context.Subjects, "Id", "SubjectName", enrollment.SubjectId);

            return View(enrollment);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EnrollmentEdit(int id, Enrollment enrollment)
        {
            if (id != enrollment.Id)
            {
                TempData["ErrorMessage"] = "Mismatched enrollment ID.";
                return RedirectToAction(nameof(EnrollmentList));
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(enrollment);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Enrollment updated successfully!";
                    return RedirectToAction(nameof(EnrollmentList));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Enrollments.Any(e => e.Id == enrollment.Id))
                    {
                        TempData["ErrorMessage"] = "Enrollment not found.";
                        return RedirectToAction(nameof(EnrollmentList));
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            // Repopulate dropdowns in case of validation errors
            ViewBag.StudentList = new SelectList(_context.Students, "Id", "FirstName", enrollment.StudentId);
            ViewBag.ClassList = new SelectList(_context.Classes, "Id", "ClassName", enrollment.ClassId);
            ViewBag.SubjectList = new SelectList(_context.Subjects, "Id", "SubjectName", enrollment.SubjectId);

            TempData["ErrorMessage"] = "Please correct the highlighted errors.";
            return View(enrollment);
        }



        public async Task<IActionResult> EnrollmentDelete(int? id)
        {
            if (id == null)
            {
                TempData["ErrorMessage"] = "Invalid enrollment ID.";
                return RedirectToAction(nameof(EnrollmentList));
            }

            var enrollment = await _context.Enrollments
                .Include(e => e.Student)
                .Include(e => e.Class)
                .Include(e => e.Subject)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (enrollment == null)
            {
                TempData["ErrorMessage"] = "Enrollment not found.";
                return RedirectToAction(nameof(EnrollmentList));
            }

            return View(enrollment);
        }
        [HttpPost, ActionName("EnrollmentDelete")]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> EnrollmentDeleteConfirmed(int id)
        {
            try
            {
                var enrollment = await _context.Enrollments.FindAsync(id);
                if (enrollment != null)
                {
                    _context.Enrollments.Remove(enrollment);
                    await _context.SaveChangesAsync();

                    TempData["SuccessMessage"] = "Enrollment deleted successfully!";
                }
                else
                {
                    TempData["ErrorMessage"] = "Enrollment not found.";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while deleting the enrollment.";
            }

            return RedirectToAction(nameof(EnrollmentList));
        }
    }
}
