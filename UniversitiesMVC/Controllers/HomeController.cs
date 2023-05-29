using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using UniversitiesMVC.Models;

namespace UniversitiesMVC.Controllers
{
    public class HomeController : Controller
    {
        public UniversityEntity ue_model { get; set;}
        public string mess = "..";
        private readonly ILogger<HomeController> _logger;

        private readonly UniversitiesContext _db;
        public HomeController(ILogger<HomeController> logger, UniversitiesContext db)
        {
            _logger = logger;
            _db = db;
            //ue_model.message = "default";
        }
        public IActionResult Privacy()
        {
            return View();
        }

        //===============================Index=======================================================================
        public IActionResult Index()
        {
            var univers = _db.Universities.ToList();
            var countries = _db.Countries.ToList();
            if (ue_model != null)
                ViewBag.Message = ue_model.message;
            //Console.WriteLine(ViewBag.Message);
            return View(univers);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateUniversity(string UniversityName, string? CountryName)
        {        
            if (ModelState.IsValid)
            {
                var univers = _db.Universities.ToList();
                var countries = _db.Countries.ToList();
                Console.WriteLine(UniversityName + CountryName);
                var country = countries.FirstOrDefault(u => u.CountryName == CountryName);
                int countryId;
                if (country == null) {
                    addNewCountry(CountryName);
                    countryId = _db.Countries.ToList().FirstOrDefault(u => u.CountryName == CountryName).Id;
                }
                else
                {
                    countryId = country.Id;
                }
                if (univers.All(s => s.UniversityName != UniversityName) && countryId != null)
                {
                    var maxId = univers.Max(s => s.Id);              
                    var newUniversity = new University
                    {
                        Id = maxId + 1,
                        UniversityName = UniversityName,
                        CountryId = countryId
                    };
                    _db.Universities.Add(newUniversity);
                    _db.SaveChanges();        
                }
                //await new Task(() => { ViewBag.mmm = "text"; });
                //ue_model.message = "Country add";
                return Ok("Add univertsity");
                //return RedirectToAction(nameof(Index));                
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteUniversity(string? UniversityName)
        {
            if (ModelState.IsValid)
            {
                var univers = _db.Universities.ToList();   
                if (univers.Any(s => s.UniversityName == UniversityName))
                {
                    University univer = univers.Find(s => s.UniversityName == UniversityName);
                    _db.Universities.Remove(univer);
                    _db.SaveChanges();
                }
                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        //===================================Countries===================================================================
        public IActionResult Countries()
        {
            var countries = _db.Countries.ToList();
            return View(countries);
        }

        private void addNewCountry(string countryName)
        {
            var countries = _db.Countries.ToList();
            var maxId = countries.Max(u => u.Id);           
            var newCountry = new Country
            {
                Id = maxId + 1,
                CountryName = countryName,
            };
            _db.Countries.Add(newCountry);
            _db.SaveChanges();
        }

        private void deleteOldCountry(string countryName)
        {
            var countries = _db.Countries.ToList();
            Country country = countries.Find(c => c.CountryName == countryName);
            var countryId = country.Id;
            var universities = _db.Universities.ToList();
            if (universities.Count(c => c.CountryId == countryId) > 0) {
                universities.ForEach(u => {
                    if (u.CountryId == countryId)
                    {
                        _db.Universities.Remove(u);
                        _db.SaveChanges();
                    }
                });
            }
            _db.Countries.Remove(country);
            _db.SaveChanges();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateCountry(string? CountryName)
        {
            if (ModelState.IsValid)
            {
                var countries = _db.Countries.ToList();
                Console.WriteLine(CountryName);
                if (CountryName!=null && countries.All(s => s.CountryName != CountryName))
                {
                    addNewCountry(CountryName);
                }
                return RedirectToAction(nameof(Countries));
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteCountry(string? CountryName)
        {
            if (ModelState.IsValid)
            {
                var countries = _db.Countries.ToList();
                if (CountryName != null && countries.Count(s => s.CountryName == CountryName)>0)
                {
                    deleteOldCountry(CountryName);
                }
                return RedirectToAction(nameof(Countries));
            }
            return View();
        }


        //====================================RankingSystems==================================================================
        public IActionResult RankingSystems()
        {
            var rsystem = _db.RankingSystems.ToList();
            return View(rsystem);

        }

        //=====================================RankingCriteria=================================================================
        public IActionResult RankingCriteria()
        {
            var rcriteria = _db.RankingCriteria.ToList();
            var rsystem = _db.RankingSystems.ToList();
            //foreach (var u in rcriteria)
            //{
            //    System.Console.Write(u.RankingSystem.SystemName);
            //}
            return View(rcriteria);
        }

        //======================================UniversityRankingYears================================================================
        public IActionResult UniversityRankingYears()
        {
            var ury = _db.UniversityRankingYears.ToList();
            var univers = _db.Universities.ToList();
            //foreach (var u in univers)
            //{
                //System.Console.Write(u.Id);
                //System.Console.Write(u.UniversityId);
                //System.Console.Write(u.UniversityName);
                //System.Console.Write(u.University.UniversityName);
            //}
            var rcriteria = _db.RankingCriteria.ToList();
            return View(ury);
        }

            //< td > @ranking.University.UniversityName </ td >
            //< td > @ranking.RankingCriteria.CriteriaName </ td >

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
    public class UniversityRankingViewModel
    {
        public string UniversityName { get; set; }
        public string CountryName { get; set; }
        public string CriteriaName { get; set; }
        public string Year { get; set; }
        public string Score { get; set; }
    }
}