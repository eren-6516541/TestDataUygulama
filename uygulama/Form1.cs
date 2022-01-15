using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Dapper;
namespace uygulama
{
    public partial class Form1 : Form
    {
        Fonksiyonlar vt = new Fonksiyonlar();
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Listele();
           
        }

        void Listele()// verileri db den gride doldurup gerekli işlemler yapan fonksiyon
        {
            //DateTime tar1 = txttar1.DateTime;
            //DateTime tar2 = txttar2.DateTime;


            Fonksiyonlar vt = new Fonksiyonlar();//bu fonksiyon  veritabanına bağlanmak için kendim hazırladığım clastır.
            var s1 = $@"SELECT St1.ID,EvrakNo,CONVERT(VARCHAR(15), CAST(Tarih - 2 AS datetime) , 104)as Tarih,St1.MalKodu,Miktar,Fiyat,Tutar,GirisMiktar,CikisMiktar,Stok,

CASE
    WHEN IslemTur = 1 THEN 'Giris'
    WHEN IslemTur = 0 THEN 'Cikis'

    ELSE 'YOK'
END AS IslemTur



FROM STI as St1
left join
(Select ID, Case When IslemTur=1 Then Miktar else 0   end as GirisMiktar  from STI) as St2
on(St1.ID=St2.ID)

left join
(Select ID, Case When IslemTur=0 Then Miktar else 0   end as CikisMiktar  from STI) as St3
on(St1.ID=St3.ID)

left join
(Select ID, Case When IslemTur=1 Then 0 when IslemTur=0 then 0 else 0   end as Stok  from STI) as St4
on(St1.ID=St4.ID)

left join
(Select * from STK) as Stk
on(Stk.MalKodu=St1.MalKodu)  
  order by ID";

            //dapper ve model yöntemi ile veriler hafızıya alınıp filtreleme yapıldı...
            var list = vt.Baglan().Query<ClsModels>(s1);//.Where(a => a.Tarih >= tar1 && a.Tarih <= tar2).Where(a => a.MalAdi.Contains(txtara.Text.ToUpper()));//Dapper ile filtreleme kodu
            
            decimal stok = 0;
            // burada IslemTurune göre stok hesaplama yapar
            foreach (var item in list)
            {
                if (item.IslemTur == "Giris")
                    stok = stok + Convert.ToDecimal(item.Miktar);

                if (item.IslemTur == "Cikis")
                    stok = stok - Convert.ToDecimal(item.Miktar);

                item.Stok = stok;
            }
            gridControl1.DataSource = list;

            
        }
        void Sp_Stok()//store procedure çağırma
        {
            DateTime dt = Convert.ToDateTime(dateEdit1.EditValue);
            int txtedit1 = Convert.ToInt32(dt.ToOADate());

            DateTime dt3 = Convert.ToDateTime(dateEdit2.EditValue);
            int txtedit2 = Convert.ToInt32(dt3.ToOADate());
            try
            {
                DataTable dt2 = new DataTable();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = vt.Baglan();
                cmd.CommandText = "stok";
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@basTar", txtedit1 );
                cmd.Parameters.AddWithValue("@bitTar", txtedit2);
                cmd.Parameters.AddWithValue("@Malkodu", textEdit1.Text);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt2);
                gridControl1.DataSource = dt2;

            }
            catch (Exception)
            {
                Listele();
                throw;
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {

            Sp_Stok();


        }

        private void textEdit1_KeyDown(object sender, KeyEventArgs e)
        {
           



        }

        private void textEdit1_TextChanged(object sender, EventArgs e)
        {
             
        }

        private void textEdit1_EditValueChanged(object sender, EventArgs e)
        {

        }

        private void textEdit1_EditValueChanged_1(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            saveFileDialog1.Title = "Kaydet";

            saveFileDialog1.DefaultExt = "xls";

            //tablonun adı Stoklar olduğu için onunla başladı ve sonuna bugünün tarihini yazdır.
            saveFileDialog1.FileName = "StokListesi(" + DateTime.Now.Day + "." + DateTime.Now.Month + "." + DateTime.Now.Year + ")";

            //Yalnızca XLS ve XLSX dosyalarının açılabileceğini yazdım.

            saveFileDialog1.Filter = "XLS Dosyaları (*.xls)|*.xls";

            saveFileDialog1.InitialDirectory = "c:";

            //eğer saveFileDiaolog1 açıldığında Evet’e tıklanırsa

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {

                DevExpress.XtraPrinting.XlsExportOptions _Options = new DevExpress.XtraPrinting.XlsExportOptions();

                _Options.SheetName = "Stok Listesi (" + DateTime.Now.Day + "." + DateTime.Now.Month + "." + DateTime.Now.Year + ")";

                gridView1.ExportToXls(saveFileDialog1.FileName, _Options);

                if (MessageBox.Show("Aktarılan dosyayı şimdi görmek ister misiniz?", "Excel dosyası", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {

                    //Kaydedilen Excel Dosyasını açar.

                    System.Diagnostics.Process.Start(saveFileDialog1.FileName);

                }

            }

        }
    }
}
