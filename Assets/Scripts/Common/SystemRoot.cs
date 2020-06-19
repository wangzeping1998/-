/****************************************************
    文件：SystemRoot.cs
	作者：wangz
    邮箱: wangzeping1998@gmail.com
    日期：2020/6/5 16:38:4
	功能：业务系统基类
*****************************************************/

using UnityEngine;

public class SystemRoot : MonoBehaviour
{
	protected ResSvc resSvc;
	protected AudioSvc audioSvc;
	protected NetSvc netSvc;

	public virtual void InitSys()
	{
		resSvc = ResSvc.instance;
		audioSvc = AudioSvc.instance;
		netSvc = NetSvc.instance;
	}
}