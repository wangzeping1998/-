/****************************************************
	作者：WangZeping
    邮箱: wangzeping1998@gmail.com
	功能：服务器入口
*****************************************************/

using PENet;
using System;

namespace PEProtocol
{
    [Serializable]
    public class GameMsg : PEMsg
    {
        public ReqLogin reqLogin;
        public RspLogin rspLogin;

        public ReqRename reqRename;
        public RspRename rspRename;
    }

    #region 登入相关



    [Serializable]
    public class ReqLogin
    {
        public string acct;
        public string pass;
    }

    [Serializable]
    public class RspLogin
    {
        public PlayerData playerData;
    }

    [Serializable]
    public class PlayerData
    {
        public int id;
        public string name; //名字
        public int lv;  //等级
        public int exp; //经验值
        public int power;   //体力
        public int coin;    //金币
        public int diamond; //钻石
        public int hp;  //生命值
        public int ad;  //攻击力
        public int ap;  //法强
        public int addef;   //物理防御
        public int apdef;   //法术防御
        public int dodge;   //闪避
        public int pierce;  //穿透
        public int critical;    //暴击率
    }

    [Serializable]
    public class ReqRename
    {
        public string name;
    }

    [Serializable]
    public class RspRename
    {
        public string name;
    }

    #endregion

    public enum CMD
    {
        None = 0,
        ReqLogin = 101,
        RspLogin = 102,
        ReqRename = 103,
        RspRename = 104

    }

    public enum ErrorCode
    {
        None = 0,
        AcctIsOnline,   //账号已上线
        WrongPass,  //账号或密码错误
        NameIsExist,   //名字已经存在
        UpdateDBError, //更新数据出错
    }

    public class ServerCfg
    {
        public const string srvIP = "127.0.0.1";
        public const int srvPort = 17666;
    }
}
