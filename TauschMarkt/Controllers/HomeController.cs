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
        String currentUser;
        Boolean logedIn = false;

        public ActionResult Index()
        {
            using (MySqlConnection connection = new MySqlConnection("Server=e50073-mysql.services.easyname.eu;Port=3306;Uid=u59498db9;Pwd=6lfqhupg;Database=u59498db9;"))
            {
                try
                {
                    connection.Open();
                    MySqlCommand command = connection.CreateCommand();


                    command.CommandText = $"SELECT id, Name, Preis, kategorie_id, status, beschreibung FROM artikel LIMIT 0,6";
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
                catch(Exception e)
                {
                    Console.WriteLine(e.Message);
                    return null;
                }              
            }
        }

        public ActionResult MeinTauschmarkt()
        {
            currentUser = User.Identity.Name.ToString();

            ViewBag.logedInUser = currentUser;

            if (logedIn != null)
            {

            }

            ViewBag.isLogedIn = logedIn;

            using (MySqlConnection connection = new MySqlConnection("Server=e50073-mysql.services.easyname.eu;Port=3306;Uid=u59498db9;Pwd=6lfqhupg;Database=u59498db9;"))
            {
                try
                {
                    connection.Open();
                    MySqlCommand command = connection.CreateCommand();


                    command.CommandText = $"SELECT id, Name, Preis, kategorie_id, status, beschreibung FROM artikel WHERE user_id='" +currentUser+"'";
                    var reader = command.ExecuteReader();
                    List<Artikel> artikel = new List<Artikel>();
                    while (reader.Read())
                    {
                        Artikel art = new Artikel();
                        art.id = reader["id"].ToString();
                        art.Preis = reader["Preis"].ToString();
                        art.Name = reader["Name"].ToString();
                        art.beschreibung = reader["beschreibung"].ToString();
                        artikel.Add(art);
                    }

                    return View(artikel);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    return null;
                }
            }



            return View();
        }


        public ActionResult ProductPicture(int id)
        {
            using (MySqlConnection connection = new MySqlConnection("Server=e50073-mysql.services.easyname.eu; Port=3306; Database=u59498db9; Uid=u59498db9; Pwd=6lfqhupg;"))
            {
                try
                {
                    connection.Open();
                    MySqlCommand command = connection.CreateCommand();
                    command.CommandText = $"SELECT picture FROM artikel WHERE id = {id}";

                    var reader = command.ExecuteReader();
                    byte[] picString = new byte[] { };
                    if (reader.Read())
                    {
                        if (DBNull.Value.Equals(reader["picture"]))
                        {
                            picString = System.IO.File.ReadAllBytes(Server.MapPath("~/") + "\\Media\\main.png");
                        }
                        else
                        {
                            picString = (byte[])reader["picture"];
                        }


                    }
                    reader.Close();
                    connection.Close();
                    Response.AppendHeader("Content-Type", "image/jpeg");

                    return File(picString, "image/jpeg");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    return null;
                }
                

            }


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