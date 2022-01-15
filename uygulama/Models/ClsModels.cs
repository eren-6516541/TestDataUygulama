using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace uygulama
{
    class ClsModels
    {

        public int ID { get; set; }
        public string IslemTur { get; set; }
        public string MalAdi { get; set; }
        public string EvrakNo { get; set; }
        public string Tarih { get; set; }
        public string MalKodu { get; set; }
        public decimal? Miktar { get; set; }
        public decimal? Fiyat { get; set; }
        public decimal? Tutar { get; set; }
        public string Birim { get; set; }
        public decimal GirisMiktar { get; set; }
        public decimal CikisMiktar { get; set; }
        public decimal Stok { get; set; }
    }
}
