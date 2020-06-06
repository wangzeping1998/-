/****************************************************
    文件：PETools.cs
	作者：wangz
    邮箱: wangzeping1998@gmail.com
    日期：2020/6/6 15:23:0
	功能：工具类
*****************************************************/

using System;
using UnityEngine;

public class PETools  
{
	public static int RDInit(int min,int max,System.Random rd=null)
	{
		if (rd == null)
		{
			rd = new System.Random();
		}
		int val = rd.Next(min, max + 1);
		return val;
	}
}