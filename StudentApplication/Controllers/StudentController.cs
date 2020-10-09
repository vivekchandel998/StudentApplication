using StudentApplication.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace StudentApplication.Controllers
{
    public class StudentController : Controller
    {
        StudentDbContext context;
        public StudentController()
        {
            context = new StudentDbContext();
        }

        public ActionResult StudentDetails(string queryParam)
        {
            List<Student> students;
            if (queryParam == null || string.IsNullOrEmpty(queryParam))
            {
                students = context.Students.OrderBy(x => x.FirstName).ThenBy(x => x.LastName).ToList();
            }
            else
            {
                students = context.Students.Where(x => x.FirstName.ToLower() == queryParam.ToLower()).OrderBy(x => x.FirstName).ThenBy(x => x.LastName).ToList();
            }
            foreach (var item in students)
            {
                item.Subjects = context.Subjects.Where(x => x.StudentID == item.ID).ToList();
            }
            return View(students);
        }

        [HttpGet]
        public ActionResult AddStudent(int? id)
        {
            StudentViewModel student = new StudentViewModel();
            if (id != null)
            {
                ViewBag.Mode = "Edit";
                id = Convert.ToInt32(id);
                student.Subjects = context.Subjects.Where(x => x.SubjectID == id).ToArray();
                int studentID = student.Subjects[0].StudentID;
                student.Students = context.Students.FirstOrDefault(x => x.ID == studentID);
            }
            return View(student);
        }

        [HttpPost]
        public ActionResult AddStudent(StudentViewModel student)
        {
            if (student.Students.ID > 0)
            {
                context.Students.Add(student.Students);
                context.Entry(student.Students).State = EntityState.Modified;
                context.Subjects.Add(student.Subjects[0]);
                context.Entry(student.Subjects[0]).State = EntityState.Modified;
            }
            else
            {
                context.Students.Add(student.Students);
                context.SaveChanges();
                foreach (var subject in student.Subjects)
                {
                    subject.StudentID = student.Students.ID;
                    context.Subjects.Add(subject);
                }
            }
            context.SaveChanges();
            return RedirectToAction("StudentDetails");
        }

        public ActionResult DeleteStudent(int studentID, int subjectID)
        {
            int count = context.Subjects.Where(x => x.StudentID == studentID).ToList().Count;
            if (count > 1)
            {
                Subject sub = context.Subjects.Find(subjectID);
                context.Subjects.Remove(sub);
            }
            else
            {
                Student student = context.Students.Find(studentID);
                Subject sub = context.Subjects.Find(subjectID);
                context.Subjects.Remove(sub);
                context.Students.Remove(student);
            }
            context.SaveChanges();
            return RedirectToAction("StudentDetails");
        }

    }
}