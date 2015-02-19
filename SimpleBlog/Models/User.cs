using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

namespace SimpleBlog.Models
{
    public class User
    {
        private const int WorkFactor = 13;
        
        public static void FakeHash()
        {
            BCrypt.Net.BCrypt.HashPassword("", WorkFactor);
        }
            
        public virtual int Id { get; set; }
        public virtual string Username { get; set; }
        public virtual string Email { get; set; }
        public virtual string PasswordHash { get; set; }

       

        public virtual void SetPassword(string password)
        {
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(password,WorkFactor);
        }

        public virtual bool CheckPassword(string password)
        {
            return BCrypt.Net.BCrypt.Verify(password, PasswordHash);
        }
    }

    public class UserMap : ClassMapping<User>
    {
        public UserMap()
        {
            Table("users");
            Id(x=> x.Id, x=> x.Generator(Generators.Identity));

            Property(x => x.Username, x => x.NotNullable(true));
            Property(x => x.Email, x => x.NotNullable(true));
            Property(x => x.PasswordHash, x=>
            {
                x.Column("passwordhash");
                x.NotNullable(true);        
            });
        }
        
    }
}