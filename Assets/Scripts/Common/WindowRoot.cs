/****************************************************
    文件：WindowRoot.cs
	作者：wangz
    邮箱: wangzeping1998@gmail.com
    日期：2020/6/5 15:40:38
	功能：Nothing
*****************************************************/

using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class WindowRoot : MonoBehaviour
{
	protected ResSvc resSvc = null;
	protected AudioSvc audioSvc = null;
	protected NetSvc netSvc = null;
	
	public void SetWindowState(bool isActive = true)
	{
		if (gameObject.activeSelf != isActive)
		{
			SetActive(gameObject, isActive);
		}

		if (isActive)
		{
			InitWind();
		}
		else
		{
			ClearWind();
		}
	}

	protected virtual void InitWind()
	{
		resSvc = ResSvc.instance;
		audioSvc = AudioSvc.instance;
		netSvc = NetSvc.instance;
	}

	protected virtual void ClearWind()
	{
		resSvc = null;
		audioSvc = null;
		netSvc = null;
	}

	#region Tool Functions

	protected void SetActive(GameObject go,bool isActive = true)
	{
		go.SetActive(isActive);
	}
	
	protected void SetActive(Transform trans,bool isActive = true)
	{
		trans.gameObject.SetActive(isActive);
	}
	
	protected void SetActive(RectTransform recTrans,bool isActive = true)
	{
		recTrans.gameObject.SetActive(isActive);
	}
	
	protected void SetActive(Image img,bool isActive = true)
	{
		img.gameObject.SetActive(isActive);
	}
	
	protected void SetActive(Text txt,bool isActive = true)
	{
		txt.gameObject.SetActive(isActive);
	}
	
	protected void SetText(Text txt,string context= "")
	{
		txt.text = context;
	}
	
	protected void SetText(Text txt,int num = 0)
	{
		SetText(txt,num.ToString());
	}
	
	protected void SetText(Transform trans,int num = 0)
	{
		SetText(trans.GetComponent<Text>(),num.ToString());
	}
	
	protected void SetText(Transform trans,string context= "")
	{
		SetText(trans.GetComponent<Text>(),context);
	}

	#endregion

	#region Click Evts

	protected T GetOrAddComponent<T>(GameObject go) where T: Component
	{
		T t = go.GetComponent<T>();
		if (t == null)
		{
			t = go.AddComponent<T>();
		}

		return t;
	}
	
	protected void OnClickDown(GameObject go,Action<PointerEventData> cb)
	{
		PEListener listen = GetOrAddComponent<PEListener>(go);
		listen.onClickDown = cb;
	}

	protected void OnClickUp(GameObject go,Action<PointerEventData> cb)
	{
		PEListener listen = GetOrAddComponent<PEListener>(go);
		listen.onClickUp = cb;
	}
	
	protected void OnDrag(GameObject go,Action<PointerEventData> cb)
	{
		PEListener listen = GetOrAddComponent<PEListener>(go);
		listen.onDrag = cb;
	}
	#endregion

	
	
}