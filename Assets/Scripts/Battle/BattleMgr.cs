/****************************************************
    文件：BattleMgr.cs
	作者：wangz
    邮箱: wangzeping1998@gmail.com
    日期：2020/7/11 20:2:31
	功能：战斗管理器
*****************************************************/

using UnityEngine;

public class BattleMgr : MonoBehaviour
{
    protected ResSvc resSvc;
    protected AudioSvc audioSvc;

    protected StateMgr stateMgr;
    protected SkillMgr skillMgr;
    protected MapMgr mapMgr;

    private EntityPlayer entityPlayer;

    public void Init(int mapId)
    {
        resSvc = ResSvc.instance;
        audioSvc = AudioSvc.instance;

        stateMgr = this.gameObject.AddComponent<StateMgr>();
        stateMgr.Init();
        skillMgr = this.gameObject.AddComponent<SkillMgr>();
        skillMgr.Init();


        MapCfg mc = resSvc.GetMapCfgData(mapId);
        resSvc.AsyncLoadScene(mc.sceneName, () =>
        {
            GameObject mapObj = GameObject.FindWithTag("MapRoot");
            mapMgr = mapObj.GetComponent<MapMgr>();
            mapMgr.Init();
            audioSvc.PlayBgMusic(Constants.BGHuangye);

            mapObj.transform.position = Vector3.zero;
            mapObj.transform.localScale = Vector3.one;

            Camera.main.transform.position = mc.mainCamPos;
            Camera.main.transform.localEulerAngles = mc.mainCamRote;

            LoadPlayer(mc);
        });
    }

    private void LoadPlayer(MapCfg mapData)
    {
        GameObject player = resSvc.LoadPrefab(PathDefine.AssassinBattlePlayerPrefab);

        player.transform.position = mapData.playerBornPos;
        player.transform.localEulerAngles = mapData.playerBornRote;
        player.transform.localScale = Vector3.one;
        
        PlayerController playerCtrl = player.GetComponent<PlayerController>();
        playerCtrl.Init();
        
        //生成角色赋予 状态管理器
        entityPlayer = new EntityPlayer();
        entityPlayer.SetStateMgr(this.stateMgr);
        entityPlayer.SetController(playerCtrl);
    }

    public void SetSelfPlayerMoveDir(Vector2 dir)
    {
        if (dir == Vector2.zero)
        {
            entityPlayer.Idle();
        }
        else
        {
            entityPlayer.Move();
        }
    }

    public void ReqReleaseSkill(int index)
    {
        switch (index)
        {
            case 0:
                ReleasNormalAttack();
                break;
            case 1:
                ReleasSkill1();
                break;
            case 2:
                ReleasSkill2();
                break;
            case 3:
                ReleasSkill3();
                break;
        }
    }

    public void ReleasNormalAttack()
    {
        PECommon.Log("ReleasAttack");
    }
    
    public void ReleasSkill1()
    {
        PECommon.Log("ReleasSkill1");
    }

    public void ReleasSkill2()
    {
        PECommon.Log("ReleasSkill2");
    }

    public void ReleasSkill3()
    {
        PECommon.Log("ReleasSkill2");
    }
}