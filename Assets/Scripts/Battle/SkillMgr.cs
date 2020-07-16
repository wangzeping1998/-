/****************************************************
    文件：SkillMgr.cs
	作者：wangz
    邮箱: wangzeping1998@gmail.com
    日期：2020/7/11 21:10:43
	功能：Nothing
*****************************************************/

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

	public void AttackEffect(EntityBase entity,int skillId)
	{
		SkillCfg skillData = resSvc.GetSkillCfg(skillId);
		
		entity.SetAction(skillData.aniAction);	//播放动画
		entity.SetFx(skillData.fx,skillData.skillTime);	//播放特效

		List<int> skillMoveLst = skillData.skillMoveLst;


		
		//技能位移
		long timeSum = 0;

		entity.SetSkillMove(true);
		for (int i = 0; i < skillMoveLst.Count; i++)
		{
			SkillMoveCfg skillMoveData = resSvc.GetSkillMoveCfg(skillMoveLst[i]);
			
			long delayTime = skillMoveData.delayTime;	//延迟释放时间
			float distance = skillMoveData.moveDis;		//移动距离
			long time = skillMoveData.moveTime;			//消耗时间
			float speed = distance / (time / 1000.0f);			//计算 每秒速度 = 距离 / 时间 
			
			//计算延迟
			//开始位移
			timeSum += delayTime;
			timerSvc.AddTimeTask((taskID) =>
			{
				entity.SetSkillMove(true, speed);
			}, timeSum);


			//计算延迟
			//结束位移
			timeSum += time;
			timerSvc.AddTimeTask((taskID) =>
			{
				entity.SetSkillMove(true,0);
			}, timeSum);

			

		}
		//等待技能释放完成回到Idle状态
		timerSvc.AddTimeTask((taskID) =>
		{
			entity.Idle();
			entity.SetSkillMove(false);
		}, skillData.skillTime);

		
		
		
	}
}