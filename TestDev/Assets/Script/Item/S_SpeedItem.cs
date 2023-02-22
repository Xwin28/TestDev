using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XwinStudio
{
public class S_SpeedItem : MonoBehaviour
{

        // __________________________FUNTION______________
        private void OnTriggerEnter(Collider other)
        {
            if(other.CompareTag("BlockItem"))
            {
                Destroy(other.gameObject);
            }
        }
    }//end class
}//end namespace
