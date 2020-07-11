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
			if (i<(fbId % 10000))
			{
				SetActive(btnFubens[i].transform);
				if (i == fbId % 10000 - 1)
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

	protected override void ClearWind()
	{
		base.ClearWind();
		btnClose.onClick.RemoveListener(OnClickCloseBtn);
	}
	
	
	//点击关闭按钮
	private void OnClickCloseBtn()
	{
		audioSvc.PlayUIAudio(Constants.UIClickBtn);
		SetWindowState(false);
	}
}