using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;

namespace uygulama

{
    public class Fonksiyonlar
    {
        public SqlConnection Baglan()
        {
            SqlConnection baglanti = new SqlConnection(@"Server=BYALKANS\SQLEXPRESS;Database=TestData;integrated Security=true;connection timeout=0;");
            if (baglanti.State != System.Data.ConnectionState.Open)
            {
                try
                {

                    baglanti.Open();
                    SqlConnection.ClearPool(baglanti);

                }
                catch (SqlException)
                {
                }

            }

            return baglanti;

        }

        public int cmd(string sqlcumle)
        {
            SqlConnection baglan = this.Baglan();
            SqlCommand cmd = new SqlCommand(sqlcumle, baglan);


            int sonuc = 0;
            try
            {
                sonuc = cmd.ExecuteNonQuery();
                cmd.CommandTimeout = 0;
            }
            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }
            cmd.CommandTimeout = 0;
            cmd.Dispose();
            baglan.Close();
            baglan.Dispose();

            return (sonuc);
        }

        public DataTable GetDataTable(string sql)
        {
            SqlConnection baglan = this.Baglan();
            SqlDataAdapter adap = new SqlDataAdapter(sql, baglan);
            DataTable dt = new DataTable();
            try
            {
                adap.Fill(dt);

            }
            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }
            adap.Dispose();
            baglan.Close();

            return dt;

        }
        

        internal DataTable GetDataTable()
        {
            throw new NotImplementedException();
        }

        public string GetDataCell(string sql)
        {
            DataTable dt = GetDataTable(sql);
            if (dt.Rows.Count == 0) return null;
            return dt.Rows[0][0].ToString();

        }
    }
}