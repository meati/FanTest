using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public static class Ease
{
	public static float OutBack(float t)
	{
		float c1 = 1.70158f;
		float c3 = c1 + 1;
		return 1f + c3 * Mathf.Pow(t - 1, 3) + c1 * Mathf.Pow(t - 1, 2);
	}
}
