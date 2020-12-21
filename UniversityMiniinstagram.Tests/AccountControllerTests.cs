using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Moq;
using UniversityMiniinstagram.Services.Interfaces;
using UniversityMiniinstagram.Web.Controllers;
using Xunit;

namespace UniversityMiniinstagram.Tests
{
    public class AccountControllerTests
    {
        [Fact]
        public async void IsBadReqResult()
        {
            var accountS = new Mock<IAccountService>();
            accountS.Setup(service => service.IsAdminCreated());
            var postS = new Mock<IPostService>();
            var host = new Mock<IWebHostEnvironment>();
            var accountController = new AccountController(accountS.Object, postS.Object, host.Object);
            IActionResult result = await accountController.CreateRolePost();
            BadRequestResult viewResult = Assert.IsAssignableFrom<BadRequestResult>(result);
        }
    }
}
