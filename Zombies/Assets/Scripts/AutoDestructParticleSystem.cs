using UnityEngine;

public class AutoDestructParticleSystem : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem m_ParticleSystem;

	protected void Start()
    {
        Destroy(gameObject, m_ParticleSystem.duration);
	}

}