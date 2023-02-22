using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class S_RenderPath : MonoBehaviour
{
    public Transform m_PointA;
    public Transform m_PointB;
    public Transform m_PointC;
    public Transform m_PointD;
    public Transform m_PointAB;
    public Transform m_PointBC;
    public Transform m_PointCD;
    public Transform m_PointAB_BC;
    public Transform m_PointBC_CD;
    public Transform m_PointABCD;

    float m_interpolateAmount;

    private void Update()
    {
        m_interpolateAmount = (m_interpolateAmount + Time.deltaTime) % 1f;
        /*m_PointAB.position = Vector3.Lerp(m_PointA.position,
            m_PointB.position, m_interpolateAmount);
        m_PointBC.position = Vector3.Lerp(m_PointB.position,
            m_PointC.position, m_interpolateAmount);
        m_PointCD.position = Vector3.Lerp(m_PointC.position,
            m_PointD.position, m_interpolateAmount);

        m_PointAB_BC.position = Vector3.Lerp(m_PointAB.position,
            m_PointBC.position, m_interpolateAmount);
        m_PointBC_CD.position = Vector3.Lerp(m_PointBC.position,
            m_PointCD.position, m_interpolateAmount);

        m_PointABCD.position = Vector3.Lerp(m_PointAB_BC.position,
            m_PointBC_CD.position, m_interpolateAmount);*/

        m_PointABCD.position = CubicLerp(m_PointA.position, m_PointB.position, m_PointC.position, m_PointD.position, m_interpolateAmount);
    }

    private Vector3 QuadraticLerp(Vector3 a, Vector3 b, Vector3 c, float t)
    {
        Vector3 ab = Vector3.Lerp(a, b, t);
        Vector3 bc = Vector3.Lerp(b, c, t);

        return Vector3.Lerp(ab, bc, m_interpolateAmount);
    }

    private Vector3 CubicLerp(Vector3 a, Vector3 b, Vector3 c, Vector3 d, float t)
    {
        Vector3 ab_bc = QuadraticLerp(a, b, c, t);
        Vector3 bc_cd = QuadraticLerp(b, c, d, t);

        return Vector3.Lerp(ab_bc, bc_cd, m_interpolateAmount);
    }
}//end class

