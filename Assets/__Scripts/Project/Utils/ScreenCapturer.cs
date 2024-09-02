using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace __Scripts.Project.Utils
{
	public class ScreenCapturer : MonoBehaviour
	{
		private Dictionary<CanvasGroup, float> preCaptureSave = new Dictionary<CanvasGroup, float>();

		public void Capture(Action<Texture2D> callBack, params CanvasGroup[] ignoredObjects)
		{
			StartCoroutine(_Capture(callBack, ignoredObjects));
		}

		public IEnumerator _Capture(Action<Texture2D> callBack, params CanvasGroup[] ignoredObjects)
		{
			SaveAndResetAlpha(ignoredObjects);
			yield return new WaitForEndOfFrame();
			Texture2D texture2D = new Texture2D(Screen.width, Screen.height, TextureFormat.ARGB32, mipChain: false);
			texture2D.ReadPixels(new Rect(0f, 0f, Screen.width, Screen.height), 0, 0, recalculateMipMaps: false);
			texture2D.Apply();
			RestoreAlpha(ignoredObjects);
			callBack?.Invoke(texture2D);
		}

		private void SaveAndResetAlpha(params CanvasGroup[] ignoredObjects)
		{
			foreach (CanvasGroup canvasGroup in ignoredObjects)
			{
				preCaptureSave[canvasGroup] = canvasGroup.alpha;
				canvasGroup.alpha = 0f;
			}
		}

		private void RestoreAlpha(params CanvasGroup[] ignoredObjects)
		{
			foreach (CanvasGroup canvasGroup in ignoredObjects)
			{
				canvasGroup.alpha = preCaptureSave[canvasGroup];
			}
			preCaptureSave.Clear();
		}
	}
}
