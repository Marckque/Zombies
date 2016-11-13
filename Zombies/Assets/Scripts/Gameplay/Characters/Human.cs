using UnityEngine;

namespace HumansVersusZombies
{
    public class Human : PlayerControllable
    {
        [Header("Weapons"), SerializeField]
        private Weapon[] m_Weapons;
        private int m_CurrentWeapon;

        [Header("Zombie"), SerializeField]
        private Zombie m_Zombie;

        protected void Start()
        {
            InitialiseWeapons();
        }

        protected override void CheckInputs()
        {
            base.CheckInputs();

            ChangeWeaponChange();
            CheckShoot();
        }

        protected void CheckShoot()
        {
            if (Input.GetMouseButton(0))
            {
                m_Weapons[m_CurrentWeapon].CanShoot();
            }
        }

        protected void ChangeWeaponChange()
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
            }
        }

        protected void InitialiseWeapons()
        {
            if (m_Weapons.Length == 0)
            {
                Debug.LogError("A human should have a weapon."); // In the game at least.
            }
            else
            {
                m_CurrentWeapon = 0;

                foreach (Weapon weapon in m_Weapons)
                {
                    weapon.GetCurrentPlayer(this);
                }
            }
        }

        public void TurnToZombie(Vector3 a_SpawnPosition, bool a_MasterZombie, int a_BonusHealth)
        {
            PlayerCamera.transform.SetParent(m_Zombie.PlayerCameraRoot);
            PlayerCamera.transform.localPosition = Vector3.zero;

            gameObject.SetActive(false);
            m_Zombie.transform.position = a_SpawnPosition;
            m_Zombie.gameObject.SetActive(true);

            if (a_MasterZombie)
            {
                m_Zombie.HealthManager.AddHealth(a_BonusHealth);
            }
        }
    }
}