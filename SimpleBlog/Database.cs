using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NHibernate.Cfg;
using NHibernate.Mapping.ByCode;
using SimpleBlog.Models;
using NHibernate;

namespace SimpleBlog
{
    public class Database
    {
        private const string SessionKey = "SimpleBlog.Database.SessionKey";
        private static ISessionFactory _sessionFactory;

        public static ISession Session
        {
            get { return (ISession)HttpContext.Current.Items[SessionKey]; }
        }

        public static void Configure()
        {
            var config = new Configuration();

            //configure the connection string
            config.Configure();

            //add our mappings
             var mapper = new ModelMapper();
             mapper.AddMapping<UserMap>();
             mapper.AddMapping<RoleMap>();

             config.AddMapping(mapper.CompileMappingForAllExplicitlyAddedEntities());

            //create session factory
             _sessionFactory = config.BuildSessionFactory();
        }


        public static void OpenSession()
        {
            HttpContext.Current.Items[SessionKey] = _sessionFactory.OpenSession();
        }


        public static void CloseSession()
        {
            var session = HttpContext.Current.Items[SessionKey] as ISession;
            if (session != null)
                session.Close();

            HttpContext.Current.Items.Remove(SessionKey);
        }

    }
}