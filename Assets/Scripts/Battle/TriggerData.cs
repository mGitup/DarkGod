/****************************************************
    文件：TriggerData.cs
	作者：洛辰
    邮箱: 893271704@qq.com
    日期：2022/8/4 17:12:17
	功能：地图触发数据类
*****************************************************/

using UnityEngine;

public class TriggerData : MonoBehaviour {
    public int triggerWave;
    public MapMgr mapMgr;

    //选用Exit是为了不让玩家回到之前的小关卡
    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if(mapMgr != null)
            {
                mapMgr.TriggerMonsterBorn(this, triggerWave);
            }
        }
    }
}
