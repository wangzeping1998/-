/****************************************************
	作者：WangZeping
    邮箱: wangzeping1998@gmail.com
	功能：协议定义
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

        public ReqGuide reqGuide;
        public RspGuide rspGuide;

        public ReqStrong reqStrong;
        public RspStrong rspStrong;

        public SndChat sndChat;
        public PshChat pshChat;

        public ReqBuy reqBuy;
        public RspBuy rspBuy;

        public PshPower pshPower;

        public ReqTaskReward reqTaskReward;
        public RspTaskReward rspTaskReward;

        public PshTaskPrgs pshTaskPrgs;

        public ReqFBFight reqFBFight;
        public RspFBFight rspFBFight;
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
        public int crystal; //水晶
        public int hp;  //生命值
        public int ad;  //攻击力
        public int ap;  //法强
        public int addef;   //物理防御
        public int apdef;   //法术防御
        public int dodge;   //闪避
        public int pierce;  //穿透
        public int critical;    //暴击率
        public int guideId; //任务ID
        public int[] strongArr; //强化部位
        public long time;
        public string[] taskArr;    //任务奖励系统
        public int fuben;
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

    #region 引导任务相关

    [Serializable]
    public class ReqGuide
    {
        public int guideId;
    }


    [Serializable]
    public class RspGuide
    {
        public int guideId;
        public int lv;
        public int exp;
        public int coin;
    }
    #endregion

    #region 强化相关
    [Serializable]
    public class ReqStrong
    {
        public int pos; //强化部位
    }

    [Serializable]
    public class RspStrong
    {
        public int coin;
        public int crystal;
        public int hp;
        public int ad;
        public int ap;
        public int addef;
        public int apdef;
        public int[] strongArr;
    }
    #endregion

    #region 聊天相关

    [Serializable]
    public class SndChat
    {
        public string chat;
    }

    [Serializable]
    public class PshChat
    {
        public string name;
        public string chat;
    }

    #endregion

    #region 购买系统
    [Serializable]
    public class ReqBuy
    {
        public int type;
        public int diamond;
    }

    [Serializable]
    public class RspBuy
    {
        public int type;
        public int diamond;
        public int coin;
        public int power;
    }

    [Serializable]
    public class PshPower
    {
        public int power;
    }

    #endregion

    #region 任务奖励系统

    [Serializable]
    public class ReqTaskReward
    {
        public int taskId;
    }

    [Serializable]
    public class RspTaskReward
    {
        public int coin;
        public int exp;
        public int lv;
        public string[] taskArr;
    }

    #endregion

    #region 推送任务进度
    [Serializable]
    public class PshTaskPrgs
    {
        public string[] taskArr;
    }
    #endregion

    #region 副本战斗
    [Serializable]
    public class ReqFBFight
    {
        public int id;
    }

    [Serializable]
    public class RspFBFight
    {
        public int id;
        public int power;
    }

    #endregion

    /// <summary>
    /// 消息命令
    /// </summary>
    public enum CMD
    {
        None = 0,
        //登入模块
        //登入
        ReqLogin = 101,
        RspLogin = 102,
        //角色命名
        ReqRename = 103,
        RspRename = 104,

        //主城相关模块
        //引导任务
        ReqGuide = 201,
        RspGuide = 202,
        //强化
        ReqStrong = 203,
        RspStrong = 204,
        //聊天
        SndChat = 205,
        PshChat = 206,
        //购买
        ReqBuy = 207,
        RspBuy = 208,
        //体力回复
        PshPower = 209,
        //任务奖励
        ReqTaskReward = 210,
        RspTaskReward = 211,
        //任务进度推送
        PshTaskPrgs = 212,
        //副本战斗
        ReqFBFight= 301,
        RspFBFight = 302,
    }

    /// <summary>
    /// 错误码
    /// </summary>
    public enum ErrorCode
    {
        None = 0,
        AcctIsOnline,   //账号已上线
        WrongPass,  //账号或密码错误
        NameIsExist,   //名字已经存在
        UpdateDBError, //更新数据出错
        ServerDataError,    //服务器数据错误
        MaxStar,    //已经强化最大星级
        LackLevel,  //等级不够
        LackCoin,   //缺少金币
        LackDiamond,    //缺少钻石
        LackCrystal,    //缺少强化水晶
        LackPower,  //体力不足
        ClientDataError,    //客户端数据异常
    }

    public class ServerCfg
    {
        public const string srvIP = "127.0.0.1";
        public const int srvPort = 17666;
    }
}
