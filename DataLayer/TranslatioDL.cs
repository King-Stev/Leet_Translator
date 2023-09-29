using Leet_Translator.Controllers;
using Leet_Translator.Models;
using System.Data.SqlClient;

namespace Leet_Translator.DataLayer
{
    public class TranslatioDL
    {
        SqlConnection conn = null;
        SqlCommand command = null;
        public static IConfiguration Configuration { set;  get; }

        private string GetConnectionString()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json");
            Configuration = builder.Build();

            return Configuration.GetConnectionString("DefaultConnection");
        }
        public List<Translations> DisplayAll()
        {
            List<Translations> translationsList = new List<Translations>();   
            using(conn = new SqlConnection(GetConnectionString()))
            {
                command = conn.CreateCommand();
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.CommandText = "ReadAllTranslations";
                conn.Open();

                SqlDataReader reader =command.ExecuteReader();

                while (reader.Read())
                {
                    Translations translation = new Translations();
                    translation.Id = Convert.ToInt32(reader["Id"]);
                    translation.OrginalText = reader["Searched Text"].ToString();
                    translation.TranslatedText =reader["Translated Text"].ToString();
                    translation.TranslatedTo =reader["Translated To"].ToString();
                    translation.createdAt = Convert.ToDateTime(reader["Created on"]);
                    translationsList.Add(translation);


                }                  
                 conn.Close();
                
            }
            return translationsList;

        }

        public bool RegisterTranslation(Translations model)
        {
            int id = 0;
            using (conn = new SqlConnection(GetConnectionString()))
            {
                command = conn.CreateCommand();
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.CommandText = "InsertTranslations";
                command.Parameters.AddWithValue("@SearchedText", model.OrginalText);
                command.Parameters.AddWithValue("@TranslatedText", model.TranslatedText);
                command.Parameters.AddWithValue("@TranslatedTo", model.TranslatedTo);
                conn.Open();
                id = command.ExecuteNonQuery();
                conn.Close();

            } 

            return id > 0 ? true : false;
        }
    }
}
