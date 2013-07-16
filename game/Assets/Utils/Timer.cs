using UnityEngine;
using System.Collections;

namespace Utils {
	
	public class Timer {
		
		protected float time;
		protected float duration;
		protected bool stop;
		
		public Timer(float duration) {
			Reset(duration);
		}
		
		public void Update(float dt) {
			if (stop) return;
			time -= dt;
			time = Mathf.Max(time, 0);
		}
		
		public void Reset(float newDuration) {
			this.duration = newDuration;
			Reset();
		}
		
		public void Reset() {
			this.time = this.duration;
			this.stop = false;
		}
		
		public bool IsFinished() {
			if (time == 0 && !stop) {
				stop = true;
				return true;
			} else {
				return false;
			}
		}
	}
}