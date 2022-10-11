/****************************************************
    文件：MonsterController.cs
	作者：洛辰
    邮箱: 893271704@qq.com
    日期：2022/7/5 11:2:38
	功能：怪物表现实体角色控制器
*****************************************************/


using UnityEngine;

public class MonsterController : Controller
{
    private void Update()
    {
        //AI 逻辑表现
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

    private void SetMove()
    {
        ctrl.Move(transform.forward * Time.deltaTime * Constants.MonsterMoveSpeed);
        //给一个向下的速度，便于在没有apply root 时怪物可以落地。 Fix Res Error
        ctrl.Move(Vector3.down * Time.deltaTime * Constants.MonsterMoveSpeed);
    }
}

