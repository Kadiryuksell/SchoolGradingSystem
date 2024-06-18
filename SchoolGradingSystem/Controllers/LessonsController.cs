using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Web;
using System.Web.Mvc;
using SchoolGradingSystem.Models.EntityFramework;

namespace SchoolGradingSystem.Controllers
{
    public class LessonsController : Controller
    {
 
        DbSchoolEntities dbLessons = new DbSchoolEntities();
        public ActionResult Lessons()
        {
            var lessons = dbLessons.Lessons.ToList();
            return View(lessons);
        }

        [HttpGet]
        public ActionResult AddLesson()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Update(int id)
        {
            var lessonsUpdate = dbLessons.Lessons.Find(id);
            return View("Update", lessonsUpdate);
        }

        [HttpPost]
        public ActionResult AddLesson(Lessons lesson)
        {
            if (!String.IsNullOrEmpty(lesson.LessonName))
            {

                if (ValidateUniquelesson(lesson))
                {
                    dbLessons.Lessons.Add(lesson);
                    dbLessons.SaveChanges();
                    return View();
                }
            }

            return View();
        }

        public ActionResult Remove(int id)
        {
            var lesson = dbLessons.Lessons.Find(id);
            dbLessons.Lessons.Remove(lesson);
            dbLessons.SaveChanges();
            return RedirectToAction("Lessons");
        }
        

        [HttpPost]
        public ActionResult UpdateLesson(Lessons lesson)
        {
            if (ModelState.IsValid)
            {
                if (ValidateUniquelesson(lesson))
                {
                    var lessonUpdate = dbLessons.Lessons.Find(lesson.LessonID);
                    lessonUpdate.LessonName = lesson.LessonName;
                    dbLessons.SaveChanges();
                    return RedirectToAction("Lessons", "Lessons");
                }
            }
            return RedirectToAction("Lessons", "Lessons");
        }

        private bool ValidateUniquelesson(Lessons lesson)
        {
            var existingLesson = dbLessons.Lessons.FirstOrDefault(i => i.LessonName == lesson.LessonName);
            if (existingLesson == null)
            {
                return true;
            }
            return false;
        }
    }
}