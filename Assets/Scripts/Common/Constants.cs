/****************************************************
    文件：Constants.cs
	作者：wangz
    邮箱: wangzeping1998@gmail.com
    日期：2020/6/5 14:33:28
	功能：常量配置
*****************************************************/

using UnityEngine;

public class Constants
{
	
	//场景名称
	public const string SceneLogin = "SceneLogin";
	public const int mainCityID = 10000; 
	//public const string SceneMainCity = "SceneMainCity";
	
	//音效
	public const string BGLogin = "bgLogin";
	public const string BGMainCity = "bgMainCity";
	//登入按钮音效
	public const string UILoginBtn = "uiLoginBtn";
	//常规UI点击音效
	public const string UIClickBtn = "uiClickBtn";

	public const string UIExtenBtn = "uiExtenBtn";
	//屏幕标准宽高
	public const int ScreenStandardWidth = 1334;
	public const int ScreenStandardHeight = 750;
	//屏幕摇杆操作距离
	public const int ScreenOPDis = 90;
	//角色移动速度
	public const int PlayerMoveSpeed = 8;
	public const int MonsterMoveSpeed = 4;

	public const float AccelerSpeed = 5;
	
	public const int BlendIdle = 0;
	public const int BlendWalk = 1;

}