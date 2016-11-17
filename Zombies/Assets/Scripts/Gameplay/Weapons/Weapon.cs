using UnityEngine;

namespace HumansVersusZombies
{
    public class Weapon : MonoBehaviour
    {
        [SerializeField]
        protected int m_Damage = 1;
        [SerializeField]
        protected float m_ShootDelay = 0.2f;
        [SerializeField]
        protected float m_ShootRange = 1f;
        private float m_ShootTime;

        protected PlayerControllable m_CurrentPlayer;

        protected void Update()
        {
            // Delay between each shots
            if (m_ShootTime > 0)
            {
                m_ShootTime -= Time.deltaTime;
            }
        }

        public void GetCurrentPlayer(PlayerControllable a_Player)
        {
            m_CurrentPlayer = a_Player;
        }

        #region Shoot
        protected virtual void Shoot()
        {
            ShootOutcome();

            Ray ray = new Ray(m_CurrentPlayer.PlayerCameraRoot.transform.position, m_CurrentPlayer.PlayerCameraRoot.transform.forward);
            RaycastHit hit;

            // TO DO: Remove; Debug purposes only.
            Debug.DrawRay(ray.origin, ray.direction * 10f, Color.green, m_ShootDelay);

            if (Physics.Raycast(ray, out hit, m_ShootRange, ~10))
            {
                Damageable damageable = hit.collider.GetComponent<Damageable>();

                if (damageable)
                {
                    damageable.CalculateDamage(m_Damage);
                }
            }
        }

        public void CanShoot()
        {
            if (m_ShootTime <= 0)
            {
                Shoot();
            }
        }

        protected void ShootOutcome()
        {
            m_ShootTime = m_ShootDelay;
        }
        #endregion Shoot
    }
}