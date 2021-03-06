﻿using UnityEngine;

namespace HumansVersusZombies
{
    public class Knife : Weapon
    {
        protected override void Shoot()
        {
            base.Shoot();

            Ray ray = new Ray(m_CurrentPlayer.PlayerCameraRoot.transform.position, m_CurrentPlayer.PlayerCameraRoot.transform.forward);
            RaycastHit hit;

            // TO DO: Remove; Debug purposes only.
            Debug.DrawRay(ray.origin, ray.direction * 10f, Color.cyan, m_ShootDelay);

            if (Physics.Raycast(ray, out hit, m_ShootRange, ~9))
            {
                Human human = hit.collider.GetComponent<Human>();

                if (human != null)
                {
                    human.TurnToZombie(GameManager.Instance.PlayerSpawns[human.CurrentManagerID].position, false, 0);
                }
            }
        }
    }
}