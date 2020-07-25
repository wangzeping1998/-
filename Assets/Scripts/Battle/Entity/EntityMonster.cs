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
			ad = battleProps.ad* level,
			ap = battleProps.ap* level,
			addef = battleProps.addef* level,
			apdef = battleProps.apdef* level,
			critical = battleProps.critical* level,
			dodge = battleProps.dodge* level,
			pierce = battleProps.pierce* level,
		};

		HP = base.battleProps.hp;
	}
	
	//移除
	public override void Remove()
	{
		this.battleMgr.RemoveMonster(Name);
		GameRoot.instance.dynamicWind.RemoveHPItemInfo(Name);
	}
	
}