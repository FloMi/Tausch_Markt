using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TauschMarkt.Models;

namespace TauschMarkt.Controllers
{
    public class ProductController : Controller
    {
        // GET: Product
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ShopItem(int id)
        {
            ViewBag.Message = "Your contact page.";
            MySqlConnection connection = new MySqlConnection("server=localhost;database=tauschmarkt;uid=root;password=");
            connection.Open();
            MySqlCommand command = connection.CreateCommand();
            command.CommandText = $"SELECT Name, Preis, kategroie_id, status, beschreibung FROM artikel WHERE id = {id}";
            var reader = command.ExecuteReader();
            Artikel art = new Artikel();
            if (reader.Read())
            {
                art.Name = reader["Name"].ToString();
                art.Preis = reader["Preis"].ToString();
                art.beschreibung = reader["beschreibung"].ToString();

            }
            return View(art);
        }
    }
}