using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace StudentApplication.Models
{
    public class Subject
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SubjectID { get; set; }
        [Display(Name ="Subject Name")]
        public string SubjectName { get; set; }
        public int Marks { get; set; }

        [ForeignKey("Student")]
        public int StudentID { get; set; }
        public virtual ICollection<Student> Student { get; set; }
    }
}