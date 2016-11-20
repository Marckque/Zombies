using UnityEngine;

namespace HumansVersusZombies
{
    public class PlayerControllable : MonoBehaviour
    {
        #region Variables
        [Header("CharacterController"), SerializeField]
        private CharacterController m_CharacterController;

        [Header("Camera"), SerializeField]
        private Transform m_Head;
        [SerializeField]
        private Camera m_Camera;
        [SerializeField]
        protected Transform m_CameraRoot;

        [Header("Health"), SerializeField]
        private HealthManager m_HealthManager;

        [Header("Movement"), SerializeField]
        private AnimationCurve m_AccelerationCurve;
        [SerializeField]
        private float m_RunVelocity;
        [SerializeField]
        private float m_CrouchVelocity;

        [Header("Jump"), SerializeField]
        private AnimationCurve m_JumpCurve;
        [SerializeField]
        private float m_JumpHeight;

        [Header("Weapons"), SerializeField]
        protected Weapon[] m_Weapons;
        protected int m_CurrentWeapon;

        private bool m_IsRunning;
        private bool m_IsCrouched;
        private bool m_IsJumping;
        private float m_MaxVelocity;
        private Vector3 m_Gravity;
        private Vector3 m_CurrentInput;
        private Vector3 m_DesiredVelocityDirection;
        private float m_AccelerationTime;
        private float m_JumpTime;

        public int CurrentManagerID { get; set; }
        public int PlayerID { get; set; }
        public Camera PlayerCamera { get { return m_Camera; } }
        public Transform PlayerCameraRoot { get { return m_CameraRoot; } }
        public HealthManager HealthManager { get { return m_HealthManager; } }
        #endregion Variables

        protected void Start()
        {
            InitialiseWeapons();
        }

        protected void Update()
        {
            CheckInputs();
        }

        protected virtual void CheckInputs()
        {
            GetMovementInput();
            GetWeaponsInput();
        }

        protected void GetWeaponsInput()
        {
            ChangeWeapon();

            if (Input.GetMouseButton(0))
            {
                MainWeaponAction();
            }

            if (Input.GetMouseButton(1))
            {
                SecondaryWeaponAction();
            }
        }

        #region Inventory
        private void InitialiseWeapons()
        {
            if (m_Weapons.Length == 0)
            {
                Debug.Log("A human should have a weapon."); // In the game at least.
            }
            else
            {
                m_CurrentWeapon = 0;

                foreach (Weapon weapon in m_Weapons)
                {
                    weapon.GetCurrentPlayer(this);
                }

                UpdateWeaponModel();
            }
        }

        private void ChangeWeapon()
        {
            float mouseWheel = Input.GetAxis("Mouse ScrollWheel");

            // Not sure why I need to make sure it's different from 0... But else it keeps on switching from 0 to 1 and vice-versa... !
            if (mouseWheel != 0)
            {
                if (mouseWheel > 0)
                {
                    if (m_CurrentWeapon < m_Weapons.Length - 1)
                    {
                        m_CurrentWeapon++;
                    }
                    else
                    {
                        m_CurrentWeapon = 0;
                    }
                }
                else
                {
                    if (m_CurrentWeapon > 0)
                    {
                        m_CurrentWeapon--;
                    }
                    else
                    {
                        m_CurrentWeapon = m_Weapons.Length - 1;
                    }
                }

                UpdateWeaponModel();
            }
        }

        private void UpdateWeaponModel()
        {
            foreach (Weapon weapon in m_Weapons)
            {
                if (weapon != m_Weapons[m_CurrentWeapon])
                {
                    weapon.gameObject.SetActive(false);
                }
                else
                {
                    weapon.gameObject.SetActive(true);
                }
            }
        }

        private void MainWeaponAction()
        {
            m_Weapons[m_CurrentWeapon].CanShoot();
        }

        private void SecondaryWeaponAction()
        {
            // Secondary action
        }
        #endregion Inventory

        #region Movement
        protected void Movement()
        {
            if (m_CurrentInput != Vector3.zero)
            {
                m_AccelerationTime += Time.deltaTime;
            }
            else
            {
                m_AccelerationTime = 0;
            }

            m_DesiredVelocityDirection = transform.forward * m_CurrentInput.z + transform.right * m_CurrentInput.x;
            m_DesiredVelocityDirection *= m_AccelerationCurve.Evaluate(m_AccelerationTime);

            ApplyGravity();

            m_CharacterController.Move(m_DesiredVelocityDirection.normalized * m_MaxVelocity * Time.deltaTime);
        }

        private void ApplyGravity()
        {
            Gravity();
            Jump();

            m_DesiredVelocityDirection += m_Gravity;
        }

        private void Gravity()
        {
            if (!m_CharacterController.isGrounded)
            {
                m_Gravity += Physics.gravity * Time.deltaTime;
            }
            else
            {
                m_Gravity = Vector3.zero;

                if (Input.GetKey(KeyCode.Space))
                {
                    m_IsJumping = true;
                }
            }
        }

        private void GetMovementInput()
        {
            m_MaxVelocity = m_IsCrouched ? m_CrouchVelocity : m_RunVelocity - m_Weapons[m_CurrentWeapon].Weight;
            m_CurrentInput = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;

            Crouch();
            Movement();
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
                m_JumpTime += Time.deltaTime;
                m_Gravity.y += m_JumpCurve.Evaluate(m_JumpTime) * m_JumpHeight;

                if (m_Gravity.y >= m_JumpHeight)
                {
                    m_IsJumping = false;
                    m_JumpTime = 0;
                }
            }   
        }
        #endregion Movement
    }
}