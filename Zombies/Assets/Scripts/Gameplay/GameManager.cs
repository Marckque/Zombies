using UnityEngine;

namespace HumansVersusZombies
{
    public class GameManager : MonoBehaviour
    {
        [Header("Game parameters"), SerializeField]
        private float m_RoundTimer;
        [SerializeField]
        private float m_MasterZombieTimer;
        [SerializeField]
        private int m_MasterZombieBonusHealth;
        [Header("Players and related"), SerializeField]
        private PlayerControllable[] m_Players;
        [SerializeField]
        private Transform[] m_PlayerSpawns;

        private bool m_MasterZombieHasSpawned;
        private int m_CurrentMasterZombie;
        private float m_RoundTime;
        private float m_MasterZombieTime;

        private static GameManager m_GameManager;
        public static GameManager Instance { get { return m_GameManager; } }

        public Transform[] PlayerSpawns { get { return m_PlayerSpawns; } }

        protected void Start()
        {
            InitialiseSingleton(); 
            InitialiseRound();
        }

        private void InitialiseSingleton()
        {
            if (m_GameManager != null)
            {
                Debug.LogWarning("m_GameManager should be null.");
            }
            else
            {
                m_GameManager = this;
            }
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
                m_Players[i].CurrentManagerID = i;
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
            SpawnZombie(m_CurrentMasterZombie, true);
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

        #region Zombie
        private void SpawnZombie(int a_PlayerID, bool a_IsMaster)
        {
            Human player = m_Players[a_PlayerID] as Human;
            player.TurnToZombie(m_PlayerSpawns[a_PlayerID].position, a_IsMaster, m_MasterZombieBonusHealth);
        }
        #endregion Zombie

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