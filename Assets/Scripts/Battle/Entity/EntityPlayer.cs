/****************************************************
    文件：EntityPlayer.cs
	作者：wangz
    邮箱: wangzeping1998@gmail.com
    日期：2020/7/13 17:34:54
	功能：Nothing
*****************************************************/

using System.Collections.Generic;
using UnityEngine;

public class EntityPlayer : EntityBase 
{
	public override Vector2 GetMoveDir()
	{
		return battleMgr.GetInputDir();
	}

	public override Vector2 CalcTargetDir()
	{
		EntityMonster em =  GetAtkRangeMonster();
		if (em == null)
		{
			return Vector2.zero;
		}
		
		Vector3 selfPos = GetPos();
		Vector3 monsterPos = em.GetPos();
		Vector2 targetDir = new Vector2(monsterPos.x - selfPos.x,monsterPos. z- selfPos.z);
		return targetDir.normalized;
	}

	private EntityMonster GetAtkRangeMonster()
	{
		List<EntityMonster> monsters = battleMgr.GetMonsters();
		if (monsters.Count <= 0)
		{
			return null;
		}

		Vector3 selfPos = GetPos();
		EntityMonster atkMonster = null;
		float minDis = 0f;

		for (int i = 0; i < monsters.Count; i++)
		{
			if (i == 0)
			{
				float dis = Vector3.Distance(selfPos, monsters[i].GetPos());
				minDis = dis;
				atkMonster = monsters[i];
			}
			else
			{
				float dis = Vector3.Distance(selfPos, monsters[i].GetPos());
				if (dis < minDis)
				{
					minDis = dis;
					atkMonster = monsters[i];
				}
			}
		}

		return atkMonster;
	}
}
