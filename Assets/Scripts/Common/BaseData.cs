/****************************************************
    文件：BaseData.cs
	作者：wangz
    邮箱: wangzeping1998@gmail.com
    日期：2020/6/19 9:46:19
	功能：配置表数据结构
*****************************************************/

using System.Collections.Generic;
using UnityEngine;

public class BaseData<T>
{
    public int id;
}

/// <summary>
/// 地图配置数据结构类
/// </summary>
public class MapCfg : BaseData<MapCfg>
{
    public string mapName;           //地图名称 
    public string sceneName;         //场景名称
    public int power;                //进入地图消耗的体力
    public Vector3 mainCamPos;       //相机位置
    public Vector3 mainCamRote;      //相机旋转
    public Vector3 playerBornPos;    //角色生成位置
    public Vector3 playerBornRote;   //角色生成旋转
    public List<MonsterSpawnData> monsterLst;    //地图怪物生成信息
}

/// <summary>
/// 地图怪物 数据结构
/// </summary>
public class MonsterSpawnData : BaseData<MonsterSpawnData>
{
    public int mWave;    //波次\轮
    public int mIndex;    //序号
    public int mLevel;    //等级
    public MonsterCfg mCfg;    //怪物信息
    public Vector3 mBornpos;    //生成位置
    public Vector3 mBornRote;    //生成朝向
}

/// <summary>
/// 怪物配置
/// </summary>
public class MonsterCfg : BaseData<MonsterCfg>
{
    public string mName;    //怪物名称
    public string resPath;    //怪物预制体路径
    public int skillID;
    public float atkDis;
    public BattleProps bps;
}

/// <summary>
/// 引导任务配置数据结构类
/// </summary>
public class AutoGuideCfg : BaseData<AutoGuideCfg>
{
    public int npcID;            //触发任务目标NPC索引号
    public string dilogArr;      //对话内容
    public int actID;            //完成后新的任务ID
    public int coin;             //金币
    public int exp;              //经验
}

/// <summary>
/// 强化配置
/// </summary>
public class StrongCfg : BaseData<StrongCfg>
{
    public int pos;                //位置
    public int starLv;             //星级
    public int addhp;              //增加生命值
    public int addhurt;            //增加伤害
    public int adddef;             //增加防御
    public int minlv;              //最小等级
    public int coin;               //消耗金币
    public int crystal;            //消耗材料
}

/// <summary>
/// 任务奖励配置
/// </summary>
public class TaskRewardCfg : BaseData<TaskRewardCfg>
{
    public string taskName;    //任务名称
    public int count;          //需要完成次数
    public int exp;            //任务经验
    public int coin;           //任务金币
}
/// <summary>
/// 任务奖励数据配置
/// </summary>
public class TaskRewardData : BaseData<TaskRewardData>
{
    public int prgs;            //任务完成进度
    public bool taked;          //奖励是否已被领取
}

/// <summary>
/// 技能配置
/// </summary>
public class SkillCfg : BaseData<SkillCfg>
{
    public string skillName;    //技能名称
    public long skillTime;      //持续时间
    public int aniAction;       //动作ID
    public string fx;            //特效路径
    public List<int> skillMoveLst;    //位移列表
    public List<int> skillActionLst;    //技能判断列表
    public List<int> skillDamageLst;    //技能伤害列表
    public DamageType dmgType;    //伤害类型
}
/// <summary>
/// 技能位移配置
/// </summary>
public class SkillMoveCfg : BaseData<SkillMoveCfg>
{
    public long moveTime;    //移动持续时间
    public float moveDis;    //移动距离
    public long delayTime;    //延迟时间
}

public class SkillActionCfg : BaseData<SkillActionCfg>
{
    public long delayTime;
    public float radius;
    public float angle;
}


public class BattleProps
{
    public int hp;
    public int ad;
    public int ap;
    public int addef;
    public int apdef;
    public int dodge;
    public int pierce;
    public int critical;
}