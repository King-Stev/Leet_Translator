using Leet_Translator.DataLayer;
using Leet_Translator.Models;
using Microsoft.AspNetCore.Mvc;

namespace Leet_Translator.Controllers
{
    public class DisplayController : Controller

    {
        private readonly TranslatioDL dal;

        public DisplayController(TranslatioDL _dal){
            dal = _dal;
        }

        [HttpGet]
        public IActionResult DisplayTranslates()
        {
            List<Translations> translations = new List<Translations>();

            try
            {
                translations = dal.DisplayAll();
            }
            catch(Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }

            return View(translations);
        }
    }
}
