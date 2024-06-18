using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SchoolGradingSystem.Models.EntityFramework;

namespace SchoolGradingSystem.Controllers
{
    public class StudentsController : Controller
    {
        
        DbSchoolEntities dbStudent = new DbSchoolEntities();
        public ActionResult Student()
        {
            var studentsList = dbStudent.Students.ToList();
            return View(studentsList);
        }

        [HttpGet]
        public ActionResult AddStudent()
        {
            List<SelectListItem> clubValue = (from i in dbStudent.Clubs.ToList()
                                              select new SelectListItem
                                              {
                                                  Text = i.ClubName,
                                                  Value = i.ClubID.ToString()
                                              }).ToList();
            ViewBag.studentClubs = clubValue;
            return View();
        }
        [HttpGet]
        public ActionResult Update(int id)
        {
            var studentUpdate = dbStudent.Students.Find(id);
            List<SelectListItem> clubValue = (from i in dbStudent.Clubs.ToList()
                                              select new SelectListItem
                                              {
                                                  Text = i.ClubName,
                                                  Value = i.ClubID.ToString()
                                              }).ToList();
            ViewBag.studentClubs = clubValue;
            return View("Update", studentUpdate);
        }

        [HttpPost]
        public ActionResult AddStudent(Students student)
        {
                if ( ValidateUniqueStudent(student))
                {
                    var studentClubs = dbStudent.Clubs.Where(i => i.ClubID == student.Clubs.ClubID).FirstOrDefault();
                    student.Clubs = studentClubs;
                    dbStudent.Students.Add(student);
                    dbStudent.SaveChanges();
                    return RedirectToAction("Student");
                }      
            return View();
        }

        public ActionResult Remove(int id) 
        {
            var studentRemove = dbStudent.Students.Find(id);
            dbStudent.Students.Remove(studentRemove);
            dbStudent.SaveChanges();
            return RedirectToAction("Student");
        }
        [HttpPost]
        public ActionResult UpdateStudent(Students student)
        {
            if (ModelState.IsValid)
            {
                if (ValidateUniqueStudent(student))
                {
                    var studentUpdate = dbStudent.Students.Find(student.StudentID);
                    studentUpdate.StudentName = student.StudentName;
                    studentUpdate.StudentSurname = student.StudentSurname;
                    studentUpdate.StudentSex = student.StudentSex;
                    studentUpdate.Clubs.ClubName = student.Clubs.ClubName;
                    studentUpdate.StudentPhoto = student.StudentPhoto;
                    dbStudent.SaveChanges();
                    return RedirectToAction("Student", "Students");
                }
            }
            return RedirectToAction("Student", "Students");
        }

        private bool ValidateUniqueStudent(Students student)
        {
            if (!(string.IsNullOrEmpty(student.StudentName) && string.IsNullOrEmpty(student.StudentSurname)))
            {
                var existingStudent = dbStudent.Students.FirstOrDefault(i => i.StudentName == student.StudentName && i.StudentSurname == student.StudentSurname);
                if (existingStudent == null)
                {
                    return true;
                }
                return false;
            }
            return false;
                
        }

    }
}