
/****************************************************
	作者：WangZeping
    邮箱: wangzeping1998@gmail.com
	功能：强化系统
*****************************************************/
using PEProtocol;
using System;

class StrongSys
{
    private static StrongSys instance;
    public static StrongSys Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new StrongSys();
            }

            return instance;
        }
    }

    private CacheSvc cacheSvc;
    private CfgRvc cfgSvc;

    public void Init()
    {
        cacheSvc = CacheSvc.Instance;
        cfgSvc = CfgRvc.Instance;
        PECommon.Log("StrongSys Init Done.");
    }

    public void ReqStrong(MsgPack pack)
    {
        ReqStrong data = pack.msg.reqStrong;


        GameMsg msg = new GameMsg
        {
            //消息类型
            cmd = (int)CMD.RspStrong,
        };


        PlayerData pd = cacheSvc.GetPlayerDataBySession(pack.session);
        StrongCfg nextSc =  cfgSvc.GetStrongCfgData(data.pos, pd.strongArr[data.pos] + 1);
        if (nextSc == null)
        {
            //错误码:满星
            msg.err = (int)ErrorCode.MaxStar;

        }
        else if (pd.lv < nextSc.minlv)
        {
            //错误码:等级不够
            msg.err = (int)ErrorCode.LackLevel;
        }
        else if (pd.coin < nextSc.coin)
        {
            //错误码:金币不够
            msg.err = (int)ErrorCode.LackCoin;
        }
        else if (pd.crystal < nextSc.crystal)
        {
            //错误码:水晶不够
            msg.err = (int)ErrorCode.LackCrystal;
        }
        else
        {
            //满足条件
            //消耗金币和水晶
            pd.coin -= nextSc.coin;
            pd.crystal -= nextSc.crystal;

            //增加属性
            pd.hp += nextSc.addhp;
            pd.ad += nextSc.addhurt;
            pd.ap += nextSc.addhurt;
            pd.addef += nextSc.adddef;
            pd.apdef += nextSc.adddef;
            pd.strongArr[data.pos] += 1;

            if (DBMgr.Instance.UpdatePlayerData(pd.id,pd))
            {
                //发送协议更新客户端内容
                msg.rspStrong = new RspStrong
                {
                    coin = pd.coin,
                    crystal = pd.crystal,
                    hp = pd.hp,
                    ad = pd.ad,
                    ap = pd.ap,
                    addef = pd.addef,
                    apdef = pd.apdef,
                    strongArr = pd.strongArr,
                };
            }
            else
            {
                msg.err = (int)ErrorCode.UpdateDBError;
            }
        }


        pack.session.SendMsg(msg);


    }
}