using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RunGroupWebApp.Helper;
using RunGroupWebApp.Interface;
using RunGroupWebApp.Models;
using RunGroupWebApp.ViewModels;
using System.Diagnostics;
using System.Globalization;
using System.Net;
using System.Net.NetworkInformation;

namespace RunGroupWebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IClubRepository _clubRepository;

        public HomeController(ILogger<HomeController> logger, IClubRepository clubRepository)
        {
            _logger = logger;
            _clubRepository = clubRepository;
        }

        public async Task<IActionResult> Index()
        {
            var ipInfo = new IPInfo();
            var homeModel = new HomeViewModel();
            try
            {
                string url = "http://ip-api.com/json/5.2.184.210";
                var info = new WebClient().DownloadString(url);
                ipInfo = JsonConvert.DeserializeObject<IPInfo>(info);
                RegionInfo myRI = new RegionInfo(ipInfo.CountryCode);
                ipInfo.Country = myRI.EnglishName;
                homeModel.City = ipInfo.City;
                homeModel.State = ipInfo.Region;
                if(homeModel.City != null)
                {
                    homeModel.Clubs = await _clubRepository.GetClubByCity(homeModel.City);
                }
                else
                {
                    homeModel.Clubs = null;
                }
                return View(homeModel);
            }
            catch (Exception ex)
            {
                homeModel.Clubs = null;
            }
            return View(homeModel);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}