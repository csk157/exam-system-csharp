using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Classes.Model
{
    public class Education : Model, IDataErrorInfo
    {
        public static readonly string table = "Educations";
        public string Title { get; set; }
        public DateTime Created { get; set; }
        public ObservableCollection<Student> Students { get; set; }
        public ObservableCollection<Exam> Exams { get; set; }

        public string Error
        {
            get { return null; }
        }

        public string this[string columnName]
        {
            get {
                string result = null;
                if (String.IsNullOrWhiteSpace(Title))
                    result = "Title cannot be empty";
                return result;
            }
        }

        public Education() : base(table){
            Students = new ObservableCollection<Student>();
            Exams = new ObservableCollection<Exam>();
        }

        public Education(DataRow dr) : this()
        {
            ReadRow(dr);
        }

        public Education(string title) : this()
        {
            Title = title;
        }

        public bool IsValid()
        {
            if (String.IsNullOrWhiteSpace(Title))
                return false;
            return true;
        }

        protected override void ReadRow(DataRow dr)
        {
            data = dr;
            ID = (int)dr["id"];
            Title = (string)dr["title"];
            Created = (DateTime)dr["created_at"];

            Students.Clear();
            Exams.Clear();
            foreach (DataRow cRow in dr.GetChildRows("EducationStudents"))
                Students.Add(new Student(cRow, this));

            foreach (DataRow cRow in dr.GetChildRows("EducationExams"))
                Exams.Add(new Exam(cRow, this));
        }

        public override DataRow FillRow(DataRow dr)
        {
            data = dr;
            dr["title"] = Title;
            if(Created == DateTime.MinValue)
                dr["created_at"] = DateTime.Now;
            else
                dr["created_at"] = Created;
            return dr;
        }

        public override string ToString()
        {
            return Title;
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
        }

        public override void BeforeDelete()
        {
            foreach (Student s in Students)
                s.Delete();

            foreach (Exam e in Exams)
                e.Delete();
        }

        public override void AfterDelete()
        {
            foreach (Student s in Students)
                s.AfterDelete();

            foreach (Exam e in Exams)
                e.AfterDelete();
        }
    }
}
