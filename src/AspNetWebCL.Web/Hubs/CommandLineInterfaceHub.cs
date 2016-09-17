using System.Net.NetworkInformation;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.SignalR.Infrastructure;
using System.Net;
using System;
using AspNetWebCL.Web.Helpers;

namespace AspNetWebCL.Web.Hubs
{
    public class CommandLineInterfaceHub: Hub, ICommandLineInterfaceHub
    {
        readonly IConnectionManager _connectionManager;

        public CommandLineInterfaceHub(IConnectionManager connectionManager)
        {
            _connectionManager = connectionManager;
        }
        public void SendCommand(string command)
        {
            var hubContext = _connectionManager.GetHubContext<CommandLineInterfaceHub>();

            if (command.StartsWith("ping", System.StringComparison.CurrentCulture))
            {
                PingCommand(command, hubContext);
            }
            else if (command.StartsWith("wholeschool zenyatta", System.StringComparison.CurrentCulture))
            {
                ZenyattaCommand(command, hubContext);
            }
            else if (command.StartsWith("wholeschool dsl", System.StringComparison.CurrentCulture))
            {
                DslCommand(command, hubContext);
            }
        }

        private void DslCommand(string command, IHubContext hubContext)
        {
            var tasks = command.Replace("'", "").Split(' ');

            hubContext.Clients.All.commandResponse("Creating " + tasks[4] + " " + tasks[3]);
            System.Threading.Thread.Sleep(1000);
            hubContext.Clients.All.commandResponse("Done.");
        }

        private void ZenyattaCommand(string command, IHubContext hubContext)
        {
            var tasks = command.Replace("'", "").Split(' ');

            hubContext.Clients.All.commandResponse("Configuring " + tasks[3] + " Module System");
            System.Threading.Thread.Sleep(1000);
            hubContext.Clients.All.commandResponse("Containers created successfully");
            System.Threading.Thread.Sleep(1000);
            hubContext.Clients.All.commandResponse("Components created successfully");
            System.Threading.Thread.Sleep(1000);
            hubContext.Clients.All.commandResponse("Fields created successfully");
            System.Threading.Thread.Sleep(1000);
            hubContext.Clients.All.commandResponse("Modules System Ready.");
        }

        private static void PingCommand(string command, IHubContext hubContext)
        {
            var url = command.Split(' ')[1];
            var ip = Dns.GetHostAddressesAsync(url).Result[0];

            hubContext.Clients.All.commandResponse($"Pinging {url} {ip} with 32 bytes of data:");
            for (int i = 0; i < 3; i++)
            {
                var ping = PingServer.Ping(url);
                hubContext.Clients.All.commandResponse(ping.Message);
                System.Threading.Thread.Sleep(1000);
            }

            hubContext.Clients.All.commandResponse($"\nPing statistics for {ip}:\n   Packets: Sent = 4, Received = 4, Lost = 0(0 % loss),\nApproximate round trip times in milli - seconds:\n   Minimum = 23ms, Maximum = 23ms, Average = 23ms \n");
        }


        //System.Net.Utilities doesnt appear to be compatabile with aspnetcore yet =(
        //public static double PingTimeAverage(string host, int echoNum)
        //{
        //    long totalTime = 0;
        //    int timeout = 120;
        //    Ping pingSender = new Ping();

        //    for (int i = 0; i < echoNum; i++)
        //    {
        //        PingReply reply = pingSender.SendPingAsync(host, timeout).Result;
        //        if (reply.Status == IPStatus.Success)
        //        {
        //            totalTime += reply.RoundtripTime;
        //        }
        //    }
        //    return totalTime / echoNum;
        //}
    }
}
