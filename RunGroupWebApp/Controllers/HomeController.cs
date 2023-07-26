using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RunGroupWebApp.Data;
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
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ILocationService _locationService;

        public HomeController(ILogger<HomeController> logger, IClubRepository clubRepository, SignInManager<AppUser> signInManager, UserManager<AppUser> userManager, ILocationService locationService)
        {
            _logger = logger;
            _clubRepository = clubRepository;
            _signInManager = signInManager;
            _userManager = userManager;
            _signInManager = signInManager;
            _locationService = locationService;
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

        public IActionResult Register()
        {
            var response = new HomeUserCreativeModel();
            return View(response);
        }

        [HttpPost]
        public async Task<IActionResult> Index(HomeViewModel homeVM)
        {
            var createVM = homeVM.Register;
            if(!ModelState.IsValid)
            {
                return View(homeVM);
            }
            var user = await _userManager.FindByEmailAsync(createVM.Email);
            if(user != null)
            {
                ModelState.AddModelError("Register.Email", "This email address is already use");
                return View(homeVM);
            }
            var userLocation = await _locationService.GetCityByZipCode(createVM.ZipCode??0);
            if(userLocation == null)
            {
                ModelState.AddModelError("Register.ZipCode", "Could not find zip code!");
                return View(homeVM);
            }

            var newUser = new AppUser
            {
                UserName = createVM.UserName,
                Email = createVM.Email,
                Address = new Address()
                {
                    State = userLocation.StateCode,
                    City = userLocation.CityName,
                    ZipCode = createVM.ZipCode ?? 0
                }
            };

            var newUserResponse = await _userManager.CreateAsync(newUser,createVM.Password);
            if(newUserResponse.Succeeded)
            {
                await _signInManager.SignInAsync(newUser, isPersistent: false);
                await _userManager.AddToRoleAsync(newUser, UserRoles.User);
                
            }
            else 
            {
                ModelState.AddModelError("Register.Password", "The password must containts alfa-number, special characters and numbers!");
                return View(homeVM);
            }
            return RedirectToAction("Index", "Club");
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