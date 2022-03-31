using Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business
{
    public interface IUserBusiness
    {
        Result CreateUser(User user);
    }
}
