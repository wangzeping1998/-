/****************************************************
    文件：LoadingWind.cs
	作者：wangz
    邮箱: wangzeping1998@gmail.com
    日期：2020/6/5 14:42:43
	功能：Nothing
*****************************************************/

using UnityEngine;
using UnityEngine.UI;

public class LoadingWind : WindowRoot
{
	public Text txtTips;
	public Image imgFG;
	public Image imgPoint;
	public Text txtPrg;

	private float fgWidth;
	
	protected override void InitWind()
	{
		base.InitWind();
		SetText(txtTips, "这是一条游戏Tips");
		SetText(txtPrg, "0%");
		imgFG.fillAmount = 0;
		imgPoint.transform.localPosition = new Vector3(-545f,0,0);
		fgWidth = imgFG.GetComponent<RectTransform>().sizeDelta.x;
	}

	public void SetProgress(float prg)
	{
		SetText(txtPrg, (int) (prg * 100) + "%");
		imgFG.fillAmount = prg;
		float posX = imgFG.fillAmount * fgWidth - 545;
		imgPoint.GetComponent<RectTransform>().anchoredPosition = new Vector2(posX,0);
	}
}