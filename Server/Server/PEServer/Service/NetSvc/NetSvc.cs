/****************************************************
	作者：WangZeping
    邮箱: wangzeping1998@gmail.com
	功能：网络服务
*****************************************************/
using PENet;
using PEProtocol;
using System.Collections;
using System.Collections.Generic;


class MsgPack
{
    public ServerSession session; 
    public GameMsg msg;

    public MsgPack()
    {
    }

    public MsgPack(ServerSession session, GameMsg msg)
    {
        this.session = session;
        this.msg = msg;
    }
}

class NetSvc
{
    private static NetSvc instance;
    public static NetSvc Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new NetSvc();
            }

            return instance;
        }
    }

    private Queue<MsgPack> msgPackQue = new Queue<MsgPack>();
    public static readonly string queLock = "lock";

    public void Init()
    {
        PESocket<ServerSession, GameMsg> server = new PESocket<ServerSession, GameMsg>();
        server.StartAsServer(ServerCfg.srvIP,ServerCfg.srvPort);

        PECommon.Log("NetSvc Init Done.");
    }

    public void AddMsg(MsgPack pack)
    {
        lock (queLock)
        {
            msgPackQue.Enqueue(pack);
        }
    }

    public void Update()
    {
        if (msgPackQue.Count > 0)
        {
            lock (queLock)
            {
                MsgPack pack = msgPackQue.Dequeue();
                HandOutMsg(pack);
            }
        }
    }

    private void HandOutMsg(MsgPack pack)
    {
        CMD cmd = (CMD)pack.msg.cmd;
        switch (cmd)
        {
            case CMD.ReqLogin: LoginSys.Instance.ReqLogin(pack);
                break;
            case CMD.ReqRename: LoginSys.Instance.ReqRename(pack);
                break;
            default:
                break;
        }
    }
}
