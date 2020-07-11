/****************************************************
	作者：WangZeping
    邮箱: wangzeping1998@gmail.com
	功能：任务系统
*****************************************************/

using PEProtocol;
using System;
using System.Data.Odbc;
using System.Security.Cryptography;
using static CfgRvc;

class GuideSys
{
    private static GuideSys instance;
    public static GuideSys Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new GuideSys();
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
        PECommon.Log("GuideSys Init Done.");
    }

    /// <summary>
    /// 引导任务请求
    /// </summary>
    public void ReqGuide(MsgPack pack)
    {
        ReqGuide reqGuide = pack.msg.reqGuide;

        GameMsg msg = new GameMsg
        {
            cmd = (int)CMD.RspGuide,
        };

        PlayerData pd = cacheSvc.GetPlayerDataBySession(pack.session);
        AutoGuideCfg  agc = cfgSvc.GetGuideCfgData(pd.guideId);
        //校验
        if (pd.guideId == reqGuide.guideId)
        {
            //检查为智者点播任务
            if (pd.guideId == 1001)
            {
                TaskSys.Instance.CalcTaskPrgs(pd,1);
            }

            //更新玩家数据
            pd.guideId++;
            pd.coin += agc.coin;
            PECommon.CalExp(pd, agc.exp);

            if (!cacheSvc.UpdataPlayerData(pd.id,pd))
            {
                //如果数据库更新出错
                msg.err = (int)ErrorCode.UpdateDBError;
            }
            else
            {
                RspGuide rspGuide = new RspGuide
                {
                    guideId = pd.guideId,
                    coin = pd.guideId,
                    lv = pd.lv,
                    exp = pd.exp,
                };
                msg.rspGuide = rspGuide;
            }
        }
        else
        {
            msg.err = (int)ErrorCode.ServerDataError;
        }

        pack.session.SendMsg(msg);
        
    }

    
}