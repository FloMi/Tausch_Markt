using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TauschMarkt.Models;

namespace TauschMarkt.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            MySqlConnection connection = new MySqlConnection("server=localhost;database=tauschmarkt;uid=root;password=");
            connection.Open();
            MySqlCommand command = connection.CreateCommand();


            command.CommandText = $"SELECT id, Name, Preis, kategroie_id, status, beschreibung FROM artikel LIMIT 0,6";
            var reader = command.ExecuteReader();
            List<Artikel> lohl = new List<Artikel>();
            while (reader.Read())
            {
                Artikel art = new Artikel();
                art.id = reader["id"].ToString();
                art.Preis = reader["Preis"].ToString();
                art.Name = reader["Name"].ToString();
                art.beschreibung = reader["beschreibung"].ToString();
                lohl.Add(art);
            }
            


            return View(lohl);
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

        public ActionResult ProductPicture(int id)
        {
            MySqlConnection connection = new MySqlConnection("server=localhost;database=tauschmarkt;uid=root;password=");
            connection.Open();
            MySqlCommand command = connection.CreateCommand();
            command.CommandText = $"SELECT picture FROM artikel WHERE id = {id}";

            var reader = command.ExecuteReader();
            byte[] picString = new byte[] { };
            if (reader.Read())
            {
                if (DBNull.Value.Equals(reader["picture"]))
                {
                    picString = System.IO.File.ReadAllBytes(Server.MapPath("~/")+"\\Media\\main.png");
                }
                else
                {
                    picString = (byte[])reader["picture"];
                }
                
               
            }
            connection.Close();
            Response.AppendHeader("Content-Type", "image/jpeg");


            return File(picString, "image/jpeg");
        }

        public Image Base64ToImage(byte[] imageBytes)
        {
            //byte[] imageBytes = Convert.FromBase64String(base64string);
            using (var ms = new MemoryStream(imageBytes, 0, imageBytes.Length))
            {
                Image image = Image.FromStream(ms, true);
                return image;
            }
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}