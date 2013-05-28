using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Classes.Model
{
    public class Exam : Model, IDataErrorInfo
    {
        public static readonly string table = "Exams";
        public string Title { get; set; }
        private Education initialEducation;
        private Education education;
        public Education Education{ get{return education;} 
            set{
                education = value;
                if (initialEducation == null) initialEducation = value;
            } 
        }
        public ObservableCollection<Attempt> Attempts { get; set; }
        public DateTime Created { get; set; }

        public string Error
        {
            get { return null; }
        }

        public string this[string columnName]
        {
            get {
                string result = null;

                switch (columnName)
                {
                    case "Title":
                        if (String.IsNullOrWhiteSpace(Title))
                            result = "Title cannot be empty";
                        break;
                }
                return result;
            }
        }

        public Exam() : base(table)
        {
            Attempts = new ObservableCollection<Attempt>();
        }

        public Exam(DataRow dr, Education ed)
            : this()
        {
            ReadRow(dr);
            Education = ed;
            initialEducation = ed;
        }

        public IEnumerable NotExamined()
        {
            return (from at in Attempts where !at.Happened() select at);
        }

        protected override void ReadRow(DataRow dr)
        {
            data = dr;
            ID = (int)dr["id"];
            Title = (string)dr["title"];
            Created = (DateTime)dr["created_at"];
        }


        public override DataRow FillRow(DataRow dr)
        {
            dr["title"] = Title;
            dr["education_id"] = Education.ID;
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
            if (initialEducation != Education)
            {
                initialEducation.Exams.Remove(this);
                Education.Exams.Add(this);
                initialEducation = Education;
            }
        }

        public override void BeforeCreate()
        {
        }

        public override void AfterCreate()
        {
            Education.Exams.Add(this);
        }

        public override void BeforeDelete()
        {
        }

        public override void AfterDelete()
        {
            Education.Exams.Remove(this);
        }

        public bool IsValid()
        {
            bool valid = true;

            if (String.IsNullOrWhiteSpace(Title))
                valid = false;

            return valid;
        }

        public override string ToString()
        {
            return Title;
        }
    }
}
