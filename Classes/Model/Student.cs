using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Classes.Model
{
    public class Student : Model, IDataErrorInfo
    {
        public static readonly string table = "Students";
        public string Name{ get; set;}
        public string CPR { get; set; }
        private Education initialEducation;
        private Education education;
        public Education Education
        {
            get { return education; }
            set
            {
                education = value;
                if (initialEducation == null) initialEducation = value;
            }
        }
        public string Password { get; set; }
        public DateTime Start { get; set; }
        public DateTime Created { get; set; }
        public ObservableCollection<Attempt> Attempts { get; set; }

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
                    case "Name":
                        if (String.IsNullOrWhiteSpace(Name))
                            result = "Name cannot be empty";
                        break;
                    case "CPR":
                        if (String.IsNullOrWhiteSpace(CPR))
                            result = "CPR cannot be empty";
                        else if (!Regex.IsMatch(CPR, @"^\d+$"))
                            result = "CPR can only contain numbers";
                        else if (CPR.Length != 10)
                            result = "CPR should be 10 digits";
                        else if (!IsValidCPR(CPR))
                            result = "Not valid CPR";
                        break;
                    case "Start":
                        if (Start.Year < 2000)
                            result = "Cannot start before 2000";
                        break;
                }
                return result;
            }
        }


        public Student() : base(table){
            Attempts = new ObservableCollection<Attempt>();
            Start = DateTime.Now;
        }
        public Student(DataRow dr, Education ed) : this()
        {
            ReadRow(dr);
            Education = ed;
            initialEducation = ed;
        }

        public bool HasRegisteredForExam(Exam e)
        {
            int count = (from at in Attempts where at.Exam == e && !at.Happened() select at).ToArray().Length;
            return count != 0;
        }

        public bool HasFailedExam(Exam e)
        {
            int count = (from at in Attempts where at.Exam == e && !at.IsPassed() select at).ToArray().Length;
            return count != 0;
        }

        public bool HasPassedExam(Exam e)
        {
            int count = (from at in Attempts where at.Exam == e && at.IsPassed() select at).ToArray().Length;
            return count != 0;
        }

        public bool NeedsExemption(Exam e)
        {
            int count = (from at in Attempts where at.Exam == e && !at.IsPassed() && at.Happened() && at.IsTaken() && !HasRegisteredForExam(e) select at).ToArray().Length;
            return count >= 3;
        }

        public bool CanRegisterForExam(Exam e)
        {
            return !HasRegisteredForExam(e) && !HasPassedExam(e) && !NeedsExemption(e);
        }

        public bool CanGetExemption(Exam e)
        {
            int count = (from at in Attempts where at.Exam == e && !at.IsPassed() && at.Happened() && at.IsTaken() select at).ToArray().Length;
            return count < 6;
        }

        public IEnumerable<Attempt> GetFailedAttempts()
        {
            return  (from at in Attempts where !at.IsPassed() && at.Happened() && at.IsTaken() select at);
        }

        public IEnumerable<Exam> GetFailedExams()
        {
            return (from at in GetFailedAttempts() select at.Exam).Distinct();
        }

        public IEnumerable<Exam> GetExamsNeedingExemptions()
        {
            return (from ex in GetFailedExams() where this.NeedsExemption(ex) && !this.HasRegisteredForExam(ex) select ex);
        }

        public bool RequiresExemptions()
        {
            return GetExamsNeedingExemptions().Count() > 0;
        }

        public IEnumerable<Exam> GetNeededExams()
        {
            return from ex in Education.Exams where !this.HasPassedExam(ex) && !this.HasRegisteredForExam(ex) select ex;
        }

        protected override void ReadRow(DataRow dr)
        {
            data = dr;
            ID = (int)dr["id"];
            Name = (string)dr["name"];
            CPR = (string)dr["cpr"];
            Password = (string)dr["password"];
            Created = (DateTime)dr["created_at"];
            Start = (DateTime)dr["start"];
        }

        public override DataRow FillRow(DataRow dr)
        {
            dr["name"] = Name;
            dr["start"] = Start;
            dr["cpr"] = CPR;
            dr["education_id"] = Education.ID;
            dr["password"] = Password == null ? CPR : Password;

            if (Created == DateTime.MinValue)
                dr["created_at"] = DateTime.Now;
            else
                dr["created_at"] = Created;

            return dr;
        }

        public override void AfterCreate()
        {
            Education.Students.Add(this);
        }

        public override void AfterDelete()
        {
            Education.Students.Remove(this);
        }

        public override void BeforeSave()
        {
        }

        public override void AfterSave()
        {
            if (initialEducation != Education)
            {
                initialEducation.Students.Remove(this);
                Education.Students.Add(this);
                initialEducation = Education;
            }
        }

        public override void BeforeCreate()
        {
        }

        public override void BeforeDelete()
        {
            foreach(Attempt a in Attempts)
                a.Delete();
        }

        public bool IsValid()
        {
            if (String.IsNullOrWhiteSpace(Name) || !IsValidCPR(CPR) || Start.Year < 2000)
                return false;
            return true;
        }

        public static bool IsValidCPR(string cpr)
        {
            bool valid = false;

            if (!String.IsNullOrWhiteSpace(cpr) && Regex.IsMatch(cpr, @"^\d+$") && cpr.Length == 10)
            {
                // For testing purposes to eliminate need of real cprs
                if (cpr.Substring(0, 2) == "00")
                    return true;

                int result = (4 * cpr[0].ToInt())
                    + (3 * cpr[1].ToInt())
                    + (2 * cpr[2].ToInt())
                    + (7 * cpr[3].ToInt())
                    + (6 * cpr[4].ToInt())
                    + (5 * cpr[5].ToInt())
                    + (4 * cpr[6].ToInt())
                    + (3 * cpr[7].ToInt())
                    + (2 * cpr[8].ToInt())
                    + cpr[9].ToInt();

                valid = result % 11 == 0;
            }

            return valid;
        }
    }

    public static class CharExtension
    {
        public static int ToInt(this char c)
        {
            string s = new string(new char[]{c});
            return int.Parse(s);
        }
    }
}
