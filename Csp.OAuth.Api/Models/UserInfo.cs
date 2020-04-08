using Csp.EF;
using System;
using System.Text.Json.Serialization;

namespace Csp.OAuth.Api.Models
{
    public enum Sex
    {
        Unknown,
        Male,
        Female
    }

    public class UserInfo : Entity
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public Sex Sex { get; set; }

        public DateTime? BirthDate { get; set; }

        public string Cell { get; set; }

        public string Email { get; set; }

        public string Wx { get; set; }

        public string Qq { get; set; }

        public string Avatar { get; set; }

        public string Remark { get; set; }

        [JsonIgnore]
        public virtual User User { get; set; }
    }
}
