using Microsoft.AspNetCore.Http;
using System.Net;
using System.Net.Sockets;

namespace Core.Helpers
{
    public static class NetworkHelper
    {
        public static string GetLocalIpAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                    return ip.ToString();
            return null;
        }

        public static string GetHostName()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            return host.HostName;
        }

        public static string GetRemoteIpAddress(this HttpContext context, bool allowForwarded = true)
        {
            if (allowForwarded)
            {
                string header = (context.Request.Headers["CF-Connecting-IP"].FirstOrDefault() ?? context.Request.Headers["X-Forwarded-For"].FirstOrDefault());
                if (IPAddress.TryParse(header, out IPAddress ip))
                    return ip.ToString();
            }
            return context.Connection.RemoteIpAddress.ToString();
        }
    }
}
