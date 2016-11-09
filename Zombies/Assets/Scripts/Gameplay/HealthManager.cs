using UnityEngine;

namespace HumansVersusZombies
{
    public class HealthManager : MonoBehaviour
    {
        [SerializeField]
        private int m_BaseHealth = 10;

        public int CurrentHealth { get; set; }

        protected void Start()
        {
            CurrentHealth = m_BaseHealth;
        }

        public void DiminishHealth(int a_Damage)
        {
            CurrentHealth -= a_Damage;
            print("CurrentHealth: " + CurrentHealth);

            if (CurrentHealth <= 0)
            {
                KillSelf();
            }
        }

        public void KillSelf()
        {
            Destroy(gameObject);
        }
    }
}