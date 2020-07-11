/****************************************************
	作者：WangZeping
    邮箱: wangzeping1998@gmail.com
	功能：服务器客户端通用工具
*****************************************************/
using PENet;
using PEProtocol;

public enum LogType
{
    None,
    Warn,
    Error,
    Info
}

public class PECommon
{
    public static void Log(string msg, LogType logType = LogType.None)
    {
        LogLevel lv = (LogLevel)logType;
        PETool.LogMsg(msg, lv);
    }

    /// <summary>
    /// 计算战斗力
    /// </summary>
    public static int GetFightProps(PlayerData pd)
    {
        //公式
        return pd.lv * 100 + pd.ad + pd.ap + pd.addef + pd.apdef;
    }

    /// <summary>
    /// 计算体力值
    /// </summary>
    public static int GetPowerLimit(int lv)
    {
        //公式
        return ((lv - 1) / 10) * 150 + 150;
    }

    /// <summary>
    /// 计算升级所需经验
    /// </summary>
    public static int GetExpUpValByLv(int lv)
    {
        return 100 * lv * lv;
    }

    /// <summary>
    /// 升级逻辑
    /// </summary>
    public static void CalExp(PlayerData pd, int addExp)
    {
        int curtLv = pd.lv;
        int curtExp = pd.exp;
        int addRestExp = addExp;
        while (true)
        {
            //计算出当前等级升级需要的经验,处理升级与连续升级问题
            int upNeedExp = PECommon.GetExpUpValByLv(curtLv) - curtExp;
            if (addRestExp >= upNeedExp)
            {
                //能升级
                curtLv++;
                curtExp = 0;
                addRestExp -= upNeedExp;
            }
            else
            {
                //不能升级
                pd.lv = curtLv;
                pd.exp = curtExp + addRestExp;
                break;
            }
        }
    }

    public const int PowerAddSpace = 5;    //体力增加间隔
    public const int PowerAddCount = 2;    //每次增加体力值
}