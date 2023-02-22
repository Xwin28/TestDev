using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;


public class S_MoneyUI : MonoBehaviour
{
    public TextMeshProUGUI m_TxtMoney;
    public int m_CurrentCoin;
    // __________________________FUNTION______________
    public void Func_AddMoney(int _Money)
    {
        m_CurrentCoin += _Money;
        m_TxtMoney.text = m_CurrentCoin.ToString();
        m_TxtMoney.transform.localScale = Vector3.one;
        m_TxtMoney.transform.DOShakeScale(.2f, .2f);
    }

}//end class

