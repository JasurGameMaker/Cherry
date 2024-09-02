using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

namespace __Scripts.Project.Core.Model.Socket
{
	public class SocketVisualStateController : MonoBehaviour, IPointerUpHandler, IEventSystemHandler, IPointerDownHandler
	{
		private GameObject _socketInstance;

		private SocketVisualState _visualState;

		private Renderer[] _subMeshes;

		private static readonly int ColorProperty = Shader.PropertyToID("_BaseColor");

		private Material _socketMaterial;

		private Shader _shader;

		public void Init(Shader shader, Vector3 position, Quaternion rotation)
		{
			_socketInstance = Instantiate(base.gameObject, position, rotation, base.gameObject.transform.parent);
			ClearAllBehaviors();
			_socketInstance.transform.localScale = base.gameObject.transform.localScale;
			_subMeshes = _socketInstance.GetComponentsInChildren<Renderer>();
			_shader = shader;
			InitSocketMaterial();
			SwitchMaterialOnAllSubMeshes();
			_visualState = SocketVisualState.Hidden;
			SetVisibilityAllSubMeshes(isVisible: false);
		}

		private void ClearAllBehaviors()
		{
			foreach (Behaviour item in from b in _socketInstance.GetComponentsInChildren<Behaviour>()
				where b as SocketVisualStateController == null
				select b)
			{
				item.enabled = false;
				//DestroyImmediate(item);
			}
			foreach (Behaviour item2 in from b in _socketInstance.GetComponentsInChildren<Behaviour>()
				where b as SocketVisualStateController != null
				select b)
			{
				item2.enabled = false;
				//DestroyImmediate(item2);
			}
		}

		private void InitSocketMaterial()
		{
			_socketMaterial = new Material(_shader);
			_socketMaterial.SetColor(ColorProperty, Color.magenta);
		}

		public void SwitchSocketVisualState(SocketVisualState newVisualState)
		{
			if (newVisualState != _visualState)
			{
				if (_visualState == SocketVisualState.Hidden)
				{
					SetVisibilityAllSubMeshes(isVisible: true);
				}
				switch (newVisualState)
				{
					case SocketVisualState.Visible:
						_socketMaterial.SetColor(ColorProperty, Color.magenta);
						break;
					case SocketVisualState.Overlapping:
						_socketMaterial.SetColor(ColorProperty, Color.green);
						break;
					case SocketVisualState.Hidden:
						SetVisibilityAllSubMeshes(isVisible: false);
						break;
				}
				_visualState = newVisualState;
			}
		}

		private void DeleteCloneObject()
		{
			if (_socketInstance != null)
			{
				DestroyImmediate(_socketInstance);
			}
		}

		private void SetVisibilityAllSubMeshes(bool isVisible)
		{
			if (_socketInstance == null || _subMeshes == null)
			{
				return;
			}
			Renderer[] subMeshes = _subMeshes;
			foreach (Renderer renderer in subMeshes)
			{
				if (!(renderer == null))
				{
					renderer.enabled = isVisible;
				}
			}
		}

		private void SwitchMaterialOnAllSubMeshes()
		{
			if (_subMeshes == null || _socketMaterial == null)
			{
				return;
			}
			Renderer[] subMeshes = _subMeshes;
			foreach (Renderer renderer in subMeshes)
			{
				Material[] materials = renderer.materials;
				for (int j = 0; j < materials.Length; j++)
				{
					materials[j] = _socketMaterial;
				}
				renderer.materials = materials;
			}
		}

		public void OnPointerUp(PointerEventData eventData)
		{
			DeleteCloneObject();
		}

		public void OnPointerDown(PointerEventData eventData)
		{
		}
	}
}
