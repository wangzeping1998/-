/****************************************************
    文件：MonsterController.cs
	作者：wangz
    邮箱: wangzeping1998@gmail.com
    日期：2020/7/18 0:21:2
	功能：怪物表现实体基类
*****************************************************/

using System;
using UnityEngine;

public class MonsterController : Controller
{
    public Transform itemRoot;
    
    private void Update()
    {
        if (isMove)
        {
            SetDir();
            SetMove();
        }
    }
    private void SetDir()
    {
        float angle = Vector2.SignedAngle(Dir, new Vector2(0, 1));
        Vector3 eulerAngles = new Vector3(0, angle, 0);
        transform.localEulerAngles = eulerAngles;
    }
    //移动
    private void SetMove()
    {
        ctrl.Move(Vector3.down + transform.forward * Time.deltaTime *Constants.MonsterMoveSpeed);
    }
    public override void SetAtkDir(Vector2 dir)
    {
        float angle = Vector2.SignedAngle(dir, new Vector2(0, 1));
        Vector3 eulerAngles = new Vector3(0, angle, 0);
        transform.localEulerAngles =eulerAngles;
    }
    
}