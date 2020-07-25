/****************************************************
    文件：SkillMgr.cs
	作者：wangz
    邮箱: wangzeping1998@gmail.com
    日期：2020/7/11 21:10:43
	功能：技能管理器
*****************************************************/

using System;
using System.Collections.Generic;
using UnityEngine;

public class SkillMgr : MonoBehaviour
{
    private ResSvc resSvc = null;
    private TimerSvc timerSvc = null;

    public void Init()
    {
        resSvc = ResSvc.instance;
        timerSvc = TimerSvc.instance;
    }


    //技能计时、判断和伤害
    public void AttackDamage(EntityBase entity, int skillId)
    {
        SkillCfg skillData = resSvc.GetSkillCfg(skillId);
        List<int> skillActionLst = skillData.skillActionLst;

        long timeSum = 0;
        //计算多段伤害
        for (int i = 0; i < skillActionLst.Count; i++)
        {
            SkillActionCfg sac = resSvc.GetSkillActionCfg(skillActionLst[i]);
            int index = i;
            timeSum += sac.delayTime;
            if (sac.delayTime > 0)
            {
                //有延迟
                timerSvc.AddTimeTask((taskId) => { AttackAction(entity, skillData, index); }, timeSum /*延迟时间*/);
            }
            else
            {
                //无延迟
                AttackAction(entity, skillData, index);
            }
        }
    }

    //判定
    public void AttackAction(EntityBase entity, SkillCfg skillCfg, int index)
    {
        List<EntityMonster> monsterLst = entity.battleMgr.GetMonsters();
        SkillActionCfg sac = resSvc.GetSkillActionCfg(skillCfg.skillActionLst[index]);
        int damage = skillCfg.skillDamageLst[index];
        //对列表所有怪物进行判断
        for (int i = 0; i < monsterLst.Count; i++)
        {
            //判断距离Range
            if (IsInRange(entity, monsterLst[i], sac.radius))
            {
                //判断角度Angle
                if (IsInAngle(entity, monsterLst[i], sac.angle))
                {
                    CalcDamge(entity, monsterLst[i], skillCfg, damage);
                }
            }
        }
    }

    private System.Random rd = new System.Random();

    //计算伤害
    public void CalcDamge(EntityBase self, EntityBase target, SkillCfg skillCfg, int damage)
    {
        int damageSum = damage;

        if (skillCfg.dmgType == DamageType.AD)
        {
            //物理伤害
            //计算目标闪避
            int dodgeNum = PETools.RDInit(1, 100, rd);
            if (dodgeNum <= target.battleProps.dodge)
            {
                PECommon.Log("闪避:" + target.battleProps.dodge + "/" + dodgeNum);
                target.Dodge();
                return;
            }

            //计算属性加成伤害
            damageSum += self.battleProps.ad;

            //计算暴击
            int criticalNum = PETools.RDInit(1, 100, rd);
            if (criticalNum < self.battleProps.critical)
            {
                //计算暴击伤害倍率 1.5f~2.0f
                float hitDoubly = 1.0f + PETools.RDInit(50, 100, rd) / 100f;
                damageSum = (int) (damageSum * hitDoubly);
                target.Critical(damageSum);
                PECommon.Log("闪避:" + target.battleProps.critical + "/" + criticalNum);
            }

            //计算穿甲
            int adPierce = (int) ((1 - self.battleProps.pierce / 100f) * target.battleProps.addef);
            damageSum -= adPierce;
        }
        else if (skillCfg.dmgType == DamageType.AP)
        {
            //魔法伤害
            //计算属性加成伤害
            damageSum += self.battleProps.ap;
            //计算魔法抗性
            damageSum -= self.battleProps.apdef;
        }
        else
        {
        }

        //最终伤害
        if (damageSum < 0)
        {
            damageSum = 0;
        }

        if (target.HP <= damageSum)
        {
            target.HP = 0;
            target.Die();
            target.Remove();
        }
        else
        {
            target.HP -= damageSum;
            target.Hit();
            target.Hurt(damageSum);
        }

        PECommon.Log("Damage:" + damageSum, LogType.Warn);
    }

    //判断距离
    public bool IsInRange(EntityBase self, EntityBase target, float range)
    {
        float dis = Vector3.Distance(self.GetPos(), target.GetPos());
        return dis < range;
    }

    //判断角度
    public bool IsInAngle(EntityBase self, EntityBase target, float angle)
    {
        if (angle >= 360)
        {
            return true;
        }

        Vector3 from = self.GetTrans().forward;
        Vector3 dir = target.GetPos() - self.GetPos();
        float currentAngle = Vector3.Angle(from, dir.normalized);

        return currentAngle < angle * 0.5;
    }

    //技能特效与位移
    public void AttackEffect(EntityBase entity, int skillId)
    {
        if (entity.GetMoveDir() == Vector2.zero)
        {
            Vector2 dir = entity.CalcTargetDir();
            //自动攻击最近怪物
            if ( dir != Vector2.zero)
            {
                entity.SetAtkDir(dir,false);
            }
        }
        else
        {
            entity.SetAtkDir(entity.GetMoveDir());
        }
        SkillCfg skillData = resSvc.GetSkillCfg(skillId);

        entity.SetAction(skillData.aniAction); //播放动画
        entity.SetFx(skillData.fx, skillData.skillTime); //播放特效

        List<int> skillMoveLst = skillData.skillMoveLst;


        //技能位移
        long timeSum = 0;

        entity.SetSkillMove(true);
        entity.SetIsCtrl(false);
        for (int i = 0; i < skillMoveLst.Count; i++)
        {
            SkillMoveCfg skillMoveData = resSvc.GetSkillMoveCfg(skillMoveLst[i]);

            long delayTime = skillMoveData.delayTime; //延迟释放时间
            float distance = skillMoveData.moveDis; //移动距离
            long time = skillMoveData.moveTime; //消耗时间
            float speed = distance / (time / 1000.0f); //计算 每秒速度 = 距离 / 时间 

            //计算延迟
            //开始位移
            timeSum += delayTime;
            timerSvc.AddTimeTask((taskID) => { entity.SetSkillMove(true, speed); }, timeSum);


            //计算延迟
            //结束位移
            timeSum += time;
            timerSvc.AddTimeTask((taskID) => { entity.SetSkillMove(true, 0); }, timeSum);
        }

        //等待技能释放完成回到Idle状态
        timerSvc.AddTimeTask((taskID) =>
        {
            entity.SetSkillMove(false);
            entity.SetIsCtrl(true);
            entity.Idle();
        }, skillData.skillTime);
    }
}