using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using FluentMigrator;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

namespace SimpleBlog.Migrations
{
    [Migration(1)]
    public class _001_UsersAndRoles : Migration
    {


        public override void Up()
        {
            Create.Table("users")
                .WithColumn("id").AsInt32().Identity().PrimaryKey()
                .WithColumn("username").AsString(128)
                .WithColumn("email").AsCustom("VARCHAR(256)")
                .WithColumn("passwordhash").AsString(128);

            Insert.IntoTable("users").Row(new
            {
                username = "ben", 
                email = "ben@test.com",
                passwordhash = BCrypt.Net.BCrypt.HashPassword("test",13)
            });

            Create.Table("roles")
                .WithColumn("id").AsInt32().Identity().PrimaryKey()
                .WithColumn("name").AsString(128);

            Insert.IntoTable("roles").Row(new { name = "admin" });
            Insert.IntoTable("roles").Row(new { name = "moderator" });
            Insert.IntoTable("roles").Row(new { name = "author" });
            Insert.IntoTable("roles").Row(new { name = "user" });

            Create.Table("role_users")
                .WithColumn("user_id").AsInt32().ForeignKey("users", "id").OnDelete(Rule.Cascade)
                .WithColumn("role_id").AsInt32().ForeignKey("roles", "id").OnDelete(Rule.Cascade);

            Insert.IntoTable("role_users").Row(new
            {
                user_id = "1",
                role_id = "1"
            });

        }

        public override void Down()
        {
            Delete.Table("role_users");
            Delete.Table("roles"); 
            Delete.Table("users");
            
        }


    }
}