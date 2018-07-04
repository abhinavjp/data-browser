using AutoMapper;
using DataBrowser.Data;
using DataBrowser.Data.Repository;
using DataBrowser.Service.Interface;
using DataBrowser.Service.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBrowser.Service.Services
{
    public class UserService : IUserService
    {
        public string CreateUserRegistration(UserServiceModel userDetail)
        {
            try
            {

                if (userDetail == null)
                {
                    // return with error
                }
                var encryptPassword = EncryptPassword(userDetail.Password);
                userDetail.Password = encryptPassword;
                var user = Mapper.Map<User>(userDetail);
                user.CreatedOn = DateTime.UtcNow;

                using (var repo = new RepositoryPattern<User>())
                {
                    repo.Insert(user);
                    repo.Save();
                }
                return "Create User Succesfully";
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public bool FindUser(string userName, string password)
        {
            try
            {
                if (string.IsNullOrEmpty(userName) || string.IsNullOrWhiteSpace(userName) || string.IsNullOrWhiteSpace(password) || string.IsNullOrEmpty(password))
                {
                    return false;
                }
                else
                {
                    bool isUserAvilable = false;
                    using (var repo = new RepositoryPattern<User>())
                    {
                        isUserAvilable = repo.SelectAll()
                            .Any(a => string.Compare(a.UserName , userName) == 0 &&
                       string.Compare(DecryptPassword(a.Password),password) == 0);
                    }
                    return isUserAvilable;
                }

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private string EncryptPassword(string pass)
        {
            byte[] bytes = System.Text.Encoding.Unicode.GetBytes(pass);
            string encryptedPassword = Convert.ToBase64String(bytes);
            return encryptedPassword;
        }

        private string DecryptPassword(string encryptPassword)
        {
            byte[] bytes = Convert.FromBase64String(encryptPassword);
            string decryptedPassword = System.Text.Encoding.Unicode.GetString(bytes);
            return decryptedPassword;
        }
    }
}
