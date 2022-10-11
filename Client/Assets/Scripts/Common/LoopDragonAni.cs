/****************************************************
    文件：LoopDragonAni.cs
	作者：洛辰
    邮箱: 893271704@qq.com
    日期：2022/6/5 17:9:0
	功能：飞龙动画循环
*****************************************************/

using UnityEngine;

public class LoopDragonAni : MonoBehaviour {
    private Animation ani;

    private void Awake()
    {
        ani = transform.GetComponent<Animation>();
    }

    private void Start()
    {
        if(ani != null)
        {
            InvokeRepeating("PlayDragonAni", 0, 18);
        }
    }

    private void PlayDragonAni()
    {
        if(ani != null)
        {
            ani.Play();

        }
    }
}
