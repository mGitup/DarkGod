/****************************************************
    文件：EntityBase.cs
	作者：洛辰
    邮箱: 893271704@qq.com
    日期：2022/6/9 19:31:9
	功能：表现实体控制器抽象基类
*****************************************************/

using System.Collections.Generic;
using UnityEngine;

public abstract class Controller :MonoBehaviour
{
    public Animator ani;

    public CharacterController ctrl;
    public Transform hpRoot;

    

    protected Transform camTrans;

    protected bool isMove = false;
    protected bool skillMove = false;
    protected float skillMoveSpeed = 0f;

    protected TimerSvc timerSvc;
    protected Dictionary<string,GameObject>fxDic=new Dictionary<string, GameObject> ();

    private Vector2 dir = Vector2.zero;

    public virtual void Init()
    {
        
        timerSvc = TimerSvc.Instance;
        camTrans = Camera.main.transform;
    }
    public Vector2 Dir
    {
        get
        {
            return dir;
        }

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
            dir = value;
        }
    }

    public virtual void SetBlend(float blend)
    {
        ani.SetFloat("Blend", blend);
    }
    public virtual void SetAction(int act)
    {
        ani.SetInteger("Action", act);
    }
    public virtual void SetFX(string name, float destroy)
    {

    }

    //设置移动数据
    public void SetSkillMoveState(bool move, float skillSpeed = 0f)
    {
        skillMove = move;
        skillMoveSpeed = skillSpeed;
    }

    public virtual void SetAtkRotationCam(Vector2 camDir)
    {
        float angle = Vector2.SignedAngle(camDir, new Vector2(0, 1)) + camTrans.eulerAngles.y;
        Vector3 eulerAngles = new Vector3(0, angle, 0);
        transform.localEulerAngles = eulerAngles;
    }

    public virtual void SetAtkRotationLocal(Vector2 atkDir)
    {
        float angle = Vector2.SignedAngle(atkDir, new Vector2(0, 1));
        Vector3 eulerAngles = new Vector3(0, angle, 0);
        transform.localEulerAngles = eulerAngles;
    }
}

