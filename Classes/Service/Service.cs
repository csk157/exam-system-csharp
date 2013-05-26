using Classes.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Classes.Service
{
    public class Service
    {
        static Service instance;
        private Dao.Dao dao;
        public List<Attempt> Attempts { get; set; }
        public List<Exam> Exams { get; set; }
        public List<Student> Students { get; set; }
        public List<Education> Educations { get; set; }

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
            Attempts = new List<Attempt>();
            Exams = new List<Exam>();
            Students = new List<Student>();
            Educations = new List<Education>();

            UpdateLists();
        }

        public void UpdateLists()
        {
            foreach (DataRow dr in dao.Data.Tables["Educations"].Rows)
            {
                Education e = new Education(dr);
                Educations.Add(e);

                Students.AddRange(e.Students);
                Exams.AddRange(e.Exams);
            }

            foreach (DataRow dr in dao.Data.Tables["Attempts"].Rows)
            {
                Student s = (from stud in Students where stud.ID == int.Parse(dr["student_id"] as string) select stud).First();
                Exam e = (from ex in Exams where ex.ID == int.Parse(dr["exam_id"] as string) select ex).First();

                Attempts.Add(new Attempt(dr, e, s));
            }
        }


        //private Education BuildEducation(DataRow dr)
        //{

        //}
    }
}
