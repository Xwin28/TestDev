using DG.Tweening;
using PathCreation.Examples;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class S_PlayerController : MonoBehaviour
{

    public Transform m_pointA; // The first point to move to
    public Transform m_pointB; // The second point to move to

    public S_GameManager m_GameManager;
    public float m_movementSpeed = 5f; // The speed at which the object should move
    public bool m_CanControl;
    private bool m_moving = false; // Whether the object is currently moving
    private bool m_movingToPointA = false; // Whether the object is currently moving towards point A
    private bool m_movingToPointB = false; // Whether the object is currently moving towards point B
    [Header("UI")]
    public S_MoneyUI m_MoneyUI;
    [Header("Animator")]
    public PathFollower m_PathFollower;
    [SerializeField] bool m_SpeedingUp;
    bool m_SlowingDown;
    public Animator m_Anim;
    public Transform m_WheelFront, m_WheelBack;
    [Header("Skin Color")]
    public Material m_Mat;
    public Material m_DriverMat;
    public Renderer m_MotorSkin;
    public SkinnedMeshRenderer m_DriverSkin;
    [Header("Camera")]
    Sequence m_CameraSequence;
    public Transform m_CameraNearPos;
    public Transform m_CameraFarPos;
    public Transform m_CameraFarAwayPos;
    public Transform m_Camera;
    [Header("Complete")]
    public GameObject m_DriverOnBike;
    public GameObject m_DriverVictory;
    public GameObject m_Police;
    public GameObject m_PoliceBiker;


    public void Func_StartPlay()
    {
        m_CanControl = false;
        Func_RollingWheel();
        m_PathFollower.Event_MoveDone += Func_Victory;
        m_GameManager.m_AudioManager.Func_PlayEngineSound();

    }
    #region Wheel Rolling
    public void Func_RollingWheel()
    {
        m_Mat = new Material(m_Mat);
        m_DriverMat = new Material(m_DriverMat);
        // Set up the rotation animation using DOTween
        m_WheelFront.DOLocalRotate(new Vector3(0, 0, -360), .5f, RotateMode.LocalAxisAdd)
            .SetEase(Ease.Linear)
            .SetLoops(-1, LoopType.Restart);

        m_WheelBack.DOLocalRotate(new Vector3(0, 0, -360), .5f, RotateMode.LocalAxisAdd)
            .SetEase(Ease.Linear)
            .SetLoops(-1, LoopType.Restart);
    }
    public void Func_StopWheel()
    {
        m_WheelFront.DOKill();
        m_WheelBack.DOKill();
    }
    #endregion

    #region Controll
    void Update()
    {
        if (!m_CanControl) return;
        // Move towards point A if the A button is pressed down
        if (Input.GetKeyDown(KeyCode.D))
        {
            m_moving = true;
            m_movingToPointA = true;
            m_movingToPointB = false;

            if (transform.position != m_pointA.position)
            {
                if (!m_SpeedingUp && !m_SlowingDown)
                {
                    m_Anim.SetTrigger("TurnRight");
                }
            }
        }

        // Move towards point B if the D button is pressed down
        if (Input.GetKeyDown(KeyCode.A))
        {

            m_moving = true;
            m_movingToPointA = false;
            m_movingToPointB = true;


            if (transform.position != m_pointB.position)
            {
                if (!m_SpeedingUp && !m_SlowingDown)
                {
                    m_Anim.SetTrigger("TurnLeft");
                }
            }
        }

        // Stop moving if the A or D button is released
        if (Input.GetKeyUp(KeyCode.A) && m_movingToPointA == false)
        {
            m_moving = false;
            if (!m_SpeedingUp && !m_SlowingDown)
                m_Anim.SetTrigger("Running");
        }
        if (Input.GetKeyUp(KeyCode.D) && m_movingToPointB == false)
        {
            m_moving = false;
            if (!m_SpeedingUp && !m_SlowingDown)
                m_Anim.SetTrigger("Running");
        }

        // Move towards the appropriate point if we're currently moving
        if (m_moving)
        {
            if (m_movingToPointA)
            {
                if (transform.position != m_pointA.position && !m_SlowingDown)
                {
                    transform.position = Vector3.MoveTowards(transform.position, m_pointA.position, m_movementSpeed * Time.deltaTime);

                }
            }
            else if (m_movingToPointB)
            {
                if (transform.position != m_pointB.position && !m_SlowingDown)
                {
                    transform.position = Vector3.MoveTowards(transform.position, m_pointB.position, m_movementSpeed * Time.deltaTime);
                }



            }
        }
    }
    public void Func_TurnRightPress()
    {
        if (!m_CanControl) return;
        m_moving = true;
        m_movingToPointA = true;
        m_movingToPointB = false;

        if (transform.position != m_pointA.position)
        {
            if (!m_SpeedingUp && !m_SlowingDown)
            {
                m_Anim.SetTrigger("TurnRight");
            }
        }
    }
    public void Func_TurnRightRelease()
    {
        if (m_movingToPointB == false)
        {
            m_moving = false;
            if (!m_SpeedingUp && !m_SlowingDown)
                m_Anim.SetTrigger("Running");
        }
    }
    public void Func_TurnLeftPress()
    {
        if (!m_CanControl) return;
        m_moving = true;
        m_movingToPointA = false;
        m_movingToPointB = true;


        if (transform.position != m_pointB.position)
        {
            if (!m_SpeedingUp && !m_SlowingDown)
            {
                m_Anim.SetTrigger("TurnLeft");
            }
        }
    }
    public void Func_TurnLeftRelease()
    {
        if (m_movingToPointA == false)
        {
            m_moving = false;
            if (!m_SpeedingUp && !m_SlowingDown)
                m_Anim.SetTrigger("Running");
        }
    }
    #endregion

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("BlockItem"))
        {
            other.GetComponent<Collider>().enabled = false;
            Func_HitBlockItem();
        }
        if (other.CompareTag("MoneyItem"))
        {
            other.GetComponent<Collider>().enabled = false;
            Destroy(other.gameObject);
            Func_TakeMoney();
        }
        if (other.CompareTag("SpeedUpItem"))
        {
            other.GetComponent<Collider>().enabled = false;
            Func_HitSpeedUpItem();
        }
        if (other.CompareTag("SkinItem"))
        {
            Func_HitChangeSkin();
        }
    }

    #region Hit Block
    void Func_HitBlockItem()
    {
        Debug.Log("Hit Block Item");

        if (!m_SpeedingUp)
        {
            m_SlowingDown = true;
            m_Anim.SetTrigger("SlowDown");
            m_GameManager.m_AudioManager.Func_PlaySound(E_Sound._3_SlowDown);
            m_PathFollower.speed -= 8;
            StartCoroutine(ResetSpeed(8));
        }
    }
    IEnumerator ResetSpeed(float _Value)
    {
        yield return new WaitForSeconds(.5f);
        m_PathFollower.speed += _Value;
        m_SlowingDown = false;
        if (m_moving && !m_SpeedingUp)
        {
            if (m_movingToPointA)
            {
                m_Anim.SetTrigger("TurnRight");
            }
            else if (m_movingToPointB)
            {
                m_Anim.SetTrigger("TurnLeft");
            }
        }
        else if (!m_SpeedingUp)
        {
            m_Anim.SetTrigger("Running");
        }
    }

    public void Func_SetSlowingDown(bool _Value)
    {
        m_SlowingDown = _Value;
    }
    #endregion
    #region HitSpeed Up
    void Func_HitSpeedUpItem()
    {
        Debug.Log("Hit Speed Item");
        m_SpeedingUp = true;
        m_Anim.SetTrigger("SpeedUp");
        m_GameManager.m_AudioManager.Func_PlaySound(E_Sound._2_SpeedUp);
        m_PathFollower.speed += 10;
        Func_SetCameraNear();
        StartCoroutine(ResetSpeed_SpeedUp(-10));
    }
    IEnumerator ResetSpeed_SpeedUp(float _Value)
    {
        yield return new WaitForSeconds(1.5f);
        m_PathFollower.speed += _Value;
        m_SpeedingUp = false;
        Func_SetCameraFar();
        if (m_moving && !m_SlowingDown)
        {
            if (m_movingToPointA)
            {
                m_Anim.SetTrigger("TurnRight");
            }
            else if (m_movingToPointB)
            {
                m_Anim.SetTrigger("TurnLeft");
            }
        }
        else if (!m_SlowingDown)
        {
            m_Anim.SetTrigger("Running");
        }
    }
    #endregion
    #region Hit Money
    void Func_TakeMoney()
    {
        m_MoneyUI.Func_AddMoney(1000);
        m_GameManager.m_AudioManager.Func_PlaySound(E_Sound._5_HitMoney);
    }
    #endregion
    #region Hit Change Skin
    void Func_HitChangeSkin()
    {
        m_GameManager.m_VFXController.Func_ShowVFX(transform.position);

        switch (GetUniqueRandomNumber())
        {
            case 0:
                m_Mat.SetColor("_Color", Color.red);
                m_DriverMat.SetColor("_Color", Color.yellow);
                break;
            case 1:
                m_Mat.SetColor("_Color", Color.yellow);
                m_DriverMat.SetColor("_Color", Color.red);
                break;
            case 2:
                m_Mat.SetColor("_Color", Color.green);
                m_DriverMat.SetColor("_Color", Color.cyan);
                break;
            case 3:
                m_Mat.SetColor("_Color", Color.blue);
                m_DriverMat.SetColor("_Color", Color.red);
                break;
        }
        m_MotorSkin.material = m_Mat;
        //m_DriverSkin.materials[1] = m_DriverMat;
        m_GameManager.m_AudioManager.Func_PlaySound(E_Sound._6_HitChangeSkin);


        Material[] materials = m_DriverSkin.materials;
        materials[0] = m_DriverMat;
        m_DriverSkin.materials = materials;
    }

    private int previousResult = -1;
    public int GetUniqueRandomNumber()
    {
        int result = Random.Range(0, 4);

        while (result == previousResult)
        {
            result = Random.Range(0, 4);
        }

        previousResult = result;

        return result;
    }
    #endregion

    #region CameraChange
    public void Func_SetCameraNear()
    {
        if (m_CameraSequence.IsActive())
            m_CameraSequence.Kill();
        m_CameraSequence = DOTween.Sequence();
        m_CameraSequence.Join(m_Camera.DOLocalMove(m_CameraNearPos.localPosition, .2f));
        m_CameraSequence.Join(m_Camera.DOLocalRotate(m_CameraNearPos.localRotation.eulerAngles, .2f));
    }
    public void Func_SetCameraFar()
    {
        if (m_CameraSequence.IsActive())
            m_CameraSequence.Kill();
        m_CameraSequence = DOTween.Sequence();
        m_CameraSequence.Join(m_Camera.DOLocalMove(m_CameraFarPos.localPosition, .5f));
        m_CameraSequence.Join(m_Camera.DOLocalRotate(m_CameraFarPos.localRotation.eulerAngles, .5f));
    }
    public void Func_SetCameraFarAway()
    {
        if (m_CameraSequence.IsActive())
            m_CameraSequence.Kill();
        m_CameraSequence = DOTween.Sequence();
        m_CameraSequence.Join(m_Camera.DOLocalMove(m_CameraFarAwayPos.localPosition, .5f));
        m_CameraSequence.Join(m_Camera.DOLocalRotate(m_CameraFarAwayPos.localRotation.eulerAngles, .5f));
    }
    #endregion
    #region End
    public void Func_Victory()
    {
        m_PathFollower.Event_MoveDone -= Func_Victory;
        m_Police.SetActive(false);
        m_PoliceBiker.SetActive(false);
        m_DriverOnBike.SetActive(false);
        m_DriverVictory.SetActive(true);
        Func_StopWheel();
        m_CanControl = false;
        m_GameManager.Func_Victory();
        m_GameManager.m_AudioManager.Func_PlaySound(E_Sound._0_Victory);
        m_GameManager.m_AudioManager.Func_StopEngineSound();
    }
    public void Func_Lose()
    {
        Func_SetCameraNear();
        m_PathFollower.Func_SetMoveDone(true);
        m_CanControl = false;
        transform.localPosition = Vector3.zero;
        m_Police.SetActive(true);
        m_PoliceBiker.SetActive(true);
        m_DriverOnBike.SetActive(true);
        m_DriverVictory.SetActive(false);
        Func_StopWheel();
        m_GameManager.m_AudioManager.Func_PlaySound(E_Sound._1_Defeat);
        m_GameManager.m_AudioManager.Func_StopEngineSound();
    }
    #endregion

}//end class

