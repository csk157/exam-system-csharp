using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Classes.Model
{
    public class Education
    {
        public int ID { get; set; }
        public String Title { get; set; }
        public DateTime Created { get; set; }

        public List<Student> Students { get; set; }
        public List<Exam> Exams { get; set;}

        public Education(){
            Students = new List<Student>();
            Exams = new List<Exam>();
        }

        public Education(DataRow dr) : this()
        {
            Console.WriteLine(dr);
            ID = (int) dr["id"];
            Title = (string) dr["title"];
            Created = (DateTime) dr["created_at"];

            foreach(DataRow cRow in dr.GetChildRows("EducationStudents"))
                Students.Add(new Student(cRow, this));           
            
            foreach(DataRow cRow in dr.GetChildRows("EducationExams"))
                Exams.Add(new Exam(cRow, this));
        }
    }
}
