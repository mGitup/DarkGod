/****************************************************
    文件：AudioSvc.cs
	作者：洛辰
    邮箱: 893271704@qq.com
    日期：2022/6/5 15:48:29
	功能：声音播放服务
*****************************************************/

using UnityEngine;

public class AudioSvc : MonoBehaviour {
    public static AudioSvc Instance = null;
    
    public AudioSource bgAudio;
    public AudioSource uiAudio;
     
    public void InitSvc()
    {
        Instance = this;
        PECommon.Log("Init AudioSvc...");
    }

    public void StopBGMusic()
    {
        if(bgAudio != null)
        {
            bgAudio.Stop();
        }
    }

    //播放音乐
    public void PlayBGMusic(string name, bool isLoop = true)
    {
        AudioClip audio = ResSvc.Instance.LoadAudio("ResAudio/" + name, true);
        //bgAudio 已经指定了，不会为null，但如果当前没有播放音乐则 bgAudio.clip==null
        if(bgAudio.clip == null || audio.name != bgAudio.clip.name)
        {
            bgAudio.clip = audio;
            bgAudio.loop = isLoop;
            bgAudio.Play();
        }
    }

    public void PlayUIAudio(string name)
    {
        AudioClip audio = ResSvc.Instance.LoadAudio("ResAudio/" + name, true);
        uiAudio.clip = audio;
        uiAudio.Play();
    }
    public void PlayCharAudio(string name,AudioSource audioSource)
    {
        AudioClip audio = ResSvc.Instance.LoadAudio("ResAudio/" + name, true);
        audioSource.clip = audio;
        audioSource.Play();
    }

}
