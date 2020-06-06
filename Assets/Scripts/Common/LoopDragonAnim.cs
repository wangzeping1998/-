/****************************************************
    文件：LoopDragonAnim.cs
	作者：wangz
    邮箱: wangzeping1998@gmail.com
    日期：2020/6/5 16:46:21
	功能：Nothing
*****************************************************/

using System;
using UnityEngine;

public class LoopDragonAnim : MonoBehaviour
{
	private Animation anim;

	private void Awake()
	{
		anim = GetComponent<Animation>();
		
	}

	private void Start()
	{
		if (anim != null)
		{
			InvokeRepeating("PlayDragonAnim",0,25);
		}
	}

	private void PlayDragonAnim()
	{
		if (anim != null)
		{
			
			anim.Play();
		}
	}
}