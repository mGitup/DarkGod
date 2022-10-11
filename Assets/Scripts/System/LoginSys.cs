/****************************************************
    文件：LoginSys.cs
	作者：洛辰
    邮箱: 893271704@qq.com
    日期：2022/6/4 17:17:6
	功能：登录注册业务系统
*****************************************************/

using PEProtocol;
using UnityEngine;

public class LoginSys : SystemRoot
{
    public static LoginSys Instance = null;

    public LoginWnd loginWnd;
    public CreateWnd createWnd;

    public override void InitSys()
    {
        Instance = this;
        base.InitSys();
        PECommon.Log("Init LoginSys...");
    }

    //进入登录场景
    public void EnterLogin()
    {
        
        //异步地加载登录场景
        //并显示加载进度
        resSvc.AsyncLoadScene(Constants.SceneLogin, () =>
        {
            //加载完成后打开登录注册界面
            loginWnd.SetWndState();
            audioSvc.PlayBGMusic(Constants.BGLogin);
        });

        

    }
    
    public void RspLogin(GameMsg msg)
    {
        GameRoot.AddTips("登陆成功");
        GameRoot.Instance.SetPlayerData(msg.rspLogin);

        if (msg.rspLogin.playerData.name == "")
        {
            createWnd.SetWndState();
        }
        else
        {
            //进入主城 
            MainCitySys.Instance.EnterMainCity();
        }

        //打开角色创建面板
        //createWnd.SetWndState();

        //关闭登录界面
        loginWnd.SetWndState(false);
    }

    public void RspRename(GameMsg msg)
    {
        GameRoot.Instance.SetPlayerName(msg.rspRename.name);
                
        //跳转场景，进入主城        
        MainCitySys.Instance.EnterMainCity();


        //关闭创建界面
        createWnd.SetWndState(false);
    }
}
