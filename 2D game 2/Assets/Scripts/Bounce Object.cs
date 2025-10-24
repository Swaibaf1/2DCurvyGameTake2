using UnityEngine;
using System.Collections;
public class BounceObject : MonoBehaviour
{
    [SerializeField] float m_bounceForce;
    bool m_hasBounced = false;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D _collision)
    {
        if(_collision.GetComponentInParent<Rigidbody2D>() != null && !m_hasBounced)
        {
            StartCoroutine(Bounce(_collision.GetComponentInParent<Rigidbody2D>()));
        }
    }
    private void OnTriggerExit2D(Collider2D _collision)
    {
        if (_collision.GetComponentInParent<Rigidbody2D>() != null)
        {

        }
    }

    IEnumerator Bounce(Rigidbody2D _body)
    {
        //m_animator.Play("Bounce", -1, 0f);
        _body.linearVelocity = Vector3.zero;
        _body.AddForce(this.transform.up * m_bounceForce, ForceMode2D.Impulse);
        m_hasBounced = true;
        yield return new WaitForSeconds(0.5f);
        m_hasBounced = false;
    }

}
