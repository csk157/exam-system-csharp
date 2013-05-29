using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Classes.Model
{
    public class Attempt : Model
    {
        public static readonly string table = "Attempts";
        public DateTime Date { get; set; }
        public DateTime Created { get; set; }
        public string Grade { get; set; }
        public Exam Exam { get; set; }
        public Student Student { get; set; }

        public static string[] allowedGrades = {"SY", "EJ", "-3", "00", "02", "4", "7", "10", "12" };

        public Attempt() : base(table){}
        public Attempt(Exam e, Student s)
            : this()
        {
            Exam = e;
            Student = s;
            Grade = "";
        }
        public Attempt(DataRow dr, Exam e, Student s) : this(e, s)
        {
            ReadRow(dr);
            e.Attempts.Add(this);
            s.Attempts.Add(this);
        }

        public bool IsPassed()
        {
            return Grade != null && allowedGrades.Contains(Grade) && !(new string[] { "SY", "EJ", "-3", "00" }).Contains(Grade);
        }

        public bool IsTaken()
        {
            return Grade != "SY" && !String.IsNullOrEmpty(Grade);
        }

        public bool Happened()
        {
            return !String.IsNullOrWhiteSpace(Grade);
        }

        protected override void ReadRow(DataRow dr)
        {
            data = dr;
            ID = (int) dr["id"];
            Grade = (string) dr["grade"];
            Created = (DateTime) dr["created_at"];

            if (Grade == null)
                Grade = "";
        }

        public override DataRow FillRow(DataRow dr)
        {
            dr["grade"] = Grade;
            dr["exam_id"] = Exam.ID;
            dr["student_id"] = Student.ID;

            if (Created == DateTime.MinValue)
                dr["created_at"] = DateTime.Now;
            else
                dr["created_at"] = Created;

            return dr;
        }

        public override void BeforeSave()
        {
        }

        public override void AfterSave()
        {
        }

        public override void BeforeCreate()
        {
        }

        public override void AfterCreate()
        {
            Student.Attempts.Add(this);
            Exam.Attempts.Add(this);
        }

        public override void BeforeDelete()
        {
        }

        public override void AfterDelete()
        {
            Student.Attempts.Remove(this);
            Exam.Attempts.Remove(this);
        }
    }
}
