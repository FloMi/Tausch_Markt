using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TauschMarkt.Models
{
    public class Artikel
    {

        public string id { get; set; }
        public string Name { get; set; }
        public string Preis { get; set; }
        public int kategroie_id { get; set; }
        public int status { get; set; }
        public string beschreibung { get; set; }
    }
}