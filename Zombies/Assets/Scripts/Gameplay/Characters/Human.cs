using UnityEngine;

namespace HumansVersusZombies
{
    public class Human : PlayerControllable
    {
        [Header("Zombie"), SerializeField]
        private Zombie m_Zombie;

        public void TurnToZombie(Vector3 a_SpawnPosition, bool a_MasterZombie, int a_BonusHealth)
        {
            PlayerCamera.transform.SetParent(m_Zombie.PlayerCameraRoot);

            // Makes sure the camera starts in the right position/rotation/location
            PlayerCamera.transform.localPosition = Vector3.zero;
            PlayerCameraRoot.transform.localPosition = Vector3.zero;
            PlayerCamera.transform.localRotation = Quaternion.identity;
            PlayerCameraRoot.transform.localRotation = Quaternion.identity;

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