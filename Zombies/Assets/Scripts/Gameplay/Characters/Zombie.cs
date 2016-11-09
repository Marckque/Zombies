using UnityEngine;

namespace HumansVersusZombies
{
    public class Zombie : PlayerControllable
    {
        // Probably need a default weapon, always the same. Put it right in the prefab and it should work !

        protected override void CheckInputs()
        {
            base.CheckInputs();

            CheckCut();
        }

        protected void CheckCut()
        {
            if (Input.GetMouseButton(0))
            {
                // Cut !
            }
        }

    }
}