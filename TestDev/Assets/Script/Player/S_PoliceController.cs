using DG.Tweening;
using PathCreation.Examples;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class S_PoliceController : MonoBehaviour
{

    public Transform m_pointA; // The first point to move to
    public Transform m_pointB; // The second point to move to

    public float m_movementSpeed = 5f; // The speed at which the object should move
    public bool m_CanControl;
    private bool m_moving = false; // Whether the object is currently moving
    private bool m_movingToPointA = false; // Whether the object is currently moving towards point A
    private bool m_movingToPointB = false; // Whether the object is currently moving towards point B
    [Header("Animator")]
    public PathFollower m_PathFollower;

    public S_GameManager m_GameManager;



    void Update()
    {
        if (!m_CanControl) return;
        // Move towards point A if the A button is pressed down
        if (Input.GetKeyDown(KeyCode.D))
        {
            m_moving = true;
            m_movingToPointA = true;
            m_movingToPointB = false;
        }

        // Move towards point B if the D button is pressed down
        if (Input.GetKeyDown(KeyCode.A))
        {

            m_moving = true;
            m_movingToPointA = false;
            m_movingToPointB = true;

        }

        // Stop moving if the A or D button is released
        if (Input.GetKeyUp(KeyCode.A) && m_movingToPointA == false)
        {
            m_moving = false;
        }
        if (Input.GetKeyUp(KeyCode.D) && m_movingToPointB == false)
        {
            m_moving = false;
        }

        // Move towards the appropriate point if we're currently moving
        if (m_moving)
        {
            if (m_movingToPointA)
            {
                if (transform.position != m_pointA.position)
                {
                    transform.position = Vector3.MoveTowards(transform.position, m_pointA.position, m_movementSpeed * Time.deltaTime);

                }
            }
            else if (m_movingToPointB)
            {
                if (transform.position != m_pointB.position)
                {
                    transform.position = Vector3.MoveTowards(transform.position, m_pointB.position, m_movementSpeed * Time.deltaTime);
                }



            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            //Hit Player
            m_GameManager.Func_Defeat();
        }
    }

}//end class

