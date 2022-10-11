/****************************************************
    文件：GuideWnd.cs
	作者：洛辰
    邮箱: 893271704@qq.com
    日期：2022/7/10 14:19:10
	功能：引导对话界面
*****************************************************/

using PEProtocol;
using UnityEngine;
using UnityEngine.UI;

public class GuideWnd : WindowRoot {
    public Text txtName;
    public Text txtTalk;
    public Image imgIcon;

    private PlayerData pd;
    private AutoGuideCfg curTaskData;
    private string[] dialogArr;
    private int index;
    protected override void InitWnd()
    {
        base.InitWnd();

        pd = GameRoot.Instance.PlayerData;
        curTaskData=MainCitySys.Instance.GetCurtTaskData();
        dialogArr = curTaskData.dilogArr.Split('#');
        index = 1;
        SetTalk();
    }

    private void SetTalk()
    {
        string[] talkArr = dialogArr[index].Split('|');
        if (talkArr[0] == "0")
        {//自己
            SetSprite(imgIcon,PathDefine.SelfIcon);
            SetText(txtName,pd.name);
        }
        else
        {
            //NPC
            switch (curTaskData.npcID)
            {
                case 0:
                    SetSprite(imgIcon, PathDefine.WiseManIcon);
                    SetText(txtName, "智者");
                    break;
                case 1:
                    SetSprite(imgIcon, PathDefine.GeneralIcon);
                    SetText(txtName, "将军");
                    break;
                case 2:
                    SetSprite(imgIcon, PathDefine.ArtisanIcon);
                    SetText(txtName, "工匠");
                    break;
                case 3:
                    SetSprite(imgIcon, PathDefine.TraderIcon);
                    SetText(txtName, "商人");
                    break;
                default:
                    SetSprite(imgIcon, PathDefine.GuideIcon);
                    SetText(txtName, "小芸");
                    break;
            }
        }

        imgIcon.SetNativeSize();
        SetText(txtTalk, talkArr[1].Replace("$name", pd.name));
    }

    public void ClickNextBtn()
    {
        audioSvc.PlayUIAudio(Constants.UIClickBtn);
        index += 1;
        if (index == dialogArr.Length)
        {
            GameMsg msg = new GameMsg
            {
                cmd = (int)CMD.ReqGuide,
                reqGuide = new ReqGuide
                {
                    guideid = curTaskData.ID
                }
            };
            netSvc.SendMsg(msg);
            SetWndState(false);
        }
        else
        {
            SetTalk();
        }
    }
}
