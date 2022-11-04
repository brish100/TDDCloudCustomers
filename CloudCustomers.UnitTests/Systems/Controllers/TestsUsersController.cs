using CloudCustomers.API.Controllers;
using CloudCustomers.API.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;

namespace CloudCustomers.UnitTests.Systems.Controllers;

public class TestsUsersController
{
    [Test]
    public async Task Get_OnSuccess_ReturnsStatusCode200()
    {
        //Arrange
        var mockUserService = new Mock<IUsersService>();
        var sut = new UsersController(mockUserService.Object);

        //Act
        var result = (OkObjectResult)await sut.Get();

        //Assert
        Assert.That(result.StatusCode.Equals(200));
    }

    [Test]
    public async Task Get_OnSuccess_InvokeUserServiceExactlyOnce()
    {
        //Arrange
        var mockUsersService = new Mock<IUsersService>();
        mockUsersService
            .Setup(service => service.GetAllUsers())
            .ReturnsAsync(new List<User>());

        var sut = new UsersController(mockUsersService.Object);

        //Act
        await sut.Get();

        //Assert
        mockUsersService
            .Verify(service => service.GetAllUsers(),
                Times.Once()
            );
    }

    [Test]
    public async Task Get_OnSuccess_ReturnsListOfUsers()
    {
        //Arrange
        var mockUsersService = new Mock<IUsersService>();
        mockUsersService
            .Setup(service => service.GetAllUsers())
            .ReturnsAsync(new List<User>());

        var sut = new UsersController(mockUsersService.Object);

        //Act
        var result = await sut.Get();

        //Assert
        Assert.IsInstanceOf<OkObjectResult>(result);
        var objectResult = (OkObjectResult) result;
        Assert.IsInstanceOf<List<User>>(objectResult.Value);
    }

    [TestCase("1994-12-28", true)]
    [TestCase("1899-12-28", true)]
    [TestCase("19949-12-28", false)]
    [TestCase("1994-12-44", false)]
    [TestCase("1990-14-12", false)]
    public async Task Get_OnSuccess_ReturnsValidBirthDay(string birthDay, bool shouldSucceed) {
        DateTime? res = null;
        try {
            res = DateTime.Parse(birthDay);
            Assert.That(shouldSucceed.Equals(true));
            Assert.IsNotNull(res);
        } catch {
            Assert.That(shouldSucceed.Equals(false));
            Assert.IsNull(res);
        }
    }
}
