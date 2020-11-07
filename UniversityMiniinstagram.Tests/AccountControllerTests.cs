using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using UniversityMiniinstagram.Services.Interfaces;
using UniversityMiniinstagram.Services.Services;
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
            AccountController accountController = new AccountController(accountS.Object, postS.Object, host.Object);
            var result = await accountController.CreateRolePost();
            var viewResult = Assert.IsAssignableFrom<BadRequestResult>(result);
        }
    }
}
