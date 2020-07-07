/****************************************************
    文件：DynamicWind.cs
	作者：wangz
    邮箱: wangzeping1998@gmail.com
    日期：2020/6/5 16:55:9
	功能：动态UI元素界面
*****************************************************/

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


//Dynamic几乎全局处于激活状态
//因为在各个场景中都可能需要使用它
public class DynamicWind : WindowRoot
{
	public Animator anim;
	public Text txtTips;

	private Queue<string> tipsQue = new Queue<string>();	//提示队列
	private bool isTipsShow = false;	//提示标志位
	protected override void InitWind()
	{
		base.InitWind();
		SetActive(txtTips,false);
	}

	//向提示队列中添加需要提示的文字内容
	//每帧尝试提取队列中的文字内容进行播放显示
	public void AddTips(string tips)
	{
		lock (tipsQue)
		{
			tipsQue.Enqueue(tips);
		}
	}

	
	private void Update()
	{
		//每帧从队列中拿到一条提示内容
		//如果提示动画未播放完即动画正在播放时 isTipsShow一直处于ture状态，则不提取消息，等待提示动画播放完后，isTipsShow处于false后，开始提取消息
		if (tipsQue.Count > 0 && isTipsShow == false)
		{
			lock (tipsQue)
			{
				string tips = tipsQue.Dequeue();
				SetTips(tips);
			}
		}
	}

	//设置提示信息
	public void SetTips(string tips)
	{
		isTipsShow = true;
		SetActive(txtTips);
		SetText(txtTips,tips);
		
		anim.SetTrigger("show");
		float timeLen = anim.runtimeAnimatorController.animationClips[0].length;
		Debug.Log(timeLen);
		StartCoroutine(animPlayDone(timeLen, () =>
		{
			//动画播放完成，将标志位设置成false状态
			SetActive(txtTips,false);
			isTipsShow = false;
		}));
	}

	
	IEnumerator animPlayDone(float sec, Action cb)
	{
		yield return new  WaitForSeconds(sec);
		if (cb!=null)
		{
			cb.Invoke();
		}
	}
}