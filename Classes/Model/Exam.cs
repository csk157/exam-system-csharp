using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Classes.Model
{
    public class Exam
    {
        public int ID { get; set; }
        public String Title;
        public Education Education { get; set; }
        public List<Attempt> Attempts { get; set; }
        public DateTime Created { get; set; }

        public Exam()
        {
            Attempts = new List<Attempt>();
        }

        public Exam(DataRow dr, Education ed)
            : this()
        {
            ID = (int)dr["id"];
            Title = (string)dr["title"];
            Created = (DateTime) dr["created_at"];
            Education = ed;
        }
    }
}
