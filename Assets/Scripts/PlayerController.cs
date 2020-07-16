/****************************************************
    文件：PlayerController.cs
	作者：wangz
    邮箱: wangzeping1998@gmail.com
    日期：2020/6/17 11:20:0
	功能：表现实体角色控制器
*****************************************************/

using System;
using UnityEngine;

public class PlayerController : Controller
{
    private Transform m_camTrans;
    private Vector3 m_camOffset;

    public CharacterController ctrl;
    
    private float m_targetBlend;
    private float m_currentBlend;	
    
    public GameObject daggeratk1fx;

    public void Init()
    {
        base.Init();
        m_camTrans = Camera.main.transform;
        m_camOffset = transform.position - m_camTrans.position;

        if (daggeratk1fx!= null)
        {
            base.fxDic.Add(daggeratk1fx.name,daggeratk1fx);
        }
    }

    private void Update()
    {
        #region Input

//        float h = Input.GetAxis("Horizontal");
//        float v = Input.GetAxis("Vertical");
//        Vector2 _dir = new Vector2(h, v).normalized;
//
//        if (_dir != Vector2.zero)
//        {
//            this.Dir = _dir;
//            SetBlend(Constants.BlendWalk);
//        }
//        else
//        {
//            this.Dir = Vector2.zero;
//            SetBlend(Constants.BlendIdle);
//        }

        #endregion

        if ( m_currentBlend!= m_targetBlend )
        {
            UpdateMixBlend();
        }
        
        //普通移动
        if (isMove && !isSkillMove)
        {
            //设置方向
            SetDir();
            //产生移动
            SetMove();
            //相机跟随
            SetCam();
        }
        
        //技能位移
        if (isSkillMove)
        {
            //技能移动
            SkillMove();
            //相机跟随
            SetCam();
        }



    }

    //设置移动人物朝向
    private void SetDir()
    {
        float angle = Vector2.SignedAngle(Dir, new Vector2(0, 1)) + m_camTrans.eulerAngles.y;
        Vector3 eulerAngles = new Vector3(0, angle, 0);
        transform.localEulerAngles = eulerAngles;
    }

    //移动
    private void SetMove()
    {
        ctrl.Move(transform.forward * Time.deltaTime * Constants.PlayerMoveSpeed);
    }

    //技能位移
    private void SkillMove()
    {
        ctrl.Move(transform.forward * Time.deltaTime * skillMoveSpeed);
    }
    
    //设置相机跟随
    public void SetCam()
    {
        if (m_camTrans != null)
        {
            m_camTrans.position = transform.position - m_camOffset;
        }
    }
    
    //设置动画混合参数
    public override void SetBlend(float blend)
    {
        m_targetBlend = blend;
    }

    private void UpdateMixBlend()
    {
        if (Mathf.Abs(m_currentBlend - m_targetBlend) < Constants.AccelerSpeed * Time.deltaTime)
        {
            m_currentBlend = m_targetBlend;
        }
        else if (m_currentBlend > m_targetBlend)
        {
            m_currentBlend -= Constants.AccelerSpeed * Time.deltaTime;
        }
        else
        {
            m_currentBlend += Constants.AccelerSpeed * Time.deltaTime;
        }

        anim.SetFloat("Blend", m_currentBlend);
    }

    //显示技能特效
    public override void SetFx(string name, float destroy)
    {
        GameObject go = null;
        if (fxDic.TryGetValue(name, out go))
        {
            go.SetActive(true);
            timerSvc.AddTimeTask((tackId) => { go.SetActive(false); }, destroy);

        }
    }
    
}