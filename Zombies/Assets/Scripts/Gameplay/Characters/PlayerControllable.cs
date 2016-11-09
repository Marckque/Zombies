using UnityEngine;

namespace HumansVersusZombies
{
    public class PlayerControllable : MonoBehaviour
    {
        [Header("Camera"), SerializeField]
        protected Camera m_Camera;

        public int CurrentID { get; set; }

        protected void Update()
        {
            CheckInputs();
        }

        #region Controls
        protected virtual void CheckInputs()
        {
            Movement();
        }

        protected void Movement()
        {
            // Move!
        }
        #endregion Controls
    }
}