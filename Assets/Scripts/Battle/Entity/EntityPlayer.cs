/****************************************************
    文件：EntityBase.cs
	作者：洛辰
    邮箱: 893271704@qq.com
    日期：2022/6/9 19:31:9
	功能：玩家逻辑实体
*****************************************************/

using System.Collections.Generic;
using UnityEngine;

public class EntityPlayer: EntityBase
{
    public override Vector2 GetDirInput()
    {
        return battleMgr.GetDirInput();
    }

    public override Vector2 CalctargetDir()
    {
        //PECommon.Log("EntityPlayer CalctargetDir");

        EntityMonster monster = FindClosedTarget();
        if(monster != null)
        {
            Vector3 target = monster.GetPos();
            Vector3 self = GetPos();
            Vector2 dir = new Vector2(target.x - self.x, target.z - self.z);
            return dir.normalized;
        }
        return Vector2.zero;
    }
    private EntityMonster FindClosedTarget()
    {
        List<EntityMonster> list = battleMgr.GetEntityMonsters();
        if(list == null|| list.Count == 0)
        {
            return null;
        }
        Vector3 self = GetPos();
        EntityMonster targetMonster = null;
        float dis = 0;
        for(int i = 0; i < list.Count; i++)
        {
            Vector3 target = list[i].GetPos();
            if (i == 0)
            {
                dis = Vector3.Distance(self, target);
                targetMonster = list[0];
            }
            else
            {
                float calcDis = Vector3.Distance(self,target);
                if (dis > calcDis)
                {
                    dis = calcDis;
                    targetMonster = list[i];
                }
            }
        }
        return targetMonster;
    }

    public override void SetHPVal(int oldVal, int newVal)
    {
        if (controller != null)
            BattleSys.Instance.playerCtrlWnd.SetSelfHPBarVal(newVal);
    }

    public override void SetDodge()
    {
        GameRoot.Instance.dynamicWnd.SetSelfDodge();
    }
}

