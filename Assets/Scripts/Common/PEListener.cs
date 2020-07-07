/****************************************************
    文件：PEListener.cs
	作者：wangz
    邮箱: wangzeping1998@gmail.com
    日期：2020/6/16 9:35:20
	功能：触摸监听
*****************************************************/

using System;
using UnityEngine;
using UnityEngine.EventSystems;
public class PEListener : MonoBehaviour,IPointerDownHandler,IPointerUpHandler,IDragHandler,IPointerClickHandler
{
	public Action<object> onClick;
	public Action<PointerEventData> onClickDown;
	public Action<PointerEventData> onClickUp;
	public Action<PointerEventData> onDrag;

	public object args=null;
		
	/// <summary>
	/// 点击
	/// </summary>
	public void OnPointerClick(PointerEventData eventData)
	{
		if (onClick != null)
		{
			onClick.Invoke(args);
		}
	}
	
	/// <summary>
	/// 按下
	/// </summary>
	public void OnPointerDown(PointerEventData eventData)
	{
		if (onClickDown != null)
		{
			onClickDown.Invoke(eventData);
		}
	}

	/// <summary>
	/// 抬起
	/// </summary>
	public void OnPointerUp(PointerEventData eventData)
	{
		if (onClickUp != null)
		{
			onClickUp.Invoke(eventData);
		}
	}

	/// <summary>
	/// 拖拽
	/// </summary>
	public void OnDrag(PointerEventData eventData)
	{
		if (onDrag != null)
		{
			onDrag.Invoke(eventData);
		}
	}

}