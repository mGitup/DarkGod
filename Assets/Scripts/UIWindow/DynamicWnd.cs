/****************************************************
    文件：DynamicWnd.cs
	作者：洛辰
    邮箱: 893271704@qq.com
    日期：2022/6/5 17:19:21
	功能：动态UI元素界面
*****************************************************/

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DynamicWnd : WindowRoot {
    public Animation tipsAni;
    public Text txtTips;
    public Transform hpItemRoot;

    public Animation selfDodgeAin;

    private bool isTipsShow = false;
    private Queue<string> tipsQue = new Queue<string>();
    private Dictionary<string, ItemEntityHP> itemDic = new Dictionary<string, ItemEntityHP>();

    protected override void InitWnd()
    {
        base.InitWnd();

        SetActive(txtTips,false);
        selfDodgeAin.Stop();
    }

    #region Tips
    public void AddTips(string tips)
    {
        lock (tipsQue)
        {
            tipsQue.Enqueue(tips);
        }
    }

    private void Update()
    {
        if(tipsQue.Count > 0 && isTipsShow == false)
        {
            lock (tipsQue)
            {
                isTipsShow = true;
                string tips = tipsQue.Dequeue();
                SetTips(tips);
            }
        } 
    }

    private void SetTips(string tips)
    {
        SetActive(txtTips);
        SetText(txtTips, tips);

        AnimationClip clip = tipsAni.GetClip("TipsShowAni");
        tipsAni.Play();

        //延时关闭激活状态
        StartCoroutine(AniPlayDown(clip.length, () =>
        {
            SetActive(txtTips, false);
            isTipsShow=false;
        }));

    }

    private IEnumerator AniPlayDown(float sec,Action cb)
    {
        yield return new WaitForSeconds(sec);
        if(cb != null)
        {
            cb();
        }
    }
    #endregion

    public void AddHpItemInfo(string mName,Transform trans, int hp)
    {
        ItemEntityHP item = null;
        if(itemDic.TryGetValue(mName, out item))
        {
            return;
        }
        else
        {
            GameObject go = resSvc.loadPrefab(PathDefine.HPItemPrefab,true);
            go.transform.SetParent(hpItemRoot);
            go.transform.localPosition = new Vector3(-1000, 0, 0);
            ItemEntityHP ieh = go.GetComponent<ItemEntityHP>();
            ieh.InitItemInfo(trans,hp);
            itemDic.Add(mName, ieh);
        }
    }
    public void RemoveHpItemInfo(string mName)
    {
        ItemEntityHP item = null;
        if (itemDic.TryGetValue(mName, out item))
        {
            Destroy(item.gameObject);
            itemDic.Remove(mName);            
        }        
    }
    public void RemoveAllHpItemInfo()
    {
        foreach(var item in itemDic)
        {
            Destroy(item.Value.gameObject);
        }
        itemDic.Clear();
    }

    public void SetDodge(string key)
    {
        ItemEntityHP item = null;
        if(itemDic.TryGetValue(key, out item))
        {
            item.SetDodge();
        }
    }
    public void SetCritical(string key,int critical)
    {
        ItemEntityHP item = null;
        if (itemDic.TryGetValue(key, out item))
        {
            item.SetCritical(critical);
        }
    }
    public void SetHurt(string key,int hurt)
    {
        ItemEntityHP item = null;
        if (itemDic.TryGetValue(key, out item))
        {
            item.SetHurt(hurt);
        }
    }
    public void SetHPVal(string key, int oldVal,int newVal)
    {
        ItemEntityHP item = null;
        if (itemDic.TryGetValue(key, out item))
        {
            item.SetHPVal(oldVal,newVal);
        }
    }

    public void SetSelfDodge()
    {
        selfDodgeAin.Stop();
        selfDodgeAin.Play();
    }
}
