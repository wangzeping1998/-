/****************************************************
	作者：WangZeping
    邮箱: wangzeping1998@gmail.com
	功能：服务器初始化
*****************************************************/

using System.Threading;

class ServerRoot
{
    private static ServerRoot instance;
    public static ServerRoot Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new ServerRoot();
            }

            return instance;
        }
    }

    public void Init()
    {
        //数据层
        DBMgr.Instance.Init();
        //服务层
        NetSvc.Instance.Init();
        CfgRvc.Instance.Init();
        TimerSvc.Instance.Init();
        //系统层
        LoginSys.Instance.Init();
        GuideSys.Instance.Init();
        StrongSys.Instance.Init();
        ChatSys.Instance.Init();
        BuySys.Instance.Init();
        PowerSys.Instance.Init();

    }

    public void Update()
    {
        NetSvc.Instance.Update();
        TimerSvc.Instance.Update();
    }

    public static int SessionID = 0;

    /// <summary>
    ///获取连接ID 
    /// </summary>
    public static int GetSessionID()
    {
        if (SessionID == int.MaxValue)
        {
            SessionID = 0;
        }
        return SessionID += 1;
    }

}
