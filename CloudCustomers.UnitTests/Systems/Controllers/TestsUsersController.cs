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

    [TestCase("2010-06-06", true)]
    [TestCase("1993-10-04", true)]
    [TestCase("1899-12-28", true)]
    [TestCase("19949-12-28", false)]
    [TestCase("1994-12-44", false)]
    [TestCase("1990-14-12", false)]
    public async Task Get_OnSuccess_ReturnsValidBirthDay(string birthDay, bool shouldSucceed) {
        // Arrange
        DateTime? res = null;

        // Assume
        try {
            res = DateTime.Parse(birthDay);
        } catch {
            // ignored
        }

        // Assert
        if (shouldSucceed) {
            Assert.IsNotNull(res);
            Assert.IsInstanceOf<DateTime>(res);
        } else {
            Assert.IsNull(res);
            Assert.IsNotInstanceOf<DateTime>(res);
        }
    }

    [TestCase("19949-12-28")]
    [TestCase("1994-12-44")]
    [TestCase("1990-14-12")]
    public async Task Get_OnFailure_ThrowsExceptionOnInvalidDate(string birthDay) {
        // Assume & Assert
        Assert.Throws<FormatException>(() => DateTime.Parse(birthDay));
    }
}
