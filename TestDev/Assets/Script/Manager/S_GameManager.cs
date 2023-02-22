using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class S_GameManager : MonoBehaviour
{
    public S_AudioManager m_AudioManager;
    public S_VFXController m_VFXController;
    public S_PlayerController m_PlayerController;
    public S_PoliceController m_PoliceController;
    public GameObject m_StartPoint;
    public GameObject m_EndPoint;
    [Header("UI")]
    public GameObject m_DefeatUI;
    public GameObject m_VictoryUI;
    public GameObject m_BtnLeft, m_BtnRight;
    // __________________________FUNTION______________
    private void Start()
    {
        Func_BeginPlay();
    }
    #region Begin
    public void Func_BeginPlay()
    {
        m_StartPoint.SetActive(true);
        m_BtnLeft.SetActive(true);
        m_BtnRight.SetActive(true);
        //Player
        m_PlayerController.Func_StartPlay();
        m_PlayerController.m_PathFollower.speed -= 10;

        m_PoliceController.m_CanControl = false;
        m_PoliceController.m_PathFollower.speed -= 10;
    }

    public void Func_HitStartPoint()
    {
        
        //Slow motion
        Time.timeScale = .2f;
        //Action Police alert

        //Start Controll
        m_PlayerController.m_CanControl = true;
        m_PlayerController.m_PathFollower.speed += 10;

        //m_PoliceController.m_CanControl = true;
        StartCoroutine(DelayCopChase());
    }
    public float m_SpeedTest;
    IEnumerator DelayCopChase()
    {
        yield return new WaitForSeconds(.5f);
        m_PoliceController.m_PathFollower.speed += 10;
        m_PoliceController.m_PathFollower.speed += m_SpeedTest;
        //Slow motion
        Time.timeScale = 1;
        //Showup Police
        m_PoliceController.gameObject.SetActive(true);
        StartCoroutine(DelayReduceSpeedCop());

        m_AudioManager.Func_PlaySound(E_Sound._4_PoliceAlert);
    }
    IEnumerator DelayReduceSpeedCop()
    {
        yield return new WaitForSeconds(1.5f);
        m_PoliceController.m_PathFollower.speed -= m_SpeedTest;
        m_StartPoint.SetActive(false);
        m_PlayerController.Func_SetCameraFarAway();
    }
    #endregion

    #region Winning
    public void Func_Victory()
    {
        m_PlayerController.transform.position = m_EndPoint.transform.position;
        m_PoliceController.gameObject.SetActive(false);
        m_PoliceController.m_PathFollower.Func_SetMoveDone(true);
        m_VictoryUI.SetActive(true);
        m_BtnLeft.SetActive(false);
        m_BtnRight.SetActive(false);
    }
    #endregion

    #region Defeat
    public void Func_Defeat()
    {
        m_PlayerController.Func_Lose();
        m_PoliceController.gameObject.SetActive(false);
        m_PoliceController.m_PathFollower.Func_SetMoveDone(true);
        m_DefeatUI.SetActive(true);
        m_AudioManager.Func_PlaySound(E_Sound._4_PoliceAlert);
        m_BtnLeft.SetActive(false);
        m_BtnRight.SetActive(false);
    }
    #endregion

    #region UI
    public void Func_AfterFinish()
    {
        int _temp = Random.Range(0, 4);
        Debug.Log("After Finish = " + _temp);
        SceneManager.LoadScene(_temp);
    }

    #endregion
}//end class

