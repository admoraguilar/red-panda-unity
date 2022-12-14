using UnityEngine;

namespace WaterToolkit
{
	public static class LayerMaskExtensions
	{
		public static bool Includes(this LayerMask mask, int layer) =>
			(mask.value & 1 << layer) > 0;
	}
}
