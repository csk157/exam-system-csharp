using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Classes.Model
{
    public abstract class Model
    {
        public int ID { get; set; }
        private string table;
        public string Table {get{return table;} }
        protected DataRow data;
        public DataRow Data
        {
            get { return data; }
            set { data = value; ReadRow(value); }
        }

        protected abstract void ReadRow(DataRow dr);
        public abstract DataRow FillRow(DataRow dr);

        public Model(string t)
        {
            table = t;
        }

        public void Create()
        {
            BeforeCreate();
        }

        public void Update()
        {
            if (data != null)
                FillRow(data);
        }

        public void Delete()
        {
            BeforeDelete();
            if (data != null)
                data.Delete();
        }

        public void CancelChanges()
        {
            Console.WriteLine("Canceling Changes");
            Console.WriteLine(data);

            if(data != null)
                ReadRow(data);
        }

        public abstract void BeforeSave();
        public abstract void AfterSave();
        public abstract void BeforeCreate();
        public abstract void AfterCreate();
        public abstract void BeforeDelete();
        public abstract void AfterDelete();
    }
}
