using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TauschMarkt.Models
{
    public class ArtikelKategorie
    {
        public Artikel art { get; set; }
        public Kategorie kat { get; set; }
    }
}