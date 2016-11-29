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
                picString = (byte[])reader["picture"];
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