using PENet;
using Protocol;
using System.Threading;

namespace PEServer
{
    class Program
    {
        static void Main(string[] args)
        {
            PESocket<ServerSession, NetMsg> server = new PESocket<ServerSession, NetMsg>();
            server.StartAsServer(IPCig.srvIP, IPCig.srvPort);

            while (true)
            {
                Thread.Sleep(100);
            }
        }
    }
}
