using System;

namespace AssemblyCSharp
{
	public class Timer
	{
		public float time;

		protected float currentTime;
		protected bool active;
	

		public Timer (float time) {
			this.time = time;
			Reset ();
		}

		protected bool isDone;
		public bool IsDone {
			get {
				return isDone;
			}
		}

		public void Reset() {
			this.currentTime = this.time;
			active = true;
			isDone = false;
		}

		public void Tick(float dt) {
			if (!active) {
				return;
			}
			this.currentTime -= dt;
			if (this.currentTime <= 0) {
				isDone = true;
				active = false;
			}
		}
	}
}

