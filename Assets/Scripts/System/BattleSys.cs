/****************************************************
    文件：BattleSys.cs
	作者：wangz
    邮箱: wangzeping1998@gmail.com
    日期：2020/7/11 19:58:49
	功能：战斗系统
*****************************************************/

using UnityEngine;

public class BattleSys : SystemRoot
{
    public static BattleSys instance = null;

    public PlayerCtrlWind playerCtrlWind;
    private BattleMgr battleMgr;

    public override void InitSys()
    {
        base.InitSys();
        instance = this;
    }

    public void StartBattle(int mapId)
    {
        GameObject go = new GameObject()
        {
            name = "BattleMgr"
        };
        go.transform.SetParent(GameRoot.instance.transform);
        battleMgr = go.AddComponent<BattleMgr>();
        battleMgr.Init(mapId);
        SetPlayerCtrlWindState();
    }

    public void SetPlayerCtrlWindState(bool isActive = true)
    {
        playerCtrlWind.SetWindowState(isActive);
    }

    public void SetSelfPlayerMoveDir(Vector2 dir)
    {
        battleMgr.SetSelfPlayerMoveDir(dir);
    }

    public void ReqReleaseSkill(int index)
    {
        battleMgr.ReqReleaseSkill(index);
    }
}