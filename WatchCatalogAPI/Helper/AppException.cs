using System.Globalization;

namespace WatchCatalogAPI.Helper
{
    public class AppException:Exception
    {
        public AppException() : base() { }
        public AppException(string message, params object[] args) 
            : base(string.Format(CultureInfo.CurrentCulture, message, args)) { }
    }
}
