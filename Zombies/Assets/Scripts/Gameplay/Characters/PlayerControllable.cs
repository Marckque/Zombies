using UnityEngine;

namespace HumansVersusZombies
{
    public class PlayerControllable : MonoBehaviour
    {
        [Header("Camera"), SerializeField]
        private Camera m_Camera;
        [SerializeField]
        protected Transform m_CameraRoot;
        [SerializeField]
        private Transform m_Head;

        [Header("Health"), SerializeField]
        private HealthManager m_HealthManager;

        [Header("Movement"), SerializeField]
        private CharacterController m_CharacterController;
        [SerializeField]
        private AnimationCurve m_AccelerationCurve;
        [SerializeField]
        private float m_RunVelocity;
        [SerializeField]
        private float m_CrouchVelocity;
        [SerializeField]
        private float m_JumpHeight;
        [SerializeField]
        private AnimationCurve m_JumpCurve;

        private bool m_IsRunning;
        private bool m_IsCrouched;
        private bool m_IsJumping;
        private float m_MaxVelocity;
        private Vector3 m_Gravity;
        private Vector3 m_CurrentInput;
        private Vector3 m_DesiredVelocityDirection;
        private float m_AccelerationTimer;
        private float m_AccumulatedJumpHeight;

        public int CurrentManagerID { get; set; }
        public int PlayerID { get; set; }
        public Camera PlayerCamera { get { return m_Camera; } }
        public Transform PlayerCameraRoot { get { return m_CameraRoot; } }
        public HealthManager HealthManager { get { return m_HealthManager; } }

        protected void Update()
        {
            CheckInputs();
            Movement();
        }

        protected void FixedUpdate()
        {
            //Movement(); // Fixed update ?
        }

        #region Controls
        protected virtual bool MainAction()
        {
            return Input.GetMouseButton(0);
        }

        protected virtual bool SecondaryAction()
        {
            return Input.GetMouseButton(1);
        }

        protected virtual void CheckInputs()
        {
            GetMovementInput();
        }

        protected void Movement()
        {
            if (m_CurrentInput != Vector3.zero)
            {
                m_AccelerationTimer += Time.deltaTime;
            }
            else
            {
                m_AccelerationTimer = 0;
            }

            m_DesiredVelocityDirection = transform.forward * m_CurrentInput.z + transform.right * m_CurrentInput.x;
            m_DesiredVelocityDirection *= m_AccelerationCurve.Evaluate(m_AccelerationTimer);

            if (!m_CharacterController.isGrounded)
            {
                m_Gravity += Physics.gravity * Time.deltaTime;
            }
            else
            {
                m_Gravity = Vector3.zero;

                if (Input.GetKey(KeyCode.Space) && !m_IsCrouched)
                {
                    m_IsJumping = true;
                }
            }

            Jump();

            m_DesiredVelocityDirection += m_Gravity;
            m_CharacterController.Move(m_DesiredVelocityDirection.normalized * m_MaxVelocity * Time.deltaTime);
        }

        private void GetMovementInput()
        {
            Crouch();

            m_MaxVelocity = m_IsCrouched ? m_CrouchVelocity : m_RunVelocity;

            m_CurrentInput = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;
        }

        private void Crouch()
        {
            m_IsCrouched = Input.GetKey(KeyCode.LeftShift);

            // TO DO: Think about a better solution if necessary - BoxCollider instead of RayCast so that it doesn't make you stand up when the middle of the collider is not in ray range
            if (m_IsCrouched)
            {
                m_CharacterController.height = 1;
            }
            else
            {
                Ray ray = new Ray(m_Head.position, Vector3.up);
                RaycastHit hit;

                Debug.DrawRay(ray.origin, ray.direction * 2f, Color.red, 0.75f);

                if (!Physics.Raycast(ray, out hit)) // SphereCast à la place
                {
                    m_CharacterController.height = 2;
                }
                else
                {
                    m_IsCrouched = true;
                }
            }
        }

        private void Jump()
        {
            if (m_IsJumping)
            {
                m_AccumulatedJumpHeight += Time.deltaTime;
                m_Gravity.y += m_JumpCurve.Evaluate(m_AccumulatedJumpHeight) * m_JumpHeight;

                if (m_Gravity.y >= m_JumpHeight)
                {
                    m_IsJumping = false;
                    m_AccumulatedJumpHeight = 0;
                }
            }   
        }
        #endregion Controls
    }
}