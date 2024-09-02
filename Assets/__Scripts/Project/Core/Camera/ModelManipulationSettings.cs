using System;
using UnityEngine;

namespace __Scripts.Project.Core
{
	[Serializable]
	public class ModelManipulationSettings
	{
		public float BaseSpeed = 5f;

		public float CurrentSpeed;

		public float Acceleration = 5f;

		public float Deceleration = 5f;

		public bool InverseDirectionXAxis;

		public bool InverseDirectionYAxis;

		public bool UseLimits;

		public float MinValue;

		public float MaxValue = 100f;

		public Vector2 InverseValue => new Vector2(InverseDirectionXAxis ? (-1f) : 1f, InverseDirectionYAxis ? (-1f) : 1f);
	}
}
