using Microsoft.AspNetCore.Mvc;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Leet_Translator.DataLayer;
using Leet_Translator.Models;
using System.Transactions;

namespace Leet_Translator.Controllers
{
    public class TranslateController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly TranslatioDL dal;

        Uri baseAddress = new Uri("https://api.funtranslations.com");

        public TranslateController(TranslatioDL _dal)
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = baseAddress;
            dal = _dal;
        }

        public IActionResult TranslateingPage()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> TranslateingPage(string OrginalText)
        {
            try
            {
                var response = await _httpClient.GetAsync("https://api.funtranslations.com/translate/leetspeak.json?text=" + OrginalText);

                if (response.IsSuccessStatusCode)
                {
                    var data =  response.Content.ReadAsStringAsync().Result;
                    var transResponse = JsonConvert.DeserializeObject<TranslationsResponse>(data);

                    var finalTranslation = transResponse.contents.translated;
                    var translatedTo = transResponse.contents.translation;

                    //Saving to DB
                    Translations translation = new Translations();
                    translation.OrginalText = OrginalText;
                    translation.TranslatedText = finalTranslation;
                    translation.TranslatedTo = translatedTo;

                    dal.RegisterTranslation(translation);
                    
                    return Json(transResponse.contents); 
                   
                }
                else
                {
                    return Json(new { error = "Translation failed" }); 

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                return Json(new { error = "Internal Server Error" }); 
            }
        }

    }

    public class TranslationsResponse
    {
        public Success Success { get; set; }
        public Contents contents { get; set; }
    }

    public class Success
    {
        public int Total { get; set; }
    }

    public class Contents
    {
        public string translated { get; set; }
        public string Text { get; set; }
        public string translation { get; set; }
    }

}
