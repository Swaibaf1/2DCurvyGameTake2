using System.Collections;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] Transform m_target;
    [SerializeField] Vector2 m_screenUnits;


    bool m_canSwitchCamera;
    Camera m_cam; 
    private void Awake()
    {
        m_cam = this.GetComponent<Camera>();
        m_canSwitchCamera = true;
    }

    private void Update()
    {
        CheckForOutOfBounds();
    }

    void CheckForOutOfBounds()
    {
        if (m_canSwitchCamera)
        {
            Vector3 _newPosToAdd = Vector3.zero;

            if (m_target.position.x <= m_cam.transform.position.x - m_screenUnits.x / 2)
            {
                _newPosToAdd.x = -m_screenUnits.x;
            }

            if (m_target.position.x >= m_cam.transform.position.x + m_screenUnits.x / 2)
            {
                _newPosToAdd.x = m_screenUnits.x;
            }

            if (m_target.position.y <= m_cam.transform.position.y - m_screenUnits.y / 2)
            {

                _newPosToAdd.y = -m_screenUnits.y;
            }

            if (m_target.position.y >= m_cam.transform.position.y + m_screenUnits.y / 2)
            {
                _newPosToAdd.y = m_screenUnits.y;
            }

            if (_newPosToAdd != Vector3.zero)
            {

                m_cam.transform.position += _newPosToAdd;
                StartCoroutine(SetFalseForSeconds(m_canSwitchCamera, 1));
            }

        }

    }

    IEnumerator SetFalseForSeconds(bool _bool, float _duration)
    {
        _bool = !_bool;
        yield return new WaitForSeconds(_duration);
        _bool = !_bool;

    }
}
