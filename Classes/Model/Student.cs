using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Classes.Model
{
    public class Student
    {
        public int ID { get; set; }
        public String Name { get; set; }
        public String CPR { get; set; }
        public Education Education { get; set; }
        public DateTime Start { get; set; }
        public DateTime Created { get; set; }
        public List<Attempt> Attempts { get; set; }

        public Student() {
            Attempts = new List<Attempt>();
        }
        public Student(DataRow dr, Education ed) : this()
        {
            ID = (int) dr["id"];
            Name = (string)dr["name"];
            CPR = (string)dr["cpr"];
            Created = (DateTime) dr["created_at"];
            Start = (DateTime) dr["start"];
            Education = ed;
        }
    }
}
