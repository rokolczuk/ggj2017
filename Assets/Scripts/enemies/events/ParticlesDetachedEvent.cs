using System;
using UnityEngine;


public class ParticlesDetachedEvent
{
	public readonly ParticleSystem Particles;

	public ParticlesDetachedEvent(ParticleSystem particles)
	{
		this.Particles = particles;
	}
}


