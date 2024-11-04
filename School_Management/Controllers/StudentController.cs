using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using School_Management.Data;
using School_Management.Models;

namespace School_Management.Controllers
{
    public class StudentController : Controller
    {
        private readonly SchoolDbContext _context;

        public StudentController(SchoolDbContext context)
        {
            _context = context;
        }
        [Authorize]
        public async Task<IActionResult> Studentlist()
        {
            try
            {
                var students = await _context.Students.ToListAsync();
                return View(students);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while retrieving students.";
                // Log the exception if necessary
            }
            return View(new List<Student>());
        }

        [HttpGet]
        [Authorize]
        public IActionResult StudentCreate()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> StudentCreate(Student model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _context.Students.Add(model);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Student Added Successfully!";
                    return RedirectToAction("Studentlist");
                }
                TempData["ErrorMessage"] = "Invalid student data.";
            }
            catch (Exception ex) {
                TempData["ErrorMessage"] = "An error occurred while adding the student.";
            }
            return View(model);
            }

        [HttpGet]
        public async Task <IActionResult> StudentEdit(int? id)
        {
            if(id == null) return NotFound();
            var student = await _context.Students.FindAsync(id);
            if (student == null) return NotFound();
            return View(student); 
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> StudentEdit(int id, Student student)
        {
            if(id != student.Id)
            {
                TempData["ErrorMessage"] = "Student ID mismatch.";
                return RedirectToAction("Studentlist");
            }

            try
            {
                if (ModelState.IsValid)
                {
                    _context.Update(student);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Student updated successfully!";
                    return RedirectToAction("Studentlist");
                }
                TempData["ErrorMessage"] = "Invalid student data.";
            }
            catch (DbUpdateException ex) {
                TempData["ErrorMessage"] = "An error occurred while updating the student.";
            }
            return View(student);
        }
     }
}
