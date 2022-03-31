using Business;
using Entities;
using Sat.Recruitment.Api.Controllers;
using Xunit;

namespace Sat.Recruitment.Test
{
    [CollectionDefinition("Tests", DisableParallelization = true)]
    public class UnitTest1
    {
        IUserBusiness business = new UserBusiness();
        [Fact]
        public void Test1()
        {
            UsersController userController = new UsersController(business);
            Result result = userController.CreateUser("Mike", "mike@gmail.com", "Av. Juan G", "+349 1122354215", "Normal", "124");
            Assert.True(result.IsSuccess);
            Assert.Equal("User Created", result.Errors);
        }
        [Fact]
        public void Test2()
        {
            UsersController userController = new UsersController(business);
            Result result = userController.CreateUser("Agustina", "Agustina@gmail.com", "Av. Juan G", "+349 1122354215", "Normal", "124");
            Assert.False(result.IsSuccess);
            Assert.Equal("The user is duplicated", result.Errors);
        }
    }
}
