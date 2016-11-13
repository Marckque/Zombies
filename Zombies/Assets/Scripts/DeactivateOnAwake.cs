using UnityEngine;

namespace HumansVersusZombies
{
    public class DeactivateOnAwake : MonoBehaviour
    {
        protected void Awake()
        {
            gameObject.SetActive(false);
        }
    }
}