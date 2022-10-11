/****************************************************
    文件：CreateWnd.cs
	作者：洛辰
    邮箱: 893271704@qq.com
    日期：2022/6/5 20:10:7
	功能：角色创建界面
*****************************************************/

using PEProtocol;
using UnityEngine;
using UnityEngine.UI;

public class CreateWnd : WindowRoot {

    public InputField iptName;
    protected override void InitWnd()
    {
        base.InitWnd();
                
        //显示一个随机名字
        iptName.text = resSvc.GetRDNameCfg(false);
    }

    public void ClickRandBtn()
    {
        audioSvc.PlayUIAudio(Constants.UIClickBtn);
        string rdName = resSvc.GetRDNameCfg(false);
        iptName.text = rdName;
    }

    public void ClickEnterBtn()
    {
        audioSvc.PlayUIAudio(Constants.UIClickBtn);

        if (iptName.text != "")
        {
            //发送名字数据到服务器，登录主城
            GameMsg msg = new GameMsg
            {
                cmd = (int)CMD.ReqRename,
                reqRename = new ReqRename
                {
                    name = iptName.text
                }
            };
            netSvc.SendMsg(msg);
        }
        else
        {
            GameRoot.AddTips("当前名字不符合规范");
        }
    }
}
