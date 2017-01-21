using System;
using UnityEngine;
using System.Collections.Generic;


public class EffectsManager: MonoBehaviour
{
	[SerializeField]
	private float timeToKillParticleSystem;

	private List<ParticleSystem> particlesToRemove = new List<ParticleSystem>();
	private List<float> timesParticlesDetached = new List<float>();

	private void Awake()
	{
		EventDispatcher.AddEventListener<ParticlesDetachedEvent>(OnParticlesDetached);
	}

	private void OnDestroy()
	{
		EventDispatcher.RemoveEventListener<ParticlesDetachedEvent>(OnParticlesDetached);
	}

	private void OnParticlesDetached(ParticlesDetachedEvent e)
	{
		particlesToRemove.Add(e.Particles);
		timesParticlesDetached.Add(Time.time);
	}

	private void Update()
	{
		for (int i = 0; i < particlesToRemove.Count; i++)
		{
			if (Time.time > timesParticlesDetached[i] + timeToKillParticleSystem)
			{
				GameObject particleSystemGameObject = particlesToRemove[i].gameObject;
				particlesToRemove.RemoveAt(i);
				timesParticlesDetached.RemoveAt(i);

				GameObject.Destroy(particleSystemGameObject);

				i--;
			}
		}
	}
}


