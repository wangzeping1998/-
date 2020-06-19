/****************************************************
    文件：BaseData.cs
	作者：wangz
    邮箱: wangzeping1998@gmail.com
    日期：2020/6/19 9:46:19
	功能：Nothing
*****************************************************/

using UnityEngine;

public class BaseData<T>
{
    public int id;
}

public class MapCfg : BaseData<MapCfg>
{
    public string mapName;
    public string sceneName;
    public int power;
    public Vector3 mainCamPos;
    public Vector3 mainCamRote;
    public Vector3 playerBornPos;
    public Vector3 playerBornRote;
}