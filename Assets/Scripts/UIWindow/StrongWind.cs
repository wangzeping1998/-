/****************************************************
    文件：StrongWind.cs
	作者：wangz
    邮箱: wangzeping1998@gmail.com
    日期：2020/6/27 13:24:21
	功能：强化界面UI逻辑
*****************************************************/

using System.Collections.Generic;
using PEProtocol;
using UnityEngine;
using UnityEngine.UI;

public class StrongWind : WindowRoot
{
	public Image imgCurtPos;
	public Text txtStarLv;
	public Transform starTransGrp;
	public Text propHp1;
	public Text propHurt1;
	public Text propDef1;
	public Text propHp2;
	public Text propHurt2;
	public Text propDef2;
	
	public Image propArr1;
	public Image propArr2;
	public Image propArr3;
	
	public Text txtNeedLv;
	public Text txtCostCoin;
	public Text txtCostCrystal;

	public Text txtCoin;
	
	public Transform posBtnTrans;
	public Transform costInfoTrans;
	public Button btnStrong;
	public Button btnClose;
	public Image[] imgs;
	private int m_curtIndex;
	private StrongCfg nextSc;
	private PlayerData m_pd;
	protected override void InitWind()
	{
		base.InitWind();
		m_pd = GameRoot.instance.playerData;
		btnStrong.onClick.AddListener(OnClickStrongBtn);
		btnClose.onClick.AddListener(OnClickCloseBtn);
		RegClickEvts();
		ClickPosItem(0);

	}

	protected override void ClearWind()
	{
		base.ClearWind();
		btnStrong.onClick.RemoveListener(OnClickStrongBtn);
		btnClose.onClick.RemoveListener(OnClickCloseBtn);
	}

	private void RegClickEvts()
	{
		imgs = new Image[posBtnTrans.childCount];
		//注册点击索引
		for (int i = 0; i < posBtnTrans.childCount; i++)
		{
			Image img = posBtnTrans.GetChild(i).GetComponent<Image>();
			OnClick(img.gameObject, (args) =>
			{
				audioSvc.PlayUIAudio(Constants.UIClickBtn);
				ClickPosItem((int)args);
			},i);
			imgs[i] = img;
			Debug.Log(img.transform.localPosition);
		}
	}

	private void ClickPosItem(int index)
	{
		PECommon.Log("Click Item" + index);
		m_curtIndex = index;
		
		//切换选择强化部位
		for (int i = 0; i < imgs.Length; i++)
		{
			Transform trans = imgs[i].transform;
			if (i == m_curtIndex)
			{
				SetSprite(imgs[i],PathDefine.ItemArrowBG);
				trans.GetComponent<RectTransform>().sizeDelta = new Vector2(258.85f,98.09f);
				trans.localPosition = new Vector3(11.3f,trans.localPosition.y,0);

			}
			else
			{
				SetSprite(imgs[i],PathDefine.ItemPlatBG);
				trans.GetComponent<RectTransform>().sizeDelta = new Vector2(230f,75f);
				trans.localPosition = new Vector3(0,trans.localPosition.y,0);

			}
		}

		RefreshItem();
	}

	//刷新数据
	private void RefreshItem()
	{
		//金币
		SetText(txtCoin,m_pd.coin);
		switch (m_curtIndex)
		{
			case 0:
				SetSprite(imgCurtPos,PathDefine.ItemToukui);
				break;
			case 1:
				SetSprite(imgCurtPos,PathDefine.ItemBody);
				break;
			case 2:
				SetSprite(imgCurtPos,PathDefine.ItemYaobu);
				break;
			case 3:
				SetSprite(imgCurtPos,PathDefine.ItemHand);
				break;
			case 4:
				SetSprite(imgCurtPos,PathDefine.ItemLeg);
				break;
			case 5:
				SetSprite(imgCurtPos,PathDefine.ItemFoot);
				break;
		}
		SetText(txtStarLv,m_pd.strongArr[m_curtIndex] + "星级");
		int curtStarLv = m_pd.strongArr[m_curtIndex];
		//星级图片
		for (int i = 0; i <starTransGrp.childCount ; i++)
		{
			Image img = starTransGrp.GetChild(i).GetComponent<Image>();
			if (i < curtStarLv)
			{
				SetSprite(img,PathDefine.SpStar2);
			}
			else
			{
				SetSprite(img,PathDefine.SpStar1);
			}
		}

		//查询 位置 星级 的属性
		int sumAddHp = resSvc.GetPropAddValPreLv(m_curtIndex, curtStarLv, 1);
		int sumAddHurt = resSvc.GetPropAddValPreLv(m_curtIndex, curtStarLv, 2);
		int sumAddDef = resSvc.GetPropAddValPreLv(m_curtIndex, curtStarLv, 3);
		SetText(propHp1,"生命"+sumAddHp);
		SetText(propHurt1,"伤害"+sumAddHurt);
		SetText(propDef1,"防御"+sumAddDef);

		int nextStarLv = curtStarLv + 1; 
		nextSc = resSvc.GetStrongCfgData(m_curtIndex, nextStarLv);
		if (nextSc != null)
		{
			//显示“强化后”加成内容
			SetActive(costInfoTrans);
			SetActive(propArr1);
			SetActive(propArr2);
			SetActive(propArr3);
			SetActive(propHp2);
			SetActive(propHurt2);
			SetActive(propDef2);
			//强化后属性加成
			SetText(propHp2,"强化后+"+nextSc.addhp);
			SetText(propHurt2,"强化后+"+nextSc.addhurt);
			SetText(propDef2,"强化后+"+nextSc.adddef);
			//强化条件提示
			SetText(txtNeedLv, "需要等级" + nextSc.minlv);
			SetText(txtCostCoin,"需要消耗:        "+nextSc.coin);
			SetText(txtCostCrystal, m_pd.crystal + "/"+nextSc.crystal);
		}
		else
		{
			//满级
			//隐藏“强化后”加成内容
			SetActive(costInfoTrans,false);
			SetActive(propArr1,false);
			SetActive(propArr2,false);
			SetActive(propArr3,false);
			SetActive(propHp2,false);
			SetActive(propHurt2,false);
			SetActive(propDef2,false);
		}
	}


	//点击强化按钮
	private void OnClickStrongBtn()
	{
		audioSvc.PlayUIAudio(Constants.UIClickBtn);
		//下面是校验
		//判断星级是否已满
		if (m_pd.strongArr[m_curtIndex] < 10)
		{
			//判断强化条件
			if (m_pd.lv < nextSc.minlv)
			{
				GameRoot.AddTips("角色等级不够");
				return;
			}
			
			if (m_pd.coin < nextSc.coin)
			{
				GameRoot.AddTips("金币数量不够");
				return;
			}
			
			if (m_pd.crystal < nextSc.crystal)
			{
				GameRoot.AddTips("水晶数量不够");
				return;
			}
			
			//全部符合条件
			//发送强化协议
			netSvc.SendMsg(new GameMsg()
			{
				cmd = (int)CMD.ReqStrong,
				reqStrong = new ReqStrong()
				{
					pos = m_curtIndex
				}
			});
		}
		else
		{
			//满星
			GameRoot.AddTips("已经是最高星级");
		}
	}
	
	//点击关闭按钮
	private void OnClickCloseBtn()
	{
		audioSvc.PlayUIAudio(Constants.UIClickBtn);
		SetWindowState(false);
	}

	public void UpdateUI()
	{
		ClickPosItem(m_curtIndex);
	}
}