/****************************************************
    文件：ChatWnd.cs
	作者：洛辰
    邮箱: 893271704@qq.com
    日期：2022/7/17 17:11:24
	功能：
*****************************************************/

using PEProtocol;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChatWnd : WindowRoot {
    public Image imgWorld;
    public Image imgGuild;
    public Image imgFriend;
    public Text txtChat;
    public InputField iptChat;

    private int chatType;
    private List<string> chatList = new List<string>();
    private bool canSend = true;

    protected override void InitWnd()
    {
        base.InitWnd();

        chatType = 0;
        RefreshUI();
    }

    public void AddChatMsg(string name,string chat)
    {
        chatList.Add(Constants.Color(name+"：",TxtColor.Blue)+chat);
        if(chatList.Count > 10)
        {
            chatList.RemoveAt(0);
        }

        if (gameObject.activeSelf)
        {
            RefreshUI();
        }
    }

    private void RefreshUI()
    {
        if(chatType == 0)//世界
        {
            string chatMsg = "";
            for(int i = 0; i < chatList.Count; i++)
            {
                chatMsg += chatList[i] + "\n";
            }
            SetText(txtChat, chatMsg);
            SetSprite(imgWorld, PathDefine.BtnSelected);
            SetSprite(imgGuild, PathDefine.BtnUnselected);
            SetSprite(imgFriend, PathDefine.BtnUnselected);
        }
        else if(chatType == 1)//公会
        {
            SetText(txtChat, "尚未加入公会");
            SetSprite(imgWorld, PathDefine.BtnUnselected);
            SetSprite(imgGuild, PathDefine.BtnSelected);
            SetSprite(imgFriend, PathDefine.BtnUnselected);
        }
        else if (chatType == 2)//好友
        {
            SetText(txtChat, "暂无好友信息");
            SetSprite(imgWorld, PathDefine.BtnUnselected);
            SetSprite(imgGuild, PathDefine.BtnUnselected);
            SetSprite(imgFriend, PathDefine.BtnSelected);
        }
    }
    public void ClickWorldBtn()
    {
        audioSvc.PlayUIAudio(Constants.UIClickBtn);
        chatType = 0;
        RefreshUI();
    }
    public void ClickGuildBtn()
    {
        audioSvc.PlayUIAudio(Constants.UIClickBtn);
        chatType = 1;
        RefreshUI();
    }
    public void ClickFriendBtn()
    {
        audioSvc.PlayUIAudio(Constants.UIClickBtn);
        chatType = 2;
        RefreshUI();
    }
    public void ClickSendBtn()
    {
        audioSvc.PlayUIAudio(Constants.UIClickBtn);

        if (!canSend)
        {
            GameRoot.AddTips("聊天消息每 2 秒才能发送一次");
            return;
        }

        if (iptChat.text != null && iptChat.text != "")
        {
            if(iptChat.text.Length > 12)
            {
                GameRoot.AddTips("输入信息不能超过 12 个字");
            }
            else
            {
                //发送网络消息到服务器
                GameMsg msg = new GameMsg
                {
                    cmd = (int)CMD.SndChat,
                    sndChat = new SndChat
                    {
                        chat = iptChat.text
                    }
                };
                iptChat.text = "";
                netSvc.SendMsg(msg);

                canSend = false;
                //协程
                //StartCoroutine(MsgTimer());

                //TimerSvc
                timerSvc.AddTimeTask((int tid) =>
                {
                    canSend = true;
                }, 2, PETimeUnit.Second);
            }
        }
        else
        {
            GameRoot.AddTips("尚未输入聊天信息");
        }
    }

    //IEnumerator MsgTimer()
    //{
    //    yield return new WaitForSeconds(2.0f);
    //    canSend = true;
    //}


    public void ClickCloseBtn()
    {
        audioSvc.PlayUIAudio(Constants.UIClickBtn);
        chatType = 0;
        SetWndState(false);
    }

}
