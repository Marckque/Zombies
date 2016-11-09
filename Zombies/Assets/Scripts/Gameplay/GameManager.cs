using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace HumansVersusZombies
{
    public class GameManager : MonoBehaviour
    {
        [Header("Game parameters"), SerializeField]
        private float m_RoundTimer;
        [SerializeField]
        private float m_MasterZombieTimer;
        [Header("Players and related"), SerializeField]
        private PlayerControllable[] m_Players;
        [SerializeField]
        private Transform[] m_PlayerSpawns;

        private bool m_MasterZombieHasSpawned;
        private int m_CurrentMasterZombie;
        private float m_RoundTime;
        private float m_MasterZombieTime;

        protected void Start()
        {
            InitialiseRound();
        }

        private void InitialiseRound()
        {
            SetTimers();
            SetPlayersID();
            SetMasterZombie();
        }

        protected void Update()
        {
            MasterZombieUpdate();
            RoundTimerUpdate();
        }

        private void SetTimers()
        {
            m_RoundTime = m_RoundTimer;
            m_MasterZombieTime = m_MasterZombieTimer;
        }

        private void SetPlayersID()
        {
            for (int i = 0; i < m_Players.Length; i++)
            {
                m_Players[i].CurrentID = i;
            }
        }

        #region MasterZombie
        private void SetMasterZombie()
        {
            m_CurrentMasterZombie = Random.Range(0, m_Players.Length);
        }

        private void SpawnMasterZombie()
        {
            m_MasterZombieHasSpawned = true;
            print("Master zombie spawned ! It is J" + m_CurrentMasterZombie + ", be ready!");
        }

        private void MasterZombieUpdate()
        {
            if (!m_MasterZombieHasSpawned)
            {
                if (m_MasterZombieTime > 0f)
                {
                    m_MasterZombieTime -= Time.deltaTime;
                }
                else
                {
                    SpawnMasterZombie();
                }
            }
        }
        #endregion MasterZombie

        #region RoundsManagement
        private void RoundTimerUpdate()
        {
            if (m_RoundTime > 0f)
            {
                m_RoundTime -= Time.deltaTime;
            }
            else
            {
                EndRound();
            }
        }

        private void EndRound()
        {
            print("Round is over!");
        }
        #endregion RoundManagement
    }
}