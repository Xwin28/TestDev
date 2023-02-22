using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class S_SkinItem : MonoBehaviour
{
    public Rigidbody[] m_Part;
    [SerializeField] private float force = 100f; // the amount of force to apply
    [SerializeField] private ForceMode forceMode = ForceMode.Impulse; // the type of force to apply
    bool m_IsHit;
    // __________________________FUNTION______________

    
    private void OnTriggerEnter(Collider other)
    {
        if (m_IsHit) return;
        if (other.CompareTag("Player"))
        {
            other.GetComponent<S_PlayerController>().m_GameManager.m_AudioManager.Func_PlaySound(E_Sound._7_SkinChangeDestroy);
            m_IsHit = true;
            // Calculate the direction of the collision
            Vector3 direction = transform.forward;// other.transform.position - transform.position;
            direction = direction.normalized;
            foreach (var _temp in m_Part)
            {
                _temp.isKinematic = false;
                _temp.AddForce(direction * force, forceMode);
            }
           
        }
    }
}//end class
