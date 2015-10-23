using System.Runtime.InteropServices.ComTypes;
using Digipost.Api.Client.Domain.Search;
using DigipostClientLibWebapp.Constants;

namespace DigipostClientLibWebapp.Utilities
{
    public static class SessionManager
    {
        public static T GetFromSession<T>()
        {
            return (T) System.Web.HttpContext.Current.Session[SessionConstants.PersonDetails];
        }

        public static void AddToSession<T>(T value)
        {
            System.Web.HttpContext.Current.Session[SessionConstants.PersonDetails] = value;
        }
    }
}