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
	public const string SceneLogin = "SceneLogin";	//主城场景加载名称
	public const int mainCityID = 10000;			//主城场景ID

	//音效
	public const string BGLogin = "bgLogin";		//登入背景音乐
	public const string BGMainCity = "bgMainCity";	//主城背景音乐
	public const string BGHuangye = "bgHuangYe";	//主城背景音乐
	
	
	public const string UILoginBtn = "uiLoginBtn";	//登入按钮音效
	public const string UIClickBtn = "uiClickBtn";	//常规UI点击音效
	public const string UIExtenBtn = "uiExtenBtn";
	public const string UIOpenPage = "uiOpenPage";	//角色信息UI点击音效
	public const string UIFbitem = "fbitem";
	
	//屏幕标准宽高
	public const int ScreenStandardWidth = 1334;
	public const int ScreenStandardHeight = 750;
	
	//屏幕摇杆操作距离
	public const int ScreenOPDis = 90;
	
	//角色移动速度
	public const int PlayerMoveSpeed = 8;
	public const int MonsterMoveSpeed = 4;
	
	public const float AccelerSpeed = 5;
	public const float AccelerHPBar = 0.5f;

	//技能ID
	public const int AttackSkillID_101 = 101;
	public const int AttackSkillID_102 = 102;
	public const int AttackSkillID_103 = 103;

	//Action触发参数
	public const int ActionDefault = -1;
	public const int ActionBorn = 0;
	public const int ActionDie = 100;
	public const int ActionHit = 101;

	public const int DieAnimLength = 5000;

	public const int ComboSpace = 500;
	

	//混合动画参数
	public const int BlendIdle = 0;
	public const int BlendWalk = 1;
	public const int AttackAct = 1;

	//AudoGuideNpc
	public const int NPCGuide = -1;
	public const int NPCWiseMan = 0;
	public const int NPCGeneral = 1;
	public const int NPCArtisan = 2;
	public const int NPCTrader = 3;

	public static string Color(string str, TxtColor color = TxtColor.none)
	{
		switch (color)
		{
			case TxtColor.Red:
				return string.Format("<color=red>{0}</color>",str);
			case TxtColor.Green:
				return string.Format("<color=green>{0}</color>",str);
			case TxtColor.Blue:
				return string.Format("<color=blue>{0}</color>",str);
			default:
				return str;
		}
	}
}

public enum TxtColor
{
	none,
	Red,
	Green,
	Blue
}

public enum DamageType
{
	None,
	AD = 1,
	AP = 2,
}