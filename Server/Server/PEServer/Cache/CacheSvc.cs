/****************************************************
	作者：WangZeping
    邮箱: wangzeping1998@gmail.com
	功能：缓存层
*****************************************************/

using MySqlX.XDevAPI;
using PEProtocol;
using System.Collections.Generic;
using System.Linq;

class CacheSvc
{
    private static CacheSvc instance;
    public static CacheSvc Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new CacheSvc();
            }

            return instance;
        }
    }

    private DBMgr dbMgr = DBMgr.Instance;

    public Dictionary<string, ServerSession> onLineAcctDic = new Dictionary<string, ServerSession>();
    public Dictionary<ServerSession, PlayerData> onLineSessionDic = new Dictionary<ServerSession, PlayerData>();

    /// <summary>
    /// 从缓存中查找账号是否存在，判断是否处于在线状态
    /// </summary>
    public bool IsAcctOnline(string accet)
    {
        return onLineAcctDic.ContainsKey(accet);
    }

    /// <summary>
    /// 根据账号和密码获取DB的玩家数据,账号密码错误返回NULL,不存在则创建账号
    /// </summary>
    public PlayerData GetPlayerData(string acct,string pass)
    {
        PlayerData playerData = dbMgr.QueryPlayerData(acct, pass);
        return playerData;
    }

    /// <summary>
    /// 帐号上线,经行缓存
    /// </summary>
    public void AcctOnline(string acct, ServerSession session,PlayerData data)
    {
        onLineAcctDic.Add(acct, session);
        onLineSessionDic.Add(session, data);
    }

    /// <summary>
    /// 从数据库查询是否存在该名称
    /// </summary>
    public bool IsNameExist(string name)
    {
        return dbMgr.QueryNameData(name);
    }

    /// <summary>
    /// 从缓存中根据ServerSession查找对应的数据,有则返回数据，没有则返回NULL
    /// </summary>
    public PlayerData GetPlayerDataBySession(ServerSession session)
    {
        if (onLineSessionDic.TryGetValue(session, out PlayerData data))
        {
            return data;
        }
        return null;
    }

    /// <summary>
    /// 根据ID更新数据库的数据
    /// </summary>
    public bool UpdataPlayerData(int id,PlayerData playerData)
    {
        return dbMgr.UpdatePlayerData(id, playerData);
    }

    public void AcctOffLine(ServerSession session)
    {
        foreach (var item in onLineAcctDic)
        {
            if (item.Value == session)
            {
                onLineAcctDic.Remove(item.Key);
                break;
            }
        }

        bool succ = onLineSessionDic.Remove(session);
        PECommon.Log("offline Result:SessionID :"+session.sessionID + succ);
    }
}
