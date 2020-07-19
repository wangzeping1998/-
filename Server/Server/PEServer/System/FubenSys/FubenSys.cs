/****************************************************
	作者：WangZeping
    邮箱: wangzeping1998@gmail.com
	功能：副本系统
*****************************************************/
using PEProtocol;
using System;

public class FubenSys
{
    private static FubenSys instance;
    public static FubenSys Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new FubenSys();
            }

            return instance;
        }
    }

    private CacheSvc cacheSvc;
    private CfgRvc cfgRvc;


    public void Init()
    {
        cacheSvc = CacheSvc.Instance;
        cfgRvc = CfgRvc.Instance;
        PECommon.Log("FubenSys Init Done.");
    }

    public void ReqFBFight(MsgPack pack)
    {
        ReqFBFight data = pack.msg.reqFBFight;

        GameMsg msg = new GameMsg()
        {
            cmd = (int)CMD.RspFBFight,
        };

        MapCfg mcfg =  cfgRvc.GetMapCfgData(data.id);
        PlayerData pd = cacheSvc.GetPlayerDataBySession(pack.session);


        if (pd.fuben < data.id)
        {
            //玩家记录副本ID少于客户端发过来的副本ID
            //客户端数据异常
            msg.err = (int)ErrorCode.ClientDataError;
        }
        else if (pd.power < mcfg.power)
        {
            //体力不足
            msg.err = (int)ErrorCode.LackPower;
        }
        else
        {
            //扣除体力
            pd.power -= mcfg.power;
            if (!cacheSvc.UpdataPlayerData(pd.id,pd))
            {
                //数据库写入失败
                msg.err = (int)ErrorCode.UpdateDBError;
            }
            else
            {
                TaskSys.Instance.CalcTaskPrgs(pd, 2);
                msg.rspFBFight = new RspFBFight()
                {
                    id = data.id,
                    power = pd.power
                };
            }
        }

        pack.session.SendMsg(msg);
    }
}
