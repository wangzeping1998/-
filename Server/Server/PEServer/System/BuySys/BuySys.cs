/****************************************************
	作者：WangZeping
    邮箱: wangzeping1998@gmail.com
	功能：购买系统
*****************************************************/
using PEProtocol;
using System;

public class BuySys
{
    private static BuySys instance;
    public static BuySys Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new BuySys();
            }

            return instance;
        }
    }

    private CacheSvc cacheSvc;


    public void Init()
    {
        cacheSvc = CacheSvc.Instance;
        PECommon.Log("BuySys Init Done.");
    }

    public void ReqBuy(MsgPack pack)
    {
        ReqBuy data = pack.msg.reqBuy;

        GameMsg msg = new GameMsg
        {
            cmd = (int)CMD.RspBuy,
        };

        PlayerData pd = cacheSvc.GetPlayerDataBySession(pack.session);
        if (pd.diamond < data.diamond)
        {
            msg.err = (int)ErrorCode.LackDiamond;
        }
        else
        {
            pd.diamond -= 10;
            switch (data.type)
            {
                case 0:
                    //购买体力
                    pd.power += 100;
                    //更新任务进度
                    TaskSys.Instance.CalcTaskPrgs(pd, 4);
                    break;
                case 1:
                    //购买金币
                    pd.coin += 1000;
                    //更新任务进度
                    TaskSys.Instance.CalcTaskPrgs(pd, 5);
                    break;
            }

            if (!DBMgr.Instance.UpdatePlayerData(pd.id,pd))
            {
                msg.err = (int)ErrorCode.ServerDataError;
            }
            else
            {
                msg.rspBuy = new RspBuy
                {
                    type = data.type,
                    diamond = pd.diamond,
                    power = pd.power,
                    coin = pd.coin,
                };
            }
        }
        pack.session.SendMsg(msg);


    }
}