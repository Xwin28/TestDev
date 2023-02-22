using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class S_VFXController : MonoBehaviour
{
    public ParticleSystem m_VFX;
    // __________________________FUNTION______________
    public void Func_ShowVFX(Vector3 _Pos)
    {
        m_VFX.transform.position = _Pos;
        m_VFX.Play();
    }
}//end class

