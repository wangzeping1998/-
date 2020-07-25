/****************************************************
    文件：TimeSvc.cs
	作者：wangz
    邮箱: wangzeping1998@gmail.com
    日期：2020/7/6 16:52:12
	功能：计时器服务
*****************************************************/

using System;
using UnityEngine;

public class TimerSvc : MonoBehaviour 
{
	public static TimerSvc instance = null;

	private PETimer pt;
	
	public void InitSvc()
	{
		instance = this;
		pt = new PETimer();
		//设置日志输出
		pt.SetLog((info) =>
		{
			PECommon.Log(info);
		});
		Debug.Log("TimeSvc ResSvc...");
	}

	public int AddTimeTask(Action<int> cb,double delay,PETimeUnit timeUnit = PETimeUnit.Millisecond,int replay=1)
	{
		return pt.AddTimeTask(cb, delay, timeUnit, replay);
	}

	//Monobehaviour
	private void Update()
	{
		pt.Update();
	}

	public double GetNowTime()
	{
		return pt.GetMillisecondsTime();
	}
	
	
}