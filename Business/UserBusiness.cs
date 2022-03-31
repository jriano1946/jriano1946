namespace Business
{
    using Entities;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    public class UserBusiness : IUserBusiness
    {
        private readonly List<User> _users = new List<User>();
        public Result CreateUser(User user)
        {

            var errors = "";

            ValidateErrors(user, ref errors);

            if (errors != null && errors != "")
                return new Result()
                {
                    IsSuccess = false,
                    Errors = errors
                };


            switch (user.UserType)
            {
                case "Normal":
                    if (user.Money > 100)
                    {
                        var percentage = Convert.ToDecimal(0.12);
                        //If new user is normal and has more than USD100
                        var gif = user.Money * percentage;
                        user.Money = user.Money + gif;
                    }
                    else if (user.Money > 10)
                    {
                        var percentage = Convert.ToDecimal(0.8);
                        var gif = user.Money * percentage;
                        user.Money = user.Money + gif;
                    }
                    break;
                case "SuperUser":
                    if (user.Money > 100)
                    {
                        var percentage = Convert.ToDecimal(0.20);
                        var gif = user.Money * percentage;
                        user.Money = user.Money + gif;
                    }
                    break;
                case "Premium":
                    if (user.Money > 100)
                    {
                        var gif = user.Money * 2;
                        user.Money = user.Money + gif;
                    }
                    break;
            }
            ReadUsersFromFile();
            //Normalize email
            var aux = user.Email.Split(new char[] { '@' }, StringSplitOptions.RemoveEmptyEntries);

            var atIndex = aux[0].IndexOf("+", StringComparison.Ordinal);

            aux[0] = atIndex < 0 ? aux[0].Replace(".", "") : aux[0].Replace(".", "").Remove(atIndex);

            user.Email = string.Join("@", new string[] { aux[0], aux[1] });
            try
            {
                bool isDuplicated = false;

                IEnumerable<User> resultsEmailPhone = _users.Where(x => x.Email == user.Email || x.Phone == user.Phone);
                IEnumerable<User> resultsName = _users.Where(x => x.Name == user.Name || x.Address == user.Address);
                isDuplicated = true ? resultsEmailPhone.Count() > 0 : false;
                
                if (isDuplicated == false && resultsName.Count() > 0)
                {
                    isDuplicated = true;
                    throw new Exception("User is duplicated");
                }

                if (!isDuplicated)
                {
                    return new Result()
                    {
                        IsSuccess = true,
                        Errors = "User Created"
                    };
                }
                else
                {
                    return new Result()
                    {
                        IsSuccess = false,
                        Errors = "The user is duplicated"
                    };
                }
            }
            catch
            {
                return new Result()
                {
                    IsSuccess = false,
                    Errors = "The user is duplicated"
                };
            }
        }

        //Validate errors
        private void ValidateErrors(User user, ref string errors)
        {
            if (user.Name == null)
                //Validate if Name is null
                errors = "The name is required";
            if (user.Email == null)
                //Validate if Email is null
                errors += " The email is required";
            if (user.Address == null)
                //Validate if Address is null
                errors += errors + " The address is required";
            if (user.Phone == null)
                //Validate if Phone is null
                errors += errors + " The phone is required";
        }

        private void ReadUsersFromFile()
        {
            var path = Directory.GetCurrentDirectory() + "/Files/Users.txt";
            using StreamReader reader = new StreamReader(new FileStream(path, FileMode.Open));
            while (reader.Peek() >= 0)
            {
                var line = reader.ReadLineAsync().Result;
                var users = new User
                {
                    Name = line.Split(',')[0].ToString(),
                    Email = line.Split(',')[1].ToString(),
                    Phone = line.Split(',')[2].ToString(),
                    Address = line.Split(',')[3].ToString(),
                    UserType = line.Split(',')[4].ToString(),
                    Money = decimal.Parse(line.Split(',')[5].ToString()),
                };
                _users.Add(users);
            }
        }
    }
}

