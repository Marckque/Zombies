using UnityEngine;

namespace HumansVersusZombies
{
    public class HealthManager : MonoBehaviour
    {
        [SerializeField]
        private int m_BaseHealth = 10;

        public int CurrentHealth { get; set; }

        protected void Awake()
        {
            CurrentHealth = m_BaseHealth;
        }

        public void DiminishHealth(int a_Damage)
        {
            CurrentHealth -= a_Damage;

            if (CurrentHealth <= 0)
            {
                KillSelf();
            }
        }

        public void AddHealth(int a_Bonus)
        {
            CurrentHealth += a_Bonus;
        }

        public void KillSelf()
        {
            Destroy(gameObject);
        }
    }
}