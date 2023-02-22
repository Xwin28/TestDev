using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class S_StartPoint : MonoBehaviour
{

    public S_GameManager m_GameManager;
    // __________________________FUNTION______________
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            m_GameManager.Func_HitStartPoint();
        }
    }

}//end class

