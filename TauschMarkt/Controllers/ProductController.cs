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
            //ViewBag.Message = "Your contact page.";
            try
            {
                string myConnectionString = "SERVER=85.10.205.173;" +
                            "DATABASE=tauschmarkt;" +
                            "UID=flomiroesser;" +
                            "PASSWORD=flomiroesser;";

                MySqlConnection connection = new MySqlConnection(myConnectionString);
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
                connection.Close();
                return View(art);
            }
            catch (Exception ex)
            {
                
            }

            return View();
           
        }
    }
}