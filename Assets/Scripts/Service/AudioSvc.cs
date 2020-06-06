/****************************************************
    文件：AudioSvc.cs
	作者：wangz
    邮箱: wangzeping1998@gmail.com
    日期：2020/6/5 16:3:4
	功能：声音播放服务
*****************************************************/

using UnityEngine;

public class AudioSvc : MonoBehaviour
{
	public static AudioSvc instance;
	public AudioSource bgAudio;
	public AudioSource uiAudio;
	public void InitSvc()
	{
		instance = this;
		Debug.Log("Init AudioSvc...");
	}

	public void PlayBgMusic(string name, bool isLoop = true)
	{
		AudioClip audio = ResSvc.instance.LoadAudio("ResAudio/" + name, true);
		if (bgAudio.clip == null || bgAudio.clip.name != audio.name)
		{
			bgAudio.clip = audio;
			bgAudio.loop = isLoop;
			bgAudio.Play();
		}
	}

	public void PlayUIAudio(string name)
	{
		AudioClip audio = ResSvc.instance.LoadAudio("ResAudio/" + name, true);
		uiAudio.clip = audio;
		uiAudio.Play();
	}
}