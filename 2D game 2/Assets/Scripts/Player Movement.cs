using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using UnityEditorInternal;
public class PlayerMovement : MonoBehaviour
{


    #region variables 
    [Header("Move Speed Variables")]

    [SerializeField] float m_accelarateSpeed;
    [SerializeField] float m_airDamping;
    [SerializeField] float m_slopeSpeed;
    [SerializeField] float m_jumpForce;
    bool m_decelarating = false;

    [SerializeField] float m_extraGravityValue;

    Vector2 m_gravityDirection;

    [Header("Ground Check Stuff")]

    [SerializeField] Transform m_groundCheckLeft;
    [SerializeField] Transform m_groundCheckRight;
    [SerializeField] LayerMask m_groundLayer;
    [SerializeField] LayerMask m_threatLayer;


    [SerializeField] Transform m_raycastPointLeft;
    [SerializeField] Transform m_raycastPointRight;

    [Header("Timer Stuff")]

    [SerializeField] float m_coyoteTime;
    float m_coyoteTimeCounter;

    [SerializeField] float m_jumpBufferTime;
    float m_jumpBufferCounter;

    bool m_isGrounded;
    [SerializeField] bool m_canMove = true;

    Rigidbody2D m_rb;
    Vector2 m_moveValue;


    [SerializeField] GameObject m_sprite;
    


    #endregion

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
        m_rb = this.GetComponent<Rigidbody2D>();
        m_gravityDirection = Vector2.down;
    }



    void FixedUpdate()
    {
        float _extraGravityForce = 0f;

        if (Physics2D.Raycast(m_groundCheckLeft.position, m_gravityDirection, 0.5f, m_groundLayer)
            || (Physics2D.Raycast(m_groundCheckRight.position, m_gravityDirection, 0.5f, m_groundLayer)))
        {
            m_isGrounded = true;
            m_coyoteTimeCounter = m_coyoteTime;
            _extraGravityForce = 0f;
        }
        else
        {
            m_isGrounded = false;
            _extraGravityForce = -m_extraGravityValue;
            m_coyoteTimeCounter -= Time.deltaTime;
        }
        float _moveForce = 0f;
        
        if(m_moveValue.x !=0)
        {
            FlipSprite();
            if (m_isGrounded)
            {
                _moveForce = m_moveValue.x * m_accelarateSpeed;

            }
            else
            {
                _moveForce = m_moveValue.x * m_airDamping;
            }
        }


        print(m_rb.linearVelocity.magnitude);

        Vector2 _finalGravity = _extraGravityForce * m_gravityDirection;


        m_rb.linearVelocity -= _finalGravity;

        m_rb.linearVelocity += new Vector2(transform.right.x, transform.right.y) * _moveForce;
        
       


        RotateDirection();
    }

    void BaseMovement(float _accelarateSpeed, float _decelarateIncrement, Vector2 _velocityToAdd)
    {
        if (m_moveValue.x != 0)
        {
            if (Mathf.Sign(m_rb.linearVelocityX) != Mathf.Sign(m_moveValue.x))
            {
                _velocityToAdd = new Vector2(m_moveValue.x * _accelarateSpeed * 2, 0);

                //m_rb.AddForce(new Vector2(m_moveValue.x * _accelarateSpeed * 6, 0));
            }
            else
            {
                _velocityToAdd = new Vector2(m_moveValue.x * _accelarateSpeed, 0);
                //m_rb.AddForce(new Vector2(m_moveValue.x * _accelarateSpeed, 0));
            }
        }
        else if (m_rb.linearVelocityX != 0)
        {

            if (!m_decelarating)
            {
                StartCoroutine(Deccelarate(_decelarateIncrement));
            }


        }
    }


    void RotateDirection()
    {

        RaycastHit2D rayLeft = Physics2D.Raycast(m_raycastPointLeft.position, -transform.up, 1.5f);

        RaycastHit2D rayRight = Physics2D.Raycast(m_raycastPointRight.position, -transform.up, 1.5f);

        if (rayLeft && rayRight)
        {

            Debug.DrawLine(rayLeft.point, rayRight.point, Color.red);

            Vector2 _planeVector = rayRight.point - rayLeft.point;

            Vector2 _planeVectorNormal = new Vector2(-_planeVector.y, _planeVector.x);

            transform.up = _planeVectorNormal;



        }
        else
        {
            transform.up = Vector2.up;
        }

        m_gravityDirection = -transform.up;
    }
    IEnumerator Deccelarate(float _decrementValue)
    {
        m_decelarating = true;
        float i = 0;
        float _currentVelocity = m_rb.linearVelocity.magnitude;

        while (m_moveValue.x == 0 && m_rb.linearVelocityX != 0)
        {

            m_rb.linearVelocity = transform.right * _decrementValue;
            yield return new WaitForSeconds(0.01f);
        }


        m_decelarating = false;
    }

    void FlipSprite()
    {
        if(m_moveValue.x < 0)
        {
            m_sprite.transform.localRotation = new Quaternion(0,180,0,0);
        }
        else if(m_moveValue.x > 0)
        {
            m_sprite.transform.localRotation = Quaternion.identity;
        }




    }

    public void OnPlayerMove(InputAction.CallbackContext _context)
    {
        if (_context.started)
        {
            m_moveValue.x = _context.ReadValue<Vector2>().x;
        }

        if (_context.canceled)
        {
            m_moveValue.x = 0f;
        }

    }

    public void OnJumpPressed(InputAction.CallbackContext _context)
    {
        if (_context.started && m_coyoteTimeCounter > 0f)
        {
            /*m_rb.AddForce(-m_gravityDirection * m_jumpForce, ForceMode2D.Impulse);*/
             m_rb.linearVelocity += -m_gravityDirection * m_jumpForce ;

            //m_rb.linearVelocityY = m_rb.linearVelocityY + m_jumpForce;
            m_coyoteTimeCounter = 0f;
        }

        if (_context.canceled && m_rb.linearVelocityY > 0f)
        {

            m_rb.linearVelocityY = m_rb.linearVelocityY * 0.2f;


            m_coyoteTimeCounter = 0f;
        }
    }
}
