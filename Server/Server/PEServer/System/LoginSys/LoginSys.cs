/****************************************************
	作者：WangZeping
    邮箱: wangzeping1998@gmail.com
	功能：登入系统
*****************************************************/

using PENet;
using PEProtocol;
using System;

class LoginSys
{
    private static LoginSys instance;
    public static LoginSys Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new LoginSys();
            }

            return instance;
        }
    }

    private CacheSvc cacheSvc;
    private TimerSvc timerSvc;

    public void Init()
    {
        cacheSvc = CacheSvc.Instance;
        timerSvc = TimerSvc.Instance;
        PECommon.Log("LoginSys Init Done.");
    }

    public void ReqLogin(MsgPack pack)
    {
        ReqLogin data = pack.msg.reqLogin;

        GameMsg msg = new GameMsg()
        {
            cmd = (int)CMD.RspLogin,
        };

        //如果账号已经登入
        if (cacheSvc.IsAcctOnline(data.acct))
        {
            msg.err = (int)ErrorCode.AcctIsOnline;
        }
        else
        {
            PlayerData pd = cacheSvc.GetPlayerData(data.acct, data.pass);
            if (pd == null)
            {
                //存在，密码错误
                msg.err = (int)ErrorCode.WrongPass;
            }
            else
            {
                //登入成功

                //计算体力最大值
                int power = pd.power;
                int powerMax = PECommon.GetPowerLimit(pd.lv);
                if (pd.power < powerMax)
                {
                    long nowTime = timerSvc.GetNowTime();
                    long millisecendsCount = nowTime - pd.time;
                    //计算玩家离线时间回复体力值
                    int addPower = (int)(millisecendsCount / (1000  *60* PECommon.PowerAddSpace)) * PECommon.PowerAddCount;
                    if (addPower > 0)
                    {
                        pd.power += addPower;
                        if (pd.power > powerMax)
                        {
                            pd.power = powerMax;
                        }
                    }
                }

                if (pd.power != power)
                {
                    cacheSvc.UpdataPlayerData(pd.id, pd);
                }

                msg.rspLogin = new RspLogin()
                {
                    playerData = pd
                };
                cacheSvc.AcctOnline(data.acct, pack.session, pd);
            }
        }

        pack.session.SendMsg(msg);
    }

    public void ReqRename(MsgPack pack)
    {
        ReqRename rename = pack.msg.reqRename;

        GameMsg msg = new GameMsg()
        {
            cmd = (int)CMD.RspRename,

        };

        if (cacheSvc.IsNameExist(rename.name))
        {
            msg.err = (int)ErrorCode.NameIsExist;
        }
        else
        {
            PlayerData playerData = cacheSvc.GetPlayerDataBySession(pack.session);
            playerData.name = rename.name;

            if (!cacheSvc.UpdataPlayerData(playerData.id, playerData))
            {
                msg.err = (int)ErrorCode.UpdateDBError;
            }
            else
            {
                msg.rspRename = new RspRename()
                {
                    name = rename.name
                };
            }
        }

        pack.session.SendMsg(msg);


    }

    //下线
    public void ClearOfflineData(ServerSession session)
    {
        //写入离线时间
        PlayerData pd = cacheSvc.GetPlayerDataBySession(session);
        if (pd!=null)
        {
            pd.time = timerSvc.GetNowTime();
            if (!cacheSvc.UpdataPlayerData(pd.id,pd))
            {
                PECommon.Log("Update offline time error!", LogType.Error);
            }
        }
        cacheSvc.AcctOffLine(session);
    }
}