/****************************************************
    文件：WindowRoot.cs
	作者：洛辰
    邮箱: 893271704@qq.com
    日期：2022/6/5 11:21:39
	功能：UI界面基类
*****************************************************/

using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class WindowRoot : MonoBehaviour {
    protected ResSvc resSvc = null;
    protected AudioSvc audioSvc = null;
    protected NetSvc netSvc = null;
    protected TimerSvc timerSvc = null;

    public void SetWndState(bool isActive = true)
    {
        if (gameObject.activeSelf != isActive)
        {
            SetActive(gameObject, isActive);
        }
        if (isActive)
        {
            InitWnd();
        }
        else
        {
            ClearWnd();
        }
    }

    public bool GetWndState()
    {
        return gameObject.activeSelf;
    }

    protected virtual void InitWnd()
    {
        resSvc = ResSvc.Instance;
        audioSvc = AudioSvc.Instance;
        netSvc = NetSvc.Instance;
        timerSvc = TimerSvc.Instance;
    }

    protected virtual void ClearWnd()
    {
        resSvc = null;
        audioSvc = null;
        netSvc = null;
        timerSvc = null;
    }


    #region Tool Functions
    protected void SetText(Text txt, string content = "")
    {
        txt.text = content;
    }
    protected void SetText(Text txt, int content = 0)
    {
        txt.text = content.ToString();
    }
    protected void SetText(Transform trans, string content = "")
    {
        trans.GetComponent<Text>().text = content;
    }
    protected void SetText(Transform trans, int content = 0)
    {
        trans.GetComponent<Text>().text = content.ToString();
    }

    protected void SetActive(GameObject go, bool isActive = true)
    {
        go.SetActive(isActive);
    }
    protected void SetActive(Transform trans, bool isActive = true)
    {
        trans.gameObject.SetActive(isActive);
    }
    protected void SetActive(RectTransform rectTrans, bool isActive = true)
    {
        rectTrans.gameObject.SetActive(isActive);
    }
    protected void SetActive(Image img, bool isActive = true)
    {
        img.transform.gameObject.SetActive(isActive);
    }
    protected void SetActive(Text txt, bool isActive = true)
    {
        txt.transform.gameObject.SetActive(isActive);
    }

    protected void SetSprite(Image img, string path)
    {
        Sprite sp = resSvc.LoadSprite(path, true);
        img.sprite = sp;
    }


    protected T GetOrAddComponent<T>(GameObject go) where T : Component
    {
        T t = go.GetComponent<T>();
        if (t == null)
        {
            t = go.AddComponent<T>();
        }
        return t;
    }

    protected Transform GetTrans(Transform trans,string name)
    {
        if (trans != null)
        {
            return trans.Find(name);
        }
        else
        {
            return transform.Find(name);
        }
    }
    #endregion

    #region Click Evts
    protected void OnClickDown(GameObject go, Action<PointerEventData> cb)
    {
        PEListener listener = GetOrAddComponent<PEListener>(go);
        listener.onClickDown = cb;
    }
    protected void OnClickUp(GameObject go, Action<PointerEventData> cb)
    {
        PEListener listener = GetOrAddComponent<PEListener>(go);
        listener.onClickUp = cb;
    }
    protected void OnDrag(GameObject go, Action<PointerEventData> cb)
    {
        PEListener listener = GetOrAddComponent<PEListener>(go);
        listener.onDrag = cb;
    }

    protected void OnClick(GameObject go, Action<object> cb,object args)
    {
        PEListener listener = GetOrAddComponent<PEListener>(go);
        listener.onClick = cb;
        listener.args = args;
    }
    #endregion
}
