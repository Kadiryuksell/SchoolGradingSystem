using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SchoolGradingSystem.Models.EntityFramework;
using SchoolGradingSystem.Models;

namespace SchoolGradingSystem.Controllers
{
    public class NotesController : Controller
    {
        DbSchoolEntities dbNotes = new DbSchoolEntities();
        public ActionResult Note()
        {
            var noteList = dbNotes.Notes.ToList();
            return View(noteList);
        }

        [HttpGet]
        public ActionResult AddNote()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AddNote(Notes note, int exam1, int exam2, int exam3, int project)
        {     
                if (ModelState.IsValid)
                {
                int avgValue = ExamAvgResult(exam1, exam2, exam3, project);
                note.Average = avgValue;
                note.Statu = ExamPassResult(avgValue);
                    dbNotes.Notes.Add(note);
                    dbNotes.SaveChanges();
                }
 
            return RedirectToAction("Note");
        }
        [HttpGet]
        public ActionResult Update(int id)
        {
            var noteUpdate = dbNotes.Notes.Find(id);
            return View("Update", noteUpdate);
        }

        [HttpPost]
        public ActionResult Update(Notes note, int exam1, int exam2, int exam3, int project, SelectButton model)
        {
            if (model.SelectOperation == "Calculate")
            {
                double averageResult = ExamAvgResult(exam1, exam2, exam3, project);
                ViewBag.avg = averageResult;
                ViewBag.avgPass = ExamPassResult(averageResult);
                return View();
            }


            var noteUpdate = dbNotes.Notes.Find(note.NoteID);
            noteUpdate.Students.StudentName = note.Students.StudentName;
            noteUpdate.Students.StudentSurname = note.Students.StudentSurname;
            noteUpdate.Lessons.LessonName = note.Lessons.LessonName;
            noteUpdate.Exam1 = note.Exam1;
            noteUpdate.Exam2 = note.Exam2;
            noteUpdate.Exam3 = note.Exam3;
            noteUpdate.Project = note.Project;
            noteUpdate.Average = note.Average;
            noteUpdate.Statu = note.Statu;
            dbNotes.SaveChanges();
            return RedirectToAction("Note","Notes");
        }

        public bool ExamPassResult(double studentAverage)
        {
            return studentAverage >=50 ? true : false;
        }
        public int ExamAvgResult(int exam1, int exam2, int exam3, int project)
        {
            return (exam1 + exam2 + exam3 + project) / 4;
        }
    }
}