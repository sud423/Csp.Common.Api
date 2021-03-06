﻿using Csp.EF;
using Csp.OAuth.Api.Application;
using System.Text.Json.Serialization;

namespace Csp.OAuth.Api.Models
{
    public class UserLogin : Entity
    {
        public int Id { get; set; }

        public int WebSiteId { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        [JsonIgnore]
        public virtual User User { get; set; }


        public void SetNewPwd(string pwd)
        {
            Password = PasswordHasher.Hash(pwd);
        }
    }
}
