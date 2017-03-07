using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using TauschMarkt.Models;

namespace TauschMarkt.Controllers
{
    public class HomeController : Controller
    {
        public static String currentUser;

        public ActionResult Index()
        {
            currentUser = User.Identity.Name.ToString();

            Session["isLoggedIn"] = AccountController.logedIn;

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
                        kat.id = reader3["id"].ToString();
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

                    int s = rnd.Next(3, seas.Count() - 3);


                    ViewBag.bild1 = seas[s];
                    ViewBag.bild2 = seas[s + 1];
                    ViewBag.bild3 = seas[s + 2];

                    return View(lohl);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    return null;
                }
            }

        }



        public ActionResult MeinTauschmarkt()
        {
            Session["isLoggedIn"] = AccountController.logedIn;

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
                    if ((Boolean)Session["isLoggedIn"])
                    {
                        return View(artikel);
                    }
                    else
                    {
                        return View("NotLoggedIn");
                    }

                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    return null;
                }
            }
        }
        [ValidateInput(false)]
        public ActionResult SendMessage(string nachricht, string productId, string name)
        {

            using (MySqlConnection connection = new MySqlConnection("Server=e50073-mysql.services.easyname.eu;Port=3306;Uid=u59498db9;Pwd=6lfqhupg;Database=u59498db9;"))
            {
                try
                {
                    connection.Open();
                    MySqlCommand command = connection.CreateCommand();


                    command.CommandText = $"SELECT id, Name, Preis, kategorie_id, status, beschreibung, user_id FROM artikel WHERE id='" + productId + "'";
                    var reader = command.ExecuteReader();
                    Artikel art = new Artikel();
                    var productOwnerMail = "";
                    while (reader.Read())
                    {
                        Artikel tempArt = new Artikel();
                        art.id = reader["id"].ToString();
                        art.Preis = reader["Preis"].ToString();
                        art.Name = reader["Name"].ToString();
                        art.beschreibung = reader["beschreibung"].ToString();
                        productOwnerMail = reader["user_id"].ToString();
                    }


                    using (MailMessage mm = new MailMessage("adrian.roesser@gmail.com", productOwnerMail))
                    {

                        mm.Subject = "Kaufanfrage: "+art.Name; //Betreff
                        string standardText = "Kaufanfrage für " + art.Name + " von " + name +" um "+art.Preis + "€\r\n\r\n";
                        string linie = "\r\n---------------------------------------------------------------------------------\r\n\r\n";
                        string ruckmeldung = "\r\nFür Rückmeldungen bitte E-Mail an: " + User.Identity.Name;
                        mm.Body = standardText + linie+ nachricht + linie + ruckmeldung; //Zusätzliche Nachricht
                        mm.IsBodyHtml = false; //html formatieren
                        SmtpClient smtp = new SmtpClient(); //noch smtp server von mir verwenden (google)
                        smtp.Host = "smtp.gmail.com";
                        smtp.EnableSsl = true;


                        smtp.UseDefaultCredentials = true;
                        smtp.EnableSsl = true;

                        NetworkCredential NetworkCred = new NetworkCredential("adrian.roesser@gmail.com", "diplomtest");
                        smtp.Credentials = NetworkCred;

                        smtp.Port = 587; //Standard Port von Gmail
                        smtp.Send(mm); //Senden
                    }
                    return View("~/Views/Product/ShopItem.cshtml", art);
                }
                catch (Exception e)
                {

                }
            }


           
            return View("");
        }





        public ActionResult ProductPicture(int id)
        {
            Session["isLoggedIn"] = AccountController.logedIn;
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
            Session["isLoggedIn"] = AccountController.logedIn;
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            Session["isLoggedIn"] = AccountController.logedIn;
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}