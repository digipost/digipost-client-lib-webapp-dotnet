namespace DigipostClientLibWebapp.Utilities
{
    public static class SessionManager
    {
        public static T GetFromSession<T>(string key)
        {
            var sessionData = System.Web.HttpContext.Current.Session[key];
            
            return sessionData == null ? default(T) : (T) sessionData;
        }

        public static void AddToSession<T>(string key, T value)
        {
            System.Web.HttpContext.Current.Session[key] = value;
        }

        public static void RemoveFromSession(string key)
        {
            System.Web.HttpContext.Current.Session.Remove(key);
        }
    }
}