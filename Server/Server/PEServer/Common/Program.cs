/****************************************************
	作者：WangZeping
    邮箱: wangzeping1998@gmail.com
	功能：服务器入口
*****************************************************/
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;

class Program
{
    static void Main(string[] args)
    {
        ServerRoot.Instance.Init();
        while (true) {
            ServerRoot.Instance.Update();
            Thread.Sleep(100);
        }
    }

}