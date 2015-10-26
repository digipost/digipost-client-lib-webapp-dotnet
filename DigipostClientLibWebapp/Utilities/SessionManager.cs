using System.Web;

namespace DigipostClientLibWebapp.Utilities
{
    public static class SessionManager
    {
        public static T GetFromSession<T>(HttpContextBase Context,string key)
        {
            var sessionData = Context.Session[key];
            
            return sessionData == null ? default(T) : (T) sessionData;
        }

        public static void AddToSession<T>(HttpContextBase Context,string key, T value)
        {
             Context.Session[key] = value;
        }

        public static void RemoveFromSession(HttpContextBase Context,string key)
        {
            Context.Session.Remove(key);
        }
    }
}