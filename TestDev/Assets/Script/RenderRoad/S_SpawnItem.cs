using DG.Tweening;
using PathCreation.Examples;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class S_SpawnItem : MonoBehaviour
{
    [Header("Item")]
    public GameObject m_MoneyItem;
    public GameObject m_SpeedItem;
    public GameObject m_SkinItem;
    public GameObject m_BlockItem;
    [Header("Spawner")]
    public Transform m_Container;
    public PathFollower m_PathFollower;
    public Transform m_PointA;
    public Transform m_PointB;
    public Transform m_SpawnPoint;
    public Transform m_SpawnSpeedPoint;
    public Transform m_SpawnSkinChangePoint;
    [Header("This Must has 3 element")]
    public Transform[] m_SpawnBlockPoint;

    public float m_speedMoneySpawner = 100;
    public float m_speedSpeedSpawner = 100;
    public float m_speedSkinSpawner = 100;
    public bool m_SpawnDone;
    [Header("TIMING")]
    public float m_TimeDelayEachBlockType;
    public float m_TimeDelayEachBlock;
    public float m_TimeDelayEachMoney;
    public float m_TimeDelayEachSpeedItem;
    public float m_TimeDelayEachSkinItem;
    // __________________________FUNTION______________

    private void Start()
    {
        m_PathFollower.Event_MoveDone += Func_SpawnItemDone;
        StartCoroutine(MoveLoop());
        StartCoroutine(SpawnMoneyItem());
        StartCoroutine(SpawnBlockItem());
        StartCoroutine(SpawnSpeedItem());
        StartCoroutine(SpawnSkinItem());
    }
    void Func_SpawnItemDone()
    {
        m_PathFollower.Event_MoveDone -= Func_SpawnItemDone;
        m_SpawnDone = true;
    }

    private IEnumerator MoveLoop()
    {
        bool _MoveForawd = true;
        while (!m_SpawnDone)
        {
            // Move towards the target position
            if (_MoveForawd)
            {
                m_SpawnPoint.position = Vector3.MoveTowards(m_SpawnPoint.position, m_PointB.position, m_speedMoneySpawner * Time.deltaTime);

                m_SpawnSpeedPoint.position = Vector3.MoveTowards(m_SpawnSpeedPoint.position, m_PointA.position, m_speedSpeedSpawner * Time.deltaTime);
                m_SpawnSkinChangePoint.position = Vector3.MoveTowards(m_SpawnSkinChangePoint.position, m_PointA.position, m_speedSpeedSpawner * Time.deltaTime);
            }
            else
            {
                m_SpawnPoint.position = Vector3.MoveTowards(m_SpawnPoint.position, m_PointA.position, m_speedMoneySpawner * Time.deltaTime);

                m_SpawnSpeedPoint.position = Vector3.MoveTowards(m_SpawnSpeedPoint.position, m_PointB.position, m_speedSpeedSpawner * Time.deltaTime);
                m_SpawnSkinChangePoint.position = Vector3.MoveTowards(m_SpawnSkinChangePoint.position, m_PointB.position, m_speedSpeedSpawner * Time.deltaTime);
            }


            // If we reach the target position, switch to the other one and wait for the specified time
            if (Vector3.Distance(m_SpawnPoint.position, m_PointA.position) < .1f)
            {
                _MoveForawd = true;
            }
            else if (Vector3.Distance(m_SpawnPoint.position, m_PointB.position) < .1f)
            {
                _MoveForawd = false;
            }




            yield return null;
        }
    }





    #region Spawn Money
    IEnumerator SpawnMoneyItem()
    {
        //m_speedMoneySpawner = Random.Range(2, 10);
        yield return new WaitForSeconds(m_TimeDelayEachMoney);
        if (!m_SpawnDone)
        {
            Func_SpawnMoneyItem();
            StartCoroutine(SpawnMoneyItem());
        }
    }
    public void Func_SpawnMoneyItem()
    {
        var _Temp = Instantiate(m_MoneyItem, transform);
        _Temp.transform.position = m_SpawnPoint.position;
        _Temp.transform.SetParent(m_Container);
        _Temp.gameObject.SetActive(true);
    }
    #endregion
    #region Spawn Speed Item
    IEnumerator SpawnSpeedItem()
    {
        //m_speedMoneySpawner = Random.Range(2, 10);
        yield return new WaitForSeconds(m_TimeDelayEachSpeedItem);
        if (!m_SpawnDone)
        {
            Func_SpawnSpeedItem();
            StartCoroutine(SpawnSpeedItem());
        }
    }
    public void Func_SpawnSpeedItem()
    {
        var _Temp = Instantiate(m_SpeedItem, transform);
        _Temp.transform.position = m_SpawnBlockPoint[Random.Range(0, 3)].position;
        _Temp.transform.SetParent(m_Container);
        _Temp.gameObject.SetActive(true);
    }
    #endregion
    #region Spawn Skin Item
    IEnumerator SpawnSkinItem()
    {
        //m_speedMoneySpawner = Random.Range(2, 10);
        yield return new WaitForSeconds(m_TimeDelayEachSkinItem);
        if (!m_SpawnDone)
        {
            Func_SpawnSkinItem();
            StartCoroutine(SpawnSkinItem());
        }
    }
    public void Func_SpawnSkinItem()
    {
        var _Temp = Instantiate(m_SkinItem, transform);
        _Temp.transform.position = m_SpawnBlockPoint[Random.Range(0, 3)].position;
        _Temp.transform.SetParent(m_Container);
        _Temp.gameObject.SetActive(true);
    }
    #endregion
    #region Spawn Block Item
    IEnumerator SpawnBlockItem()
    {
        yield return new WaitForSeconds(m_TimeDelayEachBlockType);
        if (!m_SpawnDone)
            switch (Random.Range(0, 4))
            {
                case 0:
                    StartCoroutine(SpawnBlockItem_Type_1());
                    break;
                case 1:
                    StartCoroutine(SpawnBlockItem_Type_2());
                    break;
                case 2:
                    StartCoroutine(SpawnBlockItem_Type_3());
                    break;
                case 3:
                    StartCoroutine(SpawnBlockItem_Type_4());
                    break;
            }

    }
    //Spawn Block Type 1 (2 Left 5 right)
    IEnumerator SpawnBlockItem_Type_1()
    {
        int _Count = 0;
        while (_Count < 7)
        {
            if (m_SpawnDone) break;
            yield return new WaitForSeconds(m_TimeDelayEachBlock);
            if (_Count < 2)
            {
                Func_SpawnBlockItem(true);
            }
            else
            {
                Func_SpawnBlockItem(false);
            }
            _Count += 1;
        }

        yield return null;
        StartCoroutine(SpawnBlockItem());
    }
    //Spawn Block Type 2 (alternate - Left Right Left Right 10 Time)
    IEnumerator SpawnBlockItem_Type_2()
    {
        int _Count = 0;
        while (_Count < 10)
        {
            if (m_SpawnDone) break;
            yield return new WaitForSeconds(m_TimeDelayEachBlock);
            if (_Count % 2 == 0)
            {
                Func_SpawnBlockItem(true);
            }
            else
            {
                Func_SpawnBlockItem(false);
            }
            _Count += 1;
        }
        yield return null;
        StartCoroutine(SpawnBlockItem());
    }
    //Spawn Block Type 3 (1 2 1 2)
    IEnumerator SpawnBlockItem_Type_3()
    {
        int _Count = 0;
        while (_Count < 10)
        {
            if (m_SpawnDone) break;
            yield return new WaitForSeconds(m_TimeDelayEachBlock);
            if (_Count % 3 == 0)
            {
                Func_SpawnBlockItem(true);
            }
            else
            {
                Func_SpawnBlockItem(false);
            }
            _Count += 1;
        }
        yield return null;
        StartCoroutine(SpawnBlockItem());
    }
    //Spawn Block Type 4 (Random)
    IEnumerator SpawnBlockItem_Type_4()
    {
        int _Count = 0;
        while (_Count < 10)
        {
            if (m_SpawnDone) break;
            yield return new WaitForSeconds(m_TimeDelayEachBlock);
            Func_SpawnBlockItem_Random();
        }
        yield return null;
        StartCoroutine(SpawnBlockItem());
    }


    public void Func_SpawnBlockItem_Random()
    {
        var _Temp = Instantiate(m_BlockItem, transform);
        switch (Random.Range(0, 3))
        {
            case 0:
                _Temp.transform.position = m_SpawnBlockPoint[0].position;
                break;
            case 1:
                _Temp.transform.position = m_SpawnBlockPoint[1].position;
                break;
            case 2:
                _Temp.transform.position = m_SpawnBlockPoint[2].position;
                break;
        }
        _Temp.transform.SetParent(m_Container);
        _Temp.gameObject.SetActive(true);
    }
    public void Func_SpawnBlockItem(in bool _IsLeft)
    {
        var _Temp = Instantiate(m_BlockItem, transform);
        if (_IsLeft)
            _Temp.transform.position = m_SpawnBlockPoint[2].position;
        else
            _Temp.transform.position = m_SpawnBlockPoint[0].position;
        _Temp.transform.SetParent(m_Container);
        _Temp.gameObject.SetActive(true);
    }
    #endregion



}//end class

