using Microsoft.AspNetCore.Mvc;
using RunGroupWebApp.Data;
using RunGroupWebApp.Interface;
using RunGroupWebApp.Models;
using RunGroupWebApp.Repository;
using RunGroupWebApp.ViewModels;
using System.Security.Claims;

namespace RunGroupWebApp.Controllers
{
    public class DashboardController : Controller
    {
        private readonly IDashboardRepository _dashboardRepository;
        private readonly IHttpContextAccessor _httpContext;
        private readonly IPhotoService _photoService;

        public DashboardController(IDashboardRepository dashboardRepository, IHttpContextAccessor httpContext, IPhotoService photoService)
        { 
            _dashboardRepository = dashboardRepository;
            _httpContext = httpContext;
            _photoService = photoService;
        }
        public async Task<IActionResult> Index()
        {
            var userRaces = await _dashboardRepository.GetAllUserRaces();
            var userClubs = await _dashboardRepository.GetAllUserClubs();
            var dashboardViewModel = new DashboardViewModel()
            {
                Races = userRaces,
                Clubs = userClubs
            };
            return View(dashboardViewModel);
        }

        public async Task<IActionResult> EditUserProfile()
        {
            var curUserId = _httpContext.HttpContext.User.GetUserId();
            var user = await _dashboardRepository.GetUserById(curUserId);
            if(user == null)
            {
                return View("Error");
            }
            var editUserViewModel = new EditUserDashboardViewModel()
            {
                Id = curUserId,
                Pace = user.Pace,
                Mileage = user.Mileage,
                ProfileImageUrl = user.ProfileImageUrl,
                City = user.City,
                State = user.State

            };
            return View(editUserViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> EditUserProfile(EditUserDashboardViewModel editUserVM)
        {
            
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Failed to edit profile");
                return View("EditUserProfile", editUserVM);
            }
            
            var user = await _dashboardRepository.GetByIdNoTracking(User.GetUserId());
            if(user.ProfileImageUrl == null || user.ProfileImageUrl == null)
            {
                var photoResult = await _photoService.AddPhotoAsync(editUserVM.Image);
                user.ProfileImageUrl = photoResult.Url.ToString();
                user.Mileage = editUserVM.Mileage;
                user.ProfileImageUrl = photoResult.Url.ToString();

                user.City = editUserVM.City;
                user.State = editUserVM.State;
               
                _dashboardRepository.Update(user);
                return RedirectToAction("Index");
            }
            else
            {
                try
                {
                    await _photoService.DeletePhotoAsync(user.ProfileImageUrl);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Could not delete photo");
                    return View(editUserVM);
                }
                var photoResult = await _photoService.AddPhotoAsync(editUserVM.Image);
                var userEdit = new AppUser()
                {
                    Id = user.Id,
                    Mileage = user.Mileage,
                    ProfileImageUrl = photoResult.Url.ToString(),
                    City = user.City,
                    State = user.State
                };
                _dashboardRepository.Update(userEdit);
                return RedirectToAction("Index");
            }
        }
    }
}
