using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using RunGroupWebApp.Controllers;
using RunGroupWebApp.Interface;
using RunGroupWebApp.Models;
using RunGroupWebApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RunGroupWebApp.Test.ControllerTest
{
    public class ClubControllerTest
    {
        private ClubController _clubController;
        private IClubRepository _clubRepository;
        private IPhotoService _photoService;
        private HttpContextAccessor _httpContextAccessor;
        public ClubControllerTest()
        {
            //Dependencies
            _clubRepository = A.Fake<IClubRepository>();
            _photoService = A.Fake<IPhotoService>();
            _httpContextAccessor = A.Fake<HttpContextAccessor>();


            //SUT
            _clubController = new ClubController(_clubRepository,_photoService,_httpContextAccessor);
        }

        [Fact]
        public void ClubControllerTest_Index_ReturnsSuccess()
        {
            //Arrange - What do I need to bring in?
            var clubs = A.Fake<IEnumerable<Club>>();
            A.CallTo(() => _clubRepository.GetClubs()).Returns(clubs);
            //Act
            var result = _clubController.Index();
            //Assert - Object check actions
            result.Should().BeOfType<Task<IActionResult>>();


        }

        [Fact]
        public void ClubController_Detail_ReturnsSuccess()
        {
            //Arrange
            var id = 1;
            var club = A.Fake<Club>();
            A.CallTo(() => _clubRepository.GetClubByIdAsync(id)).Returns(club);

            //Act
            var result = _clubController.Detail(id);

            //Assert
            result.Should().BeOfType<Task<IActionResult>>();
        }

        [Fact]
        public void ClubController_Create_ReturnsSuccess()
        {
            var path = "C:\\Users\\razva\\Downloads\\Razvan.png";
            using (var stream = System.IO.File.OpenRead(path))
            {
                var newClub = new CreateClubViewModel
                {
                    Id = 1,
                    Title = "Title",
                    Description = "Description",
                    Image = new FormFile(stream, 0, stream.Length, null, Path.GetFileName(stream.Name)),
                    Address = new Address
                    {
                        City = "City",
                        State = "State",
                        Street = "Street"
                    },
                };

                var result = _clubController.Create(newClub);

                //var okResult = Assert.IsType<OkObjectResult>(result);

                result.Should().BeOfType<Task<IActionResult>>();

            }
        }
    }
}
