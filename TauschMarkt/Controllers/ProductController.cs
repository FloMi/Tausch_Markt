using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.IO;
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
                MySqlConnection connection = new MySqlConnection("Server=e50073-mysql.services.easyname.eu; Port=3306; Database=u59498db9; Uid=u59498db9; Pwd=6lfqhupg;");
                connection.Open();


                MySqlCommand command = connection.CreateCommand();
                command.CommandText = $"SELECT Name, Preis, kategorie_id, status, beschreibung FROM artikel WHERE id = {id}";
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




        public ActionResult AddItem()
        {
            return View();
        }

        public string AddItemAjax(string name, string preis, string beschreibung)
        {
            
            try
            {
                MySqlConnection connection = new MySqlConnection("Server=e50073-mysql.services.easyname.eu;Port=3306;Uid=u59498db9;Pwd=6lfqhupg;Database=u59498db9;");
                

                byte[] fileData = null;

                foreach (string file in Request.Files)
                {
                    var fileContent = Request.Files[file];
                    if (fileContent != null && fileContent.ContentLength > 0)
                    {
                        using (var binaryReader = new BinaryReader(Request.Files[0].InputStream))
                            fileData = binaryReader.ReadBytes(Request.Files[0].ContentLength);
                    }
                }

                string query = "INSERT INTO artikel(Name, Preis, kategorie_id, status, picture, beschreibung, user_id) VALUES (@Name, @Preis, @KategorieId, @Status, @Picture, @Beschreibung, @Userid)";
                using (MySqlCommand cmd = new MySqlCommand(query))
                {
                    cmd.Connection = connection;
                    cmd.Parameters.AddWithValue("@Name", name);
                    cmd.Parameters.AddWithValue("@Preis", preis);
                    cmd.Parameters.AddWithValue("@KategorieId", 1);
                    cmd.Parameters.AddWithValue("@Status", 1);
                    cmd.Parameters.AddWithValue("@Picture", fileData);
                    cmd.Parameters.AddWithValue("@Beschreibung", beschreibung);
                    cmd.Parameters.AddWithValue("@Userid", User.Identity.Name.ToString());
                    connection.Open();
                    cmd.ExecuteNonQuery();
                    connection.Close();
                }
                return "ok";
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return "failed";

        }


    }
}