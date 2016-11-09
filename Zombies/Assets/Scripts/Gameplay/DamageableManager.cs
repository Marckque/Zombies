using UnityEngine;

namespace HumansVersusZombies
{
    public class DamageableManager : MonoBehaviour
    {
        [Header("Entity's health manager"), SerializeField]
        private HealthManager m_Health;
        [Header("Entity's damageables"), SerializeField]
        private Damageable[] m_Damageables;

        protected void Start()
        {
            foreach(Damageable damageable in m_Damageables)
            {
                damageable.DamageableHealth = m_Health;
            }
        }
    }
}