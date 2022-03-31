namespace Sat.Recruitment.Api.Controllers
{
    using Business;
    using Entities;
    using Microsoft.AspNetCore.Mvc;
    using System.Collections.Generic;

    [ApiController]
    [Route("[controller]")]
    public partial class UsersController : ControllerBase
    {
        private readonly IUserBusiness _userBusiness;
        public UsersController(IUserBusiness userBusiness) => _userBusiness = userBusiness;
        public UsersController() { }

        [HttpPost]
        [Route("/create-user")]
        public Result CreateUser(string name, string email, string address, string phone, string userType, string money)
        {
            User user = new User()
            {
                Name = name,
                Email = email,
                Address = address,
                Phone = phone,
                UserType = userType,
                Money = decimal.Parse(money)
            };
            return _userBusiness.CreateUser(user);
        }
    }
}
