/****************************************************
    文件：EntityMonster.cs
	作者：wangz
    邮箱: wangzeping1998@gmail.com
    日期：2020/7/18 0:21:18
	功能：怪物实体基类
*****************************************************/

using UnityEngine;

public class EntityMonster : EntityBase
{
    private MonsterSpawnData msd = null;

    public float checkTime = 2f; //检查间隔
    public float checkCountTime = 0; //检查计时

    public float atkCheckTime = 0; //攻击计时
    public float atkTime = 2f; //攻击间隔

    public bool runAI = true; //是否启动AI
    
    public EntityMonster()
    {
        enitityType = EnitityType.Monster;
    }

    public void SetMonsterCfg(MonsterSpawnData monsterSpawnData)
    {
        this.msd = monsterSpawnData;
    }

    public override void SetBattleProps(BattleProps battleProps)
    {
        int level = msd.mLevel;

        base.battleProps = new BattleProps()
        {
            hp = battleProps.hp * level,
            ad = battleProps.ad * level,
            ap = battleProps.ap * level,
            addef = battleProps.addef * level,
            apdef = battleProps.apdef * level,
            critical = battleProps.critical * level,
            dodge = battleProps.dodge * level,
            pierce = battleProps.pierce * level,
        };

        HP = base.battleProps.hp;
    }

    //移除
    public override void Remove()
    {
        this.battleMgr.RemoveMonster(Name);
        GameRoot.instance.dynamicWind.RemoveHPItemInfo(Name);
    }


    public override void TickAILogic()
    {
        if (runAI == false)
        {
            return;
        }
        
        if (currentAnimState == AniState.Idle || currentAnimState == AniState.Move)
        {
            float delta = Time.deltaTime;
            checkCountTime += delta;
            if (checkCountTime < checkTime)
            {
                return;
            }
        
            Vector2 dir = CalcTargetDir();
        
            //判断是否在攻击范围内
            if (InAttackRange())
            {
                SetDir(Vector2.zero);
                atkCheckTime += delta;
                if (atkCheckTime > atkTime)
                {
                    //攻击
                    SetAtkDir(dir,false);
                    Attack(msd.mCfg.skillID);
                    atkCheckTime = 0;
                }
                else
                {
                    //休息
                    Idle();
                }
            }
            else
            {
                //移动
                SetDir(dir);
                Move();
            }

            checkCountTime = 0;
            checkTime = PETools.RDInit(1, 5) * 0.1f / 10;
        }
        

    }

    //获得目标方向
    public override Vector2 CalcTargetDir()
    {
        EntityPlayer enitityPlayer = battleMgr.GetEnitityPlayer();

        if (enitityPlayer == null || enitityPlayer.currentAnimState == AniState.Die)
        {
            runAI = false;
            return Vector2.zero;
        }

        Vector3 playerPos = enitityPlayer.GetPos();
        Vector3 selfPos = GetPos();
        Vector2 dir = new Vector2(playerPos.x, playerPos.z) - new Vector2(selfPos.x, selfPos.z);
        return dir.normalized;
    }

    //是否在攻击范围内
    public bool InAttackRange()
    {
        EntityPlayer enitityPlayer = battleMgr.GetEnitityPlayer();
        Vector3 playerPos = enitityPlayer.GetPos();
        Vector3 selfPos = GetPos();
        float dis = Vector2.Distance(new Vector2(playerPos.x, playerPos.z), new Vector2(selfPos.x, selfPos.z));
        return dis < msd.mCfg.atkDis;
    }

    public override bool GetBreakState()
    {
        if (msd.mCfg.isStop)
        {
            if (curtSkillCfg != null)
            {
                return curtSkillCfg.isBreak;
            }
            else
            {
                return true;
            }
        }
        else
        {
            return false;
        }
    }
}