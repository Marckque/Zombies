using UnityEngine;
using UnityEngine.SceneManagement;

namespace HumansVersusZombies
{
    public class DebugFunctions : MonoBehaviour
    {
        protected void Update()
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                SceneManager.LoadScene("Base");
            }
        }
    }
}