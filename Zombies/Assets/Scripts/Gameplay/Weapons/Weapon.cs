using UnityEngine;

namespace HumansVersusZombies
{
    public class Weapon : MonoBehaviour
    {
        [SerializeField]
        protected int m_Damage = 1;
        [SerializeField]
        protected float m_ShootDelay = 0.2f;
        private float m_ShootTime;

        private Human m_CurrentPlayer;

        protected void Update()
        {
            // Delay between each shots
            if (m_ShootTime > 0)
            {
                m_ShootTime -= Time.deltaTime;
            }
        }

        public void GetCurrentPlayer(Human a_Player)
        {
            m_CurrentPlayer = a_Player;
        }

        #region Shoot
        private void Shoot()
        {
            ShootOutcome();

            Ray ray = m_CurrentPlayer.PlayerCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            // TO DO: Remove; Debug purposes only.
            Debug.DrawRay(ray.origin, ray.direction * 50f, Color.red, m_ShootDelay);

            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
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

        private void ShootOutcome()
        {
            m_ShootTime = m_ShootDelay;
        }
        #endregion Shoot
    }
}