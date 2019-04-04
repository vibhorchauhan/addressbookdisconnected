using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class DataAccessClass
    {
        SqlConnection con;
        SqlDataAdapter da;
        SqlCommandBuilder l;
        DataSet ds = new DataSet();
        int id;

        void FillDataSet()
        {
            con = GetConnection();
            da = new SqlDataAdapter("select * from addressbook", con);
            da.MissingSchemaAction = MissingSchemaAction.AddWithKey;
            l = new SqlCommandBuilder(da);
            da.Fill(ds);
        }
        public int Bindid()
        {
            using (con = GetConnection())
            {
                using (SqlCommand com = new SqlCommand("SELECT TOP 1 address_id FROM addressbook ORDER BY First_Name DESC ", con))
                {
                    con.Open();
                    id = (int)com.ExecuteScalar();
                    con.Close();

                }

                return id;
            }
        }
        SqlConnection GetConnection()
        {
            return (new SqlConnection(@"data source = DESKTOP-N18RV7J; initial catalog = address_book; Integrated security = true;"));
        }
        //Return List
        public DataTable AddressBookList()
        {
            FillDataSet();

            DataTable dt = ds.Tables[0];
            return dt;
        }
        //insert
        public void Insert(BusinessEntityLayer.BusinessEntityClass obj)
        {
            FillDataSet();
            DataRow dr = ds.Tables[0].NewRow();

            dr["First_Name"] = obj.First_Name;
            dr["Last_Name"] = obj.Last_Name;
            dr["Email"] = obj.Email;

            dr["Phone_No"] = obj.Phone;

            ds.Tables[0].Rows.Add(dr);
            da.Update(ds);
        }

        //Update

        public void Update(BusinessEntityLayer.BusinessEntityClass obj)
        {
            FillDataSet();

            DataRow dr = ds.Tables[0].Rows.Find(obj.Address_Id);
            dr["First_Name"] = obj.First_Name;
            dr["Last_Name"] = obj.Last_Name;
            dr["Email"] = obj.Email;
            dr["Phone_No"] = obj.Phone;
            da.Update(ds);
        }
        //Delete

        public void Delete(int id)
        {
            FillDataSet();
            ds.Tables[0].Rows.Find(id).Delete();
            da.Update(ds);
        }
        //findName
        public DataRow FindName(string Name)
        {
            con = GetConnection();
            da = new SqlDataAdapter("select * from addressbook", con);

            l = new SqlCommandBuilder(da);
            da.Fill(ds);
            DataTable dt = ds.Tables[0];
            DataTable tbl = dt;

            tbl.PrimaryKey = new DataColumn[] { tbl.Columns["Last_Name".ToLower()] };
            DataRow currentRow = tbl.Rows.Find(Name);
           

            return currentRow;
       

        }


    }
}
