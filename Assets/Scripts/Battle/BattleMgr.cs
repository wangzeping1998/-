/****************************************************
    文件：BattleMgr.cs
	作者：wangz
    邮箱: wangzeping1998@gmail.com
    日期：2020/7/11 20:2:31
	功能：战斗管理器
*****************************************************/

using System.Collections.Generic;
using PEProtocol;
using UnityEngine;

public class BattleMgr : MonoBehaviour
{
    protected ResSvc resSvc;
    protected AudioSvc audioSvc;
    protected TimerSvc timerSvc;

    protected StateMgr stateMgr;
    protected SkillMgr skillMgr;
    protected MapMgr mapMgr;

    private EntityPlayer entityPlayer;

    private MapCfg mapCfg;

    private Dictionary<string, EntityMonster> monsterDic = new Dictionary<string, EntityMonster>();

    public void Init(int mapId)
    {
        resSvc = ResSvc.instance;
        audioSvc = AudioSvc.instance;
        timerSvc = TimerSvc.instance;

        stateMgr = this.gameObject.AddComponent<StateMgr>();
        stateMgr.Init();
        skillMgr = this.gameObject.AddComponent<SkillMgr>();
        skillMgr.Init();


        mapCfg = resSvc.GetMapCfgData(mapId);
        resSvc.AsyncLoadScene(mapCfg.sceneName, () =>
        {
            GameObject mapObj = GameObject.FindWithTag("MapRoot");
            mapMgr = mapObj.GetComponent<MapMgr>();
            mapMgr.Init();
            audioSvc.PlayBgMusic(Constants.BGHuangye);

            mapObj.transform.position = Vector3.zero;
            mapObj.transform.localScale = Vector3.one;

            Camera.main.transform.position = mapCfg.mainCamPos;
            Camera.main.transform.localEulerAngles = mapCfg.mainCamRote;

            LoadPlayer(mapCfg);
            
            SetActiveCurrentBatchMonsters();
        });
    }

    //加载玩家
    private void LoadPlayer(MapCfg mapData)
    {
        GameObject player = resSvc.LoadPrefab(PathDefine.AssassinBattlePlayerPrefab);

        player.transform.position = mapData.playerBornPos;
        player.transform.localEulerAngles = mapData.playerBornRote;
        player.transform.localScale = Vector3.one;

        PlayerController playerCtrl = player.GetComponent<PlayerController>();
        playerCtrl.Init();

        PlayerData pd = GameRoot.instance.playerData;
        BattleProps props = new BattleProps()
        {
            hp = pd.hp,
            ad = pd.ad,
            ap = pd.ap,
            addef = pd.addef,
            apdef = pd.apdef,
            dodge = pd.dodge,
            pierce = pd.pierce,
            critical = pd.critical,
        };

        //生成角色赋予 状态管理器
        entityPlayer = new EntityPlayer();
        entityPlayer.SetBattleProps(props);
        entityPlayer.SetBattleMgr(this); //添加战斗管理器
        entityPlayer.SetStateMgr(this.stateMgr); //添加状态管理器
        entityPlayer.SetSkillMgr(this.skillMgr); //添加技能管理器
        entityPlayer.SetController(playerCtrl); //添加角色控制器
        entityPlayer.Idle();
    }

    //加载怪物
    public void LoadMonster(int wave)
    {
        List<MonsterSpawnData> monsterLst = this.mapCfg.monsterLst;
        for (int i = 0; i < monsterLst.Count; i++)
        {
            if (monsterLst[i].mWave == wave)
            {
                MonsterSpawnData msd = monsterLst[i];
                GameObject go = resSvc.LoadPrefab(msd.mCfg.resPath);
                go.transform.localPosition = msd.mBornpos;
                go.transform.localEulerAngles = msd.mBornRote;
                go.transform.name = string.Format("{0}_{1}_{2}", msd.mCfg.id, msd.mWave, msd.mIndex);

                MonsterController monsterCtrl = go.GetComponent<MonsterController>();

                EntityMonster entityMonster = new EntityMonster();
                entityMonster.SetMonsterCfg(msd);
                entityMonster.SetStateMgr(this.stateMgr);
                entityMonster.SetBattleProps(msd.mCfg.bps);
                entityMonster.SetController(monsterCtrl);
                monsterDic.Add(go.name, entityMonster);
                go.SetActive(false);
            }
        }
    }

    //获得当前批次怪物列表
    public List<EntityMonster> GetMonsters()
    {
        List<EntityMonster> emLst = new List<EntityMonster>();
        foreach (var m in monsterDic)
        {
            emLst.Add(m.Value);
        }

        return emLst;
    }

    //显示当前批次的怪物
    public void SetActiveCurrentBatchMonsters()
    {
        timerSvc.AddTimeTask((taskId) =>
        {
            foreach (var monster in monsterDic)
            {
                monster.Value.controller.gameObject.SetActive(true);
                monster.Value.Born();
                timerSvc.AddTimeTask((ttid) =>
                {
                    monster.Value.Idle();
                }, 1000);
            }
        }, 500);
    }

    #region 玩家操作移动与释放技能

    public void SetSelfPlayerMoveDir(Vector2 dir)
    {
        if (dir == Vector2.zero)
        {
            entityPlayer.Idle();
        }
        else
        {
            entityPlayer.Move();
            entityPlayer.SetDir(dir);
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
        entityPlayer.Attack(Constants.AttackSkillID);
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

    public Vector2 GetInputDir()
    {
        return BattleSys.instance.GetInputDir();
    }

    #endregion
   
}