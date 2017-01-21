using System;
using UnityEngine;

namespace AssemblyCSharp
{
	public class AudioTimer : Timer
	{
		AudioSource component;
		Action<AudioSource, AudioTimer> callback;

		public AudioTimer (AudioSource component, float time, Action<AudioSource, AudioTimer> callback) : base(time)
		{
			this.component = component;
			this.callback = callback;
		}

		public AudioSource getAudioSource(){
			return component;
		}

		new public void Tick(float dt){
			base.Tick (dt);
			if (isDone) {
				callback(component, this);
			}
		}
	}
}

