using UnityEngine;

namespace HumansVersusZombies
{
    public class PlayerControllable : MonoBehaviour
    {
        [Header("Camera"), SerializeField]
        private Camera m_Camera;
        [SerializeField]
        protected Transform m_CameraRoot;

        [Header("Health"), SerializeField]
        private HealthManager m_HealthManager;

        [Header("Movement"), SerializeField]
        private CharacterController m_CharacterController;
        [SerializeField]
        private float m_RunVelocity;
        [SerializeField]
        private float m_CrouchVelocity;

        private bool m_IsRunning;
        private float m_MaxVelocity;
        private Vector3 m_CurrentInput;
        private Vector3 m_DesiredVelocityDirection;

        public int CurrentID { get; set; }
        public Camera PlayerCamera { get { return m_Camera; } }
        public Transform PlayerCameraRoot { get { return m_CameraRoot; } }
        public HealthManager HealthManager { get { return m_HealthManager; } }

        protected void Update()
        {
            CheckInputs();
        }

        protected void FixedUpdate()
        {
            Movement();
        }

        #region Controls
        protected virtual void CheckInputs()
        {
            GetMovementInput();
        }

        protected void Movement()
        {
            m_DesiredVelocityDirection = transform.forward * m_CurrentInput.z + transform.right * m_CurrentInput.x;

            if (!m_CharacterController.isGrounded)
            {
                Vector3 gravity = Physics.gravity * Time.fixedDeltaTime;
                m_DesiredVelocityDirection += gravity;
            }

            m_CharacterController.Move(m_DesiredVelocityDirection * m_MaxVelocity * Time.fixedDeltaTime);
        }

        private void GetMovementInput()
        {
            m_IsRunning = !Input.GetKey(KeyCode.C);
            Crouch();

            m_MaxVelocity = m_IsRunning ? m_RunVelocity : m_CrouchVelocity;

            m_CurrentInput = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;
        }

        private void Crouch()
        {
            // TO DO: Think about a better solution if necessary - BoxCollider instead of RayCast so that it doesn't make you stand up when the middle of the collider is not in ray range
            if (!m_IsRunning)
            {
                m_CharacterController.height = 1;
            }
            else
            {
                Ray ray = new Ray(transform.position, Vector3.up);
                RaycastHit hit;

                if (!Physics.Raycast(ray, out hit))
                {
                    m_CharacterController.height = 2;
                }
                else
                {
                    m_IsRunning = false;
                }
            }
        }
        #endregion Controls
    }
}