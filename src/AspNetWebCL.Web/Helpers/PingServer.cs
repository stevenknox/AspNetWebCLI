using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace AspNetWebCL.Web.Helpers
{
    public class PingServer
    {
        public static PingResponse Ping(string url)
        {
            try
            {
                var ip = Dns.GetHostAddressesAsync(url).Result[0];

                var host = new Uri("http://" + url);
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(host);
                request.Method = "HEAD";
                Stopwatch pingTime = Stopwatch.StartNew();
                HttpWebResponse response = (HttpWebResponse)request.GetResponseAsync().Result;
                pingTime.Stop();
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    return new PingResponse { ResponseTime = pingTime.ElapsedMilliseconds, Message = $"Reply from {ip}: bytes=32 time={pingTime.ElapsedMilliseconds}ms TTL=54" };
                }
                else
                {
                    return new PingResponse { ResponseTime = pingTime.ElapsedMilliseconds, Message = "Response timed out" };
                }
            }
            catch (Exception ex)
            {
                return new PingResponse { ResponseTime = 0, Message = "Response timed out. " + ex.Message  };

            }
        }
    }

    public class PingResponse
    {
        public double ResponseTime { get; set; }
        public string Message { get; set; }
    }
}
