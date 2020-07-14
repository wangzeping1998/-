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
    public Animator anim;
    public CharacterController ctrl;

    private bool isMove = false;
    private Vector2 _dir = Vector2.zero;

    public Vector2 Dir
    {
        get { return _dir; }
        set
        {
            if (value == Vector2.zero)
            {
                isMove = false;
            }
            else
            {
                isMove = true;
            }

            _dir = value;
        }
    }

    private float m_targetBlend;
    private float m_currentBlend;

    public void Init()
    {
        m_camTrans = Camera.main.transform;
        m_camOffset = transform.position - m_camTrans.position;
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


        if (isMove)
        {
            //设置方向
            SetDir();
            //产生移动
            SetMove();
            //相机跟随
            SetCam();
        }
    }

    private void SetDir()
    {
        float angle = Vector2.SignedAngle(Dir, new Vector2(0, 1)) + m_camTrans.eulerAngles.y;
        Vector3 eulerAngles = new Vector3(0, angle, 0);
        transform.localEulerAngles = eulerAngles;
    }

    private void SetMove()
    {
        ctrl.Move(transform.forward * Time.deltaTime * Constants.PlayerMoveSpeed);
    }

    public void SetCam()
    {
        if (m_camTrans != null)
        {
            m_camTrans.position = transform.position - m_camOffset;
        }
    }

    public void SetBlend(float blend)
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
}