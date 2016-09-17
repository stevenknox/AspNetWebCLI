namespace AspNetWebCL.Web.Hubs
{
    public interface ICommandLineInterfaceHub
    {
        void SendCommand(string command);
    }
}