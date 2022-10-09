using System;
using UnityEngine;

namespace FlappyBird
{
	public class Scorer : MonoBehaviour
	{
		public event Action<int> OnCurrentChanged = delegate { };
		public event Action<int> OnBestChanged = delegate { };

		private int _current = 0;
		private int _best = 0;

		public int current
		{
			get => _current;
			set {
				if(_current == value) { return; }

				_current = value;
				OnCurrentChanged(_current);
				if(_current > _best) { 
					_best = _current;
					OnBestChanged(_best);
				}
			}
		}

		public int best
		{
			get => _best;
			private set => _best = value;
		}

		public void ForceSet(int current, int best)
		{
			this.current = current;
			this.best = best;
		}
	}
}
