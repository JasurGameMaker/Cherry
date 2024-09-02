using UnityEngine;

namespace __Scripts.Project.Core
{
	[RequireComponent(typeof(BoxCollider))]
	public class CameraBounds : MonoBehaviour
	{
		public BoxCollider Bounds => GetComponent<BoxCollider>();
	}
}
