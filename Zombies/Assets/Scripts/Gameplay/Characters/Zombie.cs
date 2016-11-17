using UnityEngine;

namespace HumansVersusZombies
{
    public class Zombie : PlayerControllable
    {
        [Header("Weapon"), SerializeField]
        private Weapon m_MeleeWeapon;

        protected void Start()
        {
            InitialiseWeapons();
        }

        protected override void CheckInputs()
        {
            base.CheckInputs();

            if (MainAction())
            {
                MeleeAttack();
            }
            
            if (SecondaryAction())
            {
                // Secondary action
            }        
        }

        protected void MeleeAttack()
        {
            m_MeleeWeapon.CanShoot();
        }

        // This function also is in human ---> Need to be placed properly, or refactored
        protected void InitialiseWeapons()
        {
           m_MeleeWeapon.GetCurrentPlayer(this);
        }
    }
}