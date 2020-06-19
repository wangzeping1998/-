/****************************************************
	作者：WangZeping
    邮箱: wangzeping1998@gmail.com
	功能：服务器初始化
*****************************************************/

class ServerRoot
{
    private static ServerRoot instance;
    public static ServerRoot Instance
   {
        get {
            if (instance == null)
            {
                instance = new ServerRoot();
            }

            return instance;
        }
    }

    public void Init()
    {
        //数据层TODO
        DBMgr.Instance.Init();
        //服务层
        NetSvc.Instance.Init();
        //系统层级
        LoginSys.Instance.Init();
    }

    public void Update()
    {
        NetSvc.Instance.Update();
    }

    public static int SessionID = 0;
    public static int GetSessionID()
    {
        if (SessionID == int.MaxValue)
        {
            SessionID = 0;
        }
        return SessionID += 1;
    }

}
