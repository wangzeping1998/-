/****************************************************
    文件：TaskWind.cs
	作者：wangz
    邮箱: wangzeping1998@gmail.com
    日期：2020/7/8 13:16:12
	功能：任务奖励窗口
*****************************************************/

using System.Collections.Generic;
using PEProtocol;
using UnityEngine;
using UnityEngine.UI;

public class TaskWind : WindowRoot
{
	public Button btnClose;
	private PlayerData m_pd;
	private List<TaskRewardData> m_trdLst = new List<TaskRewardData>();	//任务奖励列表
	public Transform scrollTrans;	//TaskItemPrefab的父物体
	protected override void InitWind()
	{
		base.InitWind();
		btnClose.onClick.AddListener(OnClickCloseBtn);
		m_pd = GameRoot.instance.playerData;
		Refresh();
	}

	//刷新UI
	public void Refresh()
	{
		if (!GetWindowState())
		{
			return;
		}
		m_trdLst.Clear();
		for (int i = 0; i < scrollTrans.childCount; i++)
		{
			Destroy(scrollTrans.GetChild(i).gameObject);
		}
		
		List<TaskRewardData> todoLst = new List<TaskRewardData>();	//未完成任务
		List<TaskRewardData> doneLst = new List<TaskRewardData>();	//完成任务

		string[] taskArr = m_pd.taskArr;
		for (int i = 0; i < taskArr.Length; i++)
		{
			string[] taskInfo = m_pd.taskArr[i].Split('|');
			TaskRewardData trd = new TaskRewardData()
			{
				id = int.Parse(taskInfo[0]),
				prgs = int.Parse(taskInfo[1]),
				taked = taskInfo[2].Equals("1") //等于1就是完成的任务
			};
			//是否完成
			if (trd.taked)
			{
				doneLst.Add(trd);
			}
			else
			{
				todoLst.Add(trd);
			}
		}
		
		m_trdLst.AddRange(todoLst);
		m_trdLst.AddRange(doneLst);

		//实例化Prefab并更新它的UI
		for (int i = 0; i < m_trdLst.Count; i++)
		{
			GameObject go = resSvc.LoadPrefab(PathDefine.TaskItemPrefab);
			go.transform.SetParent(scrollTrans);
			go.transform.localPosition = Vector3.zero;
			go.transform.localScale = Vector3.one;
			go.name = "taskItem_" + m_trdLst[i].id;

			TaskRewardData trd = m_trdLst[i];
			TaskRewardCfg trc = resSvc.GetTaskrewardCfg(trd.id);
			
			SetText(GetTrans(go.transform,"txtName"),trc.taskName);
			SetText(GetTrans(go.transform,"txtPro"),trd.prgs + "/" +trc.count);
			SetText(GetTrans(go.transform,"txtExp"),"奖励:      经验"+trc.exp);
			SetText(GetTrans(go.transform,"txtCoin"),"金币"+trc.coin);

			Image imgPrg = GetTrans(go.transform, "prgBarBg/prgBar").GetComponent<Image>();
			float proVal = trd.prgs * 1.0f / trc.count;
			imgPrg.fillAmount = proVal;

			Button btnTake = GetTrans(go.transform, "btnTake").GetComponent<Button>();
			btnTake.onClick.AddListener(()=>OnClickTakeBtn(go.name));

			Transform transComp = GetTrans(go.transform, "imgComp");
			if (trd.taked)
			{
				btnTake.interactable = false;
				SetActive(transComp);
			}
			else
			{
				SetActive(transComp,false);
				if (trd.prgs == trc.count)
				{
					btnTake.interactable = true;
				}
				else
				{
					btnTake.interactable = false;
				}

			}
		}
	}

	private void OnClickTakeBtn(string name)
	{
		string[] strArr = name.Split('_');
		int btnId =  int.Parse(strArr[1]);

		GameMsg msg = new GameMsg
		{
			cmd = (int) CMD.ReqTaskReward,
			reqTaskReward = new ReqTaskReward()
			{
				taskId = btnId,
			}
		};
		netSvc.SendMsg(msg);
		
		TaskRewardCfg trc = resSvc.GetTaskrewardCfg(btnId);
		GameRoot.AddTips(Constants.Color(string.Format("完成金币+{0} 经验+{1}",trc.coin,trc.exp),TxtColor.Blue));
		

	}

	protected override void ClearWind()
	{
		base.ClearWind();
		btnClose.onClick.RemoveListener(OnClickCloseBtn);
		m_trdLst.Clear();
		for (int i = 0; i < scrollTrans.childCount; i++)
		{
			 Destroy(scrollTrans.GetChild(i).gameObject);
		}
	}
	
	//点击关闭按钮
	private void OnClickCloseBtn()
	{
		audioSvc.PlayUIAudio(Constants.UIClickBtn);
		SetWindowState(false);
	}
}