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

    public void Init()
    {
        cacheSvc = CacheSvc.Instance;
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

            if (!cacheSvc.UpdataPlayerData(playerData.id,playerData))
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

    public void ClearOfflineData(ServerSession session)
    {
        cacheSvc.AcctOffLine(session);
    }
}