/****************************************************
    文件：Constants.cs
	作者：洛辰
    邮箱: 893271704@qq.com
    日期：2022/6/4 20:45:59
	功能：常量配置
*****************************************************/

using UnityEngine;
public enum TxtColor
{
    Red,
    Green,
    Blue,
    Yellow
}

public enum DamageType
{
    None,
    AD=1,
    AP=2,
}

public enum EntityType
{
    None,
    Player,
    Monster
}

public enum EntityState
{
    None,
    BatiState,//霸体状态: 不受控制，可受伤害
}

public enum MonsterType{
    None,
    Normal = 1,
    Boss = 2,
}

public class Constants {
    private const string ColorRed = "<color=#FF0000FF>";
    private const string ColorGreen = "<color=#00FF00FF>";
    private const string ColorBlue = "<color=#00B4FFFF>";
    private const string ColorYellow = "<color=#FFFF00FF>";
    private const string ColorEnd = "</color>";

    public static string Color(string str,TxtColor c)
    {
        string result = "";
        switch (c)
        {
            case TxtColor.Red:
                result = ColorRed + str + ColorEnd;
                break;
            case TxtColor.Green:
                result = ColorGreen + str + ColorEnd;
                break;
            case TxtColor.Blue:
                result = ColorBlue + str + ColorEnd;
                break;
            case TxtColor.Yellow:
                result = ColorYellow + str + ColorEnd;
                break;
        }
        return result;
    }

    //AutoGuideNPC
    public const int NPCWiseMan = 0;
    public const int NPCGeneral = 1;
    public const int NPCArtisan = 2;
    public const int NPCTrader = 3;

    //场景名称/ID
    public const string SceneLogin = "SceneLogin";
    public const int MainCityMapID = 10000;

    //public const string SceneMainCity = "SceneMainCity";

    //音效名称
    public const string BGLogin = "bgLogin";
    public const string BGMainCity = "bgMainCity";
    public const string BGHuangYe = "bgHuangYe";
    public const string AssassinHit = "assassin_Hit";//受击音效

    //登录按钮音效
    public const string UILoginBtn = "uiLoginBtn";

    //常规 UI 点击音效
    public const string UIClickBtn = "uiClickBtn";
    public const string UIExtenBtn = "uiExtenBtn";
    public const string UIOpenPage = "uiOpenPage";
    public const string FBItem = "fbitem";

    public const string FBLose = "fblose";
    public const string FBLogoEnter = "fbwin";

    //屏幕标准宽高
    public const int ScreenStandardWidth = 1334;
    public const int ScreenStandardHeight = 750;

    //遥感点标准距离
    public const int ScreenOPDis = 90;

    //混合参数
    public const int BlendIdle = 0;
    public const int BlendMove = 1;

    //角色移到速度
    public const int PlayMoveSpeed = 8;
    public const int MonsterMoveSpeed = 3;

    //运动平滑加速度
    public const float AccelerSpeed = 5;
    //虚拟血条变化加速度
    public const float AccelerHPSpeed = 0.3f;

    //Action 触发参数
    public const int ActionDefault = -1;
    public const int ActionBorn = 0;
    public const int ActionHit = 101;
    public const int ActionDie = 100;

    public const int DieAniLength = 4000;

    //普攻连招有效间隔
    public static int ComboSpace = 500;
}
