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
                MySqlConnection connection = new MySqlConnection("Server=e50073-mysql.services.easyname.eu;Port=3306;Uid=u59498db9;Pwd=6lfqhupg;Database=u59498db9;");
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
            catch (Exception e)
            {
                
            }
            throw new Exception("Error while loading shop item.");
           
        }
    }
}