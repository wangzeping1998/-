/****************************************************
    文件：FubenWind.cs
	作者：wangz
    邮箱: wangzeping1998@gmail.com
    日期：2020/7/10 19:24:45
	功能：副本窗口
*****************************************************/

using PEProtocol;
using UnityEngine;
using UnityEngine.UI;

public class FubenWind : WindowRoot
{
	public Transform pointerTrans;
	public Button[] btnFubens;
	public Button btnClose;
	
	private PlayerData m_pd;
	protected override void InitWind()
	{
		base.InitWind();
		m_pd = GameRoot.instance.playerData;
		btnClose.onClick.AddListener(OnClickCloseBtn);
		RefreshUI();
	}

	public void RefreshUI()
	{
		int fbId = m_pd.fuben;
		for (int i = 0; i < btnFubens.Length; i++)
		{
			if (i < (fbId % 10000))
			{
				SetActive(btnFubens[i].transform);
				int fubenId = i + 10001;
				btnFubens[i].onClick.AddListener(()=>OnClickFubenBtn(fubenId));
				//设置提示位置
				if (i == (fbId % 10000 - 1))
				{
					pointerTrans.SetParent(btnFubens[i].transform);
					pointerTrans.localPosition = new Vector2(30,120);
					
				}

			}
			else
			{
				SetActive(btnFubens[i].transform,false);
			}
		}
	}

	private void OnClickFubenBtn(int id)
	{
		//id 10001
		//验证
		audioSvc.PlayUIAudio(Constants.UIClickBtn);
		MapCfg mc = resSvc.GetMapCfgData(id);
		if (m_pd.power < mc.power)
		{
			GameRoot.AddTips("体力值不足!");
			return;
		}

		GameMsg msg = new GameMsg()
		{
			cmd = (int) CMD.ReqFBFight,
			reqFBFight = new ReqFBFight()
			{
				id = id
			}
		};
		netSvc.SendMsg(msg);
	}

	protected override void ClearWind()
	{
		base.ClearWind();
		btnClose.onClick.RemoveListener(OnClickCloseBtn);
		for (int i = 0; i < btnFubens.Length; i++)
		{
			btnFubens[i].onClick.RemoveAllListeners();
		}
	}
	
	
	//点击关闭按钮
	private void OnClickCloseBtn()
	{
		audioSvc.PlayUIAudio(Constants.UIClickBtn);
		SetWindowState(false);
	}
}