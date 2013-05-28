using Classes.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Classes.Service
{
    public class Service
    {
        static Service instance;
        private Dao.Dao dao;
        public ObservableCollection<Attempt> Attempts { get; set; }
        public ObservableCollection<Exam> Exams { get; set; }
        public ObservableCollection<Student> Students { get; set; }
        public ObservableCollection<Education> Educations { get; set; }

        public static Service Instance
        {
            get
            {
                if (instance == null)
                    instance = new Service();

                return instance;
            }

            set { }
        }

        private Service()
        {
            dao = Dao.Dao.Instance;
            Attempts = new ObservableCollection<Attempt>();
            Exams = new ObservableCollection<Exam>();
            Students = new ObservableCollection<Student>();
            Educations = new ObservableCollection<Education>();

            UpdateLists();
        }

        public void Add(Model.Model m)
        {
            switch (m.Table)
            {
                case "Educations":
                    Educations.Add((Education)m);
                    break;
                case "Students":
                    Students.Add((Student)m);
                    break;
                case "Exams":
                    Exams.Add((Exam)m);
                    break;
                case "Attempts":
                    Attempts.Add((Attempt)m);
                    break;
            }

            DataRow dr = dao.Data.Tables[m.Table].NewRow();
            m.FillRow(dr);
            dao.Data.Tables[m.Table].Rows.Add(dr);
            dao.UpdateTable(m.Table);
            m.Data = dr;
            m.AfterCreate();
        }

        public void Remove(Model.Model m)
        {
            m.Delete();
            switch (m.Table)
            {
                case "Educations":
                    Educations.Remove((Education)m);
                    break;
                case "Students":
                    Students.Remove((Student)m);
                    break;
                case "Exams":
                    Exams.Remove((Exam)m);
                    break;
                case "Attempts":
                    Attempts.Remove((Attempt)m);
                    break;
            }

            dao.UpdateData();
            m.AfterDelete();
        }

        public void Reset(Model.Model m)
        {
            m.CancelChanges();
            switch (m.Table)
            {
                case "Students":
                    Student s = (Student)m;
                    s.Education = (from ed in Educations where ed.ID == (int)s.Data["education_id"] select ed).First();
                    break;
                case "Exams":
                    Exam e = (Exam)m;
                    e.Education = (from ed in Educations where ed.ID == (int)e.Data["education_id"] select ed).First();
                    break;
            }
        }

        public void Update(Model.Model m)
        {
            m.BeforeSave();
            m.Update();
            UpdateTable(m.Table);
            m.AfterSave();

        }

        public Attempt RegisterForExam(Student s, Exam e)
        {
            Attempt a = new Attempt(e, s);
            Add(a);

            return a;
        }

        public bool IsUniqueCPR(string cpr)
        {
            return (from s in Students where s.CPR == cpr select s).Count() == 0;
        }

        public void UpdateTable(string table)
        {
            dao.UpdateTable(table);
        }

        public void UpdateLists()
        {
            Educations.Clear();
            Students.Clear();
            Exams.Clear();
            Attempts.Clear();
            foreach (DataRow dr in dao.Data.Tables["Educations"].Rows)
            {
                Education e = new Education(dr);
                Educations.Add(e);

                Students.AddRange(e.Students);
                Exams.AddRange(e.Exams);
            }

            foreach (DataRow dr in dao.Data.Tables["Attempts"].Rows)
            {
                Student s = (from stud in Students where stud.ID == (int)dr["student_id"] select stud).First();
                Exam e = (from ex in Exams where ex.ID == (int)dr["exam_id"] select ex).First();

                Attempts.Add(new Attempt(dr, e, s));
            }
        }
    }

    public static class Extensions{
        public static void AddRange<T>(this ObservableCollection<T> col, ICollection<T> inc)
        {
            foreach (T it in inc)
                col.Add(it);
        }
    }
}
