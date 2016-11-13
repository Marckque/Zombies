using UnityEngine;

namespace HumansVersusZombies
{
    [RequireComponent(typeof(BoxCollider))]
    public class Damageable : MonoBehaviour
    {
        [Header("Damage modifier"), SerializeField]
        private int m_DamageMultiplier = 1;

        public HealthManager HealthManager { get; set; }

        public void CalculateDamage(int a_Damage)
        {
            HealthManager.DiminishHealth(a_Damage * m_DamageMultiplier);
        }
    }
}