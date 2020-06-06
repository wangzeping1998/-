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

public class DynamicWind : WindowRoot
{
	public Animator anim;
	public Text txtTips;

	private Queue<string> tipsQue = new Queue<string>();
	private bool isTipsShow = false;
	protected override void InitWind()
	{
		base.InitWind();
		SetActive(txtTips,false);
	}

	public void AddTips(string tips)
	{
		lock (tipsQue)
		{
			tipsQue.Enqueue(tips);
		}
	}

	private void Update()
	{
		if (tipsQue.Count > 0 && isTipsShow == false)
		{
			lock (tipsQue)
			{
				string tips = tipsQue.Dequeue();
				SetTips(tips);
			}
		}
	}

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