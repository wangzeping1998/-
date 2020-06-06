/****************************************************
    文件：ResSvc.cs
	作者：wangz
    邮箱: wangzeping1998@gmail.com
    日期：2020/6/5 14:18:17
	功能：资源加载服务
*****************************************************/

using System;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResSvc : MonoBehaviour
{
	public static ResSvc instance = null;

	private Action prgCb = null;
	
	public void InitSvc()
	{
		instance = this;
		InitRDNameCfg();
		Debug.Log("Init ResSvc...");
	}

	public void AsyncLoadScene(string sceneName,Action loaded)
	{
		GameRoot.instance.loadingWind.SetWindowState();

		AsyncOperation sceneAsync = SceneManager.LoadSceneAsync(sceneName);
		prgCb = () =>
		{
			float val = sceneAsync.progress;
			GameRoot.instance.loadingWind.SetProgress(val);
			if (val == 1)
			{
				if (loaded!=null)
				{
					loaded.Invoke();
				}
				prgCb = null;
				sceneAsync = null;
				GameRoot.instance.loadingWind.SetWindowState(false);
				
			}
		};
		
	}

	private void Update()
	{
		if (prgCb != null)
		{
			prgCb.Invoke();
		}
	}

	private Dictionary<string,AudioClip> adDic = new Dictionary<string, AudioClip>();
	
	public AudioClip LoadAudio(string path, bool cache = false)
	{
		AudioClip au = null;
		if (!adDic.TryGetValue(path,out au) )
		{
			au = Resources.Load<AudioClip>(path);
			if (cache)
			{
				adDic.Add(path,au);
			}
		}

		return au;
	}

	#region InitCfgs

	private List<string> surnameLst = new List<string>();
	private List<string> mannLst = new List<string>();
	private List<string> womanLst = new List<string>();
	
	private void InitRDNameCfg()
	{
		TextAsset xml = Resources.Load<TextAsset>(PathDefine.RDNameCfg);
		if (!xml)
		{
			Debug.LogError("xml file " + PathDefine.RDNameCfg + "not exist");
		}
		else
		{
			XmlDocument doc = new XmlDocument();
			doc.LoadXml(xml.text);
			XmlNodeList nodLst = doc.SelectSingleNode("root").ChildNodes;

			for (int i = 0; i < nodLst.Count; i++)
			{
				 XmlElement ele = nodLst[i] as XmlElement;
				 if (ele.GetAttributeNode("ID") == null)
				 {
					 continue;
				 }
				 int ID = Convert.ToInt32(ele.GetAttributeNode("ID").InnerText);

				 foreach (XmlElement e  in nodLst[i].ChildNodes)
				 {
					 switch (e.Name)
					 {
						 case "surname": surnameLst.Add(e.InnerText);
							 break;
						 case "man": mannLst.Add(e.InnerText);
							 break;
						 case "woman": womanLst.Add(e.InnerText);
							 break;
					 }
				 }
			}
		}
	}

	public string GetRDNameData(bool man = true)
	{
		System.Random rd = new System.Random();
		string rdName = surnameLst[PETools.RDInit(0, surnameLst.Count-1)];
		if (man)
		{
			rdName += mannLst[PETools.RDInit(0, mannLst.Count-1)];
		}
		else
		{
			rdName += womanLst[PETools.RDInit(0, womanLst.Count-1)];
		}
		return rdName;
	}

	#endregion
}