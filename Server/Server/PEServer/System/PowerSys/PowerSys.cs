/****************************************************
	作者：WangZeping
    邮箱: wangzeping1998@gmail.com
	功能：体力系统
*****************************************************/
using PEProtocol;
using System;
using System.Collections.Generic;

public class PowerSys
{
    private static PowerSys instance;
    public static PowerSys Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new PowerSys();
            }

            return instance;
        }
    }

    private CacheSvc cacheSvc;
    private TimerSvc timeSvc;

    public void Init()
    {
        cacheSvc = CacheSvc.Instance;
        timeSvc = TimerSvc.Instance;
        TimerSvc.Instance.AddTimeTask(CalcPowerAdd, PECommon.PowerAddSpace, PETimeUnit.Minute, 0);
        PECommon.Log("PowerSys Init Done.");
    }

    private void CalcPowerAdd(int tId)
    {
        //计算体力增长

        GameMsg msg = new GameMsg
        {
            cmd = (int)CMD.PshPower,
            pshPower = new PshPower()
        };

        Dictionary<ServerSession, PlayerData> onlineDic = cacheSvc.GetOnlineCache();
        foreach (var item in onlineDic)
        {
            PlayerData pd = item.Value;
            ServerSession session = item.Key;

            int powerMax = PECommon.GetPowerLimit(pd.lv);
            if (pd.power >= powerMax)
            {
                //不回复体力
                continue;
            }
            else
            {
                //回复体力
                pd.power += PECommon.PowerAddCount;
                if (pd.power > powerMax)
                {
                    //限定回复，防止溢出体力最大值
                    pd.power = powerMax;
                    pd.time = timeSvc.GetNowTime();
                }


                if (!cacheSvc.UpdataPlayerData(pd.id, pd))
                {
                    msg.err = (int)ErrorCode.UpdateDBError;
                }
                else
                {
                    msg.pshPower.power = pd.power;
                    session.SendMsg(msg);
                }

            }
           
        }
    }
}