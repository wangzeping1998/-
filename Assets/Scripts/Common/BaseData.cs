/****************************************************
    文件：BaseData.cs
	作者：wangz
    邮箱: wangzeping1998@gmail.com
    日期：2020/6/19 9:46:19
	功能：配置表数据结构
*****************************************************/

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

//任务奖励配置
public class TaskRewardCfg : BaseData<TaskRewardCfg>
{
    public string taskName;
    public int count;
    public int exp;
    public int coin;
}
//任务奖励数据配置
public class TaskRewardData : BaseData<TaskRewardData>
{
    public int prgs;
    public bool taked;
}