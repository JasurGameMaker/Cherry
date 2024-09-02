using UnityEngine;

namespace __Scripts.Project.Utils
{
	public static class TransformExtensions
	{
		public static Bounds EncapsulateBounds(this Transform t)
		{
			Renderer[] componentsInChildren = t.GetComponentsInChildren<Renderer>();
			Bounds result;
			if (componentsInChildren != null && componentsInChildren.Length != 0)
			{
				result = componentsInChildren[0].bounds;
				for (int i = 1; i < componentsInChildren.Length; i++)
				{
					Renderer renderer = componentsInChildren[i];
					result.Encapsulate(renderer.bounds);
				}
			}
			else
			{
				result = default(Bounds);
			}
			return result;
		}

		public static string GetPath(this Transform goTransform, Transform root)
		{
			string text = goTransform.name;
			while (goTransform.parent != root && goTransform.parent != null)
			{
				goTransform = goTransform.parent;
				text = goTransform.name + "/" + text;
			}
			return text;
		}
	}
}
