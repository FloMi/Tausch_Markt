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
        public static String currentUser;

        public ActionResult Index()
        { 
            currentUser = User.Identity.Name.ToString();

            Session["isLoggedIn"] = AccountController.checkIfLoggedin();
         
            using (MySqlConnection connection = new MySqlConnection("Server=e50073-mysql.services.easyname.eu;Port=3306;Uid=u59498db9;Pwd=6lfqhupg;Database=u59498db9;"))
            {
                try
                {
                    connection.Open();
                    MySqlCommand command = connection.CreateCommand();


                    command.CommandText = $"SELECT id, Name, Preis, kategorie_id, status, beschreibung FROM artikel";
                    var reader = command.ExecuteReader();
                    List<ArtikelKategorie> lohl = new List<ArtikelKategorie>();
                    while (reader.Read())
                    {
                        ArtikelKategorie artkat = new ArtikelKategorie();
                        Artikel art = new Artikel();
                        art.id = reader["id"].ToString();
                        art.Preis = reader["Preis"].ToString();
                        art.Name = reader["Name"].ToString();
                        art.beschreibung = reader["beschreibung"].ToString();
                        artkat.art = art;
                        lohl.Add(artkat);
                    }
                    reader.Close();

                    command.CommandText = $"SELECT id,Name FROM kategorie";
                    var reader3 = command.ExecuteReader();
                    while (reader3.Read())
                    {
                        ArtikelKategorie artkat = new ArtikelKategorie();
                        Kategorie kat = new Kategorie();
                        kat.id= reader3["id"].ToString();
                        kat.Name = reader3["Name"].ToString();
                        artkat.kat = kat;
                        lohl.Add(artkat);
                    }
                    reader3.Close();

                    command.CommandText = $"SELECT id FROM artikel WHERE picture IS NOT NULL";
                    var reader2 = command.ExecuteReader();
                    List<string> seas = new List<string>();
                    while (reader2.Read())
                    {

                        string id = reader2["id"].ToString();

                        seas.Add(id);
                    }
                    reader2.Close();
                    Random rnd = new Random();

                    int s = rnd.Next(3, seas.Count()-3);
        

                    ViewBag.bild1 = seas[s];
                    ViewBag.bild2 = seas[s+1];
                    ViewBag.bild3 = seas[s+2];

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
            Session["isLoggedIn"] = AccountController.checkIfLoggedin();

            using (MySqlConnection connection = new MySqlConnection("Server=e50073-mysql.services.easyname.eu;Port=3306;Uid=u59498db9;Pwd=6lfqhupg;Database=u59498db9;"))
            {
                try
                {
                    connection.Open();
                    MySqlCommand command = connection.CreateCommand();


                    command.CommandText = $"SELECT id, Name, Preis, kategorie_id, status, beschreibung FROM artikel WHERE user_id='" + currentUser + "'";
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
        }

             



        public ActionResult ProductPicture(int id)
        {
            Session["isLoggedIn"] = AccountController.checkIfLoggedin();
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
                            picString = System.IO.File.ReadAllBytes(Server.MapPath("~/") + "\\Media\\alternative.png");
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
            Session["isLoggedIn"] = AccountController.checkIfLoggedin();
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            Session["isLoggedIn"] = AccountController.checkIfLoggedin();
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}