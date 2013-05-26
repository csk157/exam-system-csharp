using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Classes.Model
{
    public class Attempt
    {
        public int ID { get; set; }
        public DateTime Date { get; set; }
        public DateTime Created { get; set; }
        public String Grade { get; set; }
        public Exam Exam { get; set; }
        public Student Student { get; set; }

        public Attempt() {}
        public Attempt(DataRow dr, Exam e, Student s)
        {
            ID = int.Parse(dr["id"] as string);
            Grade = dr["grade"] as string;
            Created = DateTime.Parse(dr["created_at"] as string);

            Exam = e;
            Student = s;
            e.Attempts.Add(this);
            s.Attempts.Add(this);
        }
    }
}
