using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class S_AudioManager : MonoBehaviour
{
    public SoundStruct[] m_Sound;
    public AudioSource[] m_AudioSource;

    public AudioSource m_EngineAudio;
    public AudioSource m_Music;
    // __________________________FUNTION______________
    public void Func_PlaySound(E_Sound _Sound)
    {
        AudioClip _clip = null;
        for (int i = 0; i < m_Sound.Length; i++)
        {
            if (m_Sound[i].m_SoundType == _Sound)
            {
                _clip = m_Sound[i].m_AudioClip;
                break;
            }
        }
        if(_Sound == E_Sound._5_HitMoney)
        {
            m_AudioSource[m_AudioSource.Length - 1].clip = _clip;
            m_AudioSource[m_AudioSource.Length - 1].pitch = Random.Range(.95f, 1.05f);
            m_AudioSource[m_AudioSource.Length - 1].Play();
            return;
        }
        bool _hasAudioSource = false ;
        for(int i =0;i< m_AudioSource.Length-1;i++)
        {
            if(!m_AudioSource[i].isPlaying)
            {
                m_AudioSource[i].clip = _clip;
                m_AudioSource[i].pitch = Random.Range(.95f, 1.05f);
                m_AudioSource[i].Play();
                _hasAudioSource = true;
                break;
            }
        }
        if(!_hasAudioSource)
        {
            m_AudioSource[m_AudioSource.Length - 1].clip = _clip;
            m_AudioSource[m_AudioSource.Length - 1].pitch = Random.Range(.95f, 1.05f);
            m_AudioSource[m_AudioSource.Length - 1].Play();
        }
    }
    #region Engine Sound
    public void Func_PlayEngineSound()
    {
        m_EngineAudio.Play();
    }
    public void Func_StopEngineSound()
    {
        m_EngineAudio.Stop();
    }
    #endregion
    

}//end class

[System.Serializable]
public struct SoundStruct
{
    public E_Sound m_SoundType;
    public AudioClip m_AudioClip;
}

public enum E_Sound
{
    _0_Victory, 
    _1_Defeat,
    _2_SpeedUp,
    _3_SlowDown,
    _4_PoliceAlert,
    _5_HitMoney,
    _6_HitChangeSkin,
    _7_SkinChangeDestroy
}
public enum E_Music
{
    _0_BackgroundMusic,
    _1_Engine
}