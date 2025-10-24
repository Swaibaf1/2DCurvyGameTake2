using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    Camera m_cam;

    bool m_zoomedIn = false;
    [SerializeField] GameObject m_target;
    [SerializeField] bool m_usePredeterminedOffset;
    [SerializeField] Vector2 m_offset;

    bool m_moveCamera;
    Vector3 m_previousCameraPosition;


    public void Awake()
    {
        m_cam = this.GetComponent<Camera>();
        m_zoomedIn = false;
        m_moveCamera = true;

        if (m_target != null && !m_usePredeterminedOffset)
        {
            CalculateCameraOffset(m_target);
        }
    }

    public void OnCameraZoomIn()
    {
        m_cam.orthographicSize = 4f;
        m_moveCamera = true;

    }

    public void OnCameraZoomOut()
    {
        m_cam.orthographicSize = 8f;
        m_moveCamera = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (m_moveCamera)
        {
            MoveCameraToTarget();
        }
        else
        {

        }

    }

    public void MoveCameraToTarget()
    {
        Vector2 _targetPos = new Vector2(
            m_target.transform.position.x,
            m_target.transform.position.y)
            + m_offset;

        Vector3 _finalPos = new Vector3(
            _targetPos.x,
            _targetPos.y,
            m_cam.transform.position.z);

        m_cam.transform.position = _finalPos;
    }



    public void ResetCameraPos(bool _bool)
    {
        if (_bool)
        {
            m_previousCameraPosition = m_cam.transform.position;
            m_cam.transform.position = new Vector3(0, 0, m_cam.transform.position.z);

            Time.timeScale = 0f;
        }
        else
        {
            m_cam.transform.position = m_previousCameraPosition;

            Time.timeScale = 1.0f;
        }
    }

    public void CalculateCameraOffset(GameObject _target)
    {
        m_offset = m_cam.transform.position - _target.transform.position;
    }

    void ChangeCameraTarget(GameObject _newTarget)
    {
        m_target = _newTarget;
        CalculateCameraOffset(_newTarget);
    }

    public bool ZoomedIn { get { return m_zoomedIn; } set { m_zoomedIn = value; } }

    public bool MoveCamera { get { return m_moveCamera; } set { m_moveCamera = value; } }
    public GameObject Target { get { return m_target; } set { m_target = value; } }



}
