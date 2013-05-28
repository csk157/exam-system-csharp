using Classes.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Classes.Dao
{
    public class Dao
    {
        private SqlConnection con;
        private DataSet data;
        public DataSet Data { get { return data; } set { } }
        private Dictionary<string, SqlDataAdapter> adapters;
        private static string[] tables = {"Attempts", "Exams", "Students", "Educations"};
        private static string connStr = @"Data Source=localhost\SQLEXPRESS; database=grades_system; Integrated Security=true;";
        private static Dao instance;
        public static Dao Instance
        {
            get
            {
                if (instance == null)
                    instance = new Dao();

                return instance;
            }

            set { }
        }
        


        private Dao() {
            con = new SqlConnection(connStr);
            data = new DataSet();
            adapters = new Dictionary<string, SqlDataAdapter>();
            LoadData();

            data.Relations.Add("EducationStudents", data.Tables["Educations"].Columns["id"], data.Tables["Students"].Columns["education_id"]);
            data.Relations.Add("EducationExams", data.Tables["Educations"].Columns["id"], data.Tables["Exams"].Columns["education_id"]);
            data.Relations.Add("ExamAttempts", data.Tables["Exams"].Columns["id"], data.Tables["Attempts"].Columns["exam_id"]);
            data.Relations.Add("StudentAttempts", data.Tables["Students"].Columns["id"], data.Tables["Attempts"].Columns["student_id"]);

            data.AcceptChanges();
        }

        public void LoadData()
        {
                foreach(string table in tables){
                    string sql = "SELECT * FROM " + table;
                    AddToDataSet(table, sql, con);
                }
        }

        public void UpdateData()
        {
            foreach(string t in tables)
                adapters[t].Update(data, t);
        }

        public void UpdateTable(string table)
        {
            adapters[table].Update(data, table);
        }

        private void AddToDataSet(string table, string sql, SqlConnection con)
        {
            SqlCommand cmd = new SqlCommand(sql, con);
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            adapters.Add(table, adapter);
            SqlCommandBuilder cb = new SqlCommandBuilder(adapter);
            adapter.DeleteCommand = cb.GetDeleteCommand(true);
            adapter.UpdateCommand = cb.GetUpdateCommand(true);
            adapter.InsertCommand = cb.GetInsertCommand(true);

            adapter.Fill(data, table);

            // Define autoincrement
            data.Tables[table].Columns[0].AutoIncrement = true;
            data.Tables[table].Columns[0].AutoIncrementSeed = (data.Tables[table].Rows.Count > 0) ? ((int) data.Tables[table].Rows[data.Tables[table].Rows.Count - 1][0] +1) : 1;
            data.Tables[table].Columns[0].AutoIncrementStep = 1;
        }

        public DataSet GetAllEducations()
        {
            return Query("SELECT * FROM Educations", "Educations");
        }

        //public int InsertEducation(Education e)
        //{
        //    string q = "INSERT INTO Educations (`title`) VALUES (@title)";
        //    List<SqlParameter> prm = new List<SqlParameter>();
        //    prm.Add(new SqlParameter("@title", e.Title));

        //    return Insert(q, prm);
        //}


        private DataSet Query(string sql, string table, List<SqlParameter> parameters = null)
        {
            DataSet dt = null;
            using (SqlConnection con = new SqlConnection(connStr))
            {
                SqlCommand cmd = new SqlCommand(sql, con);

                if (parameters != null)
                    AddParamsToCmd(cmd, parameters);

                SqlDataAdapter da = new SqlDataAdapter(cmd);

                dt = new DataSet();
                da.Fill(dt, table);
            }

            return dt;
        }

        private int Insert(string sql, string table, List<SqlParameter> parameters = null)
        {
            DataSet d = null;
            using (SqlConnection con = new SqlConnection(connStr))
            {
                SqlCommand cmd = new SqlCommand(sql, con);
      
                if (parameters != null)
                    AddParamsToCmd(cmd, parameters);
               
                con.Open();
                cmd.ExecuteNonQuery();
                d = Query("SELECT SCOPE_IDENTITY", table, null);
            }

            return d != null ? int.Parse(d.Tables[table].Rows[0][0] as string) : -1;
        }

        private static void AddParamsToCmd(SqlCommand cmd, List<SqlParameter> parameters)
        {
            foreach (SqlParameter sp in parameters)
                cmd.Parameters.Add(sp);
        }
    }
}
