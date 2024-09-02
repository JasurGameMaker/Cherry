using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

namespace __Scripts.Project.Utils
{
	public class SpriteSwitcher : MonoBehaviour
	{
		public string[] Names;

		public AssetReferenceSprite[] Sprites;

		[SerializeField]
		private Image Image;

		public int ActiveSprite { get; private set; }

		public async UniTask SwitchSpriteByName(string locationName)
		{
			int num = -1;
			for (int i = 0; i < Names.Length; i++)
			{
				if (Names[i] == locationName)
				{
					num = i;
					break;
				}
			}
			if (num != -1)
			{
				ActiveSprite = num;
				Image image = Image;
				image.sprite = await Addressables.LoadAssetAsync<Sprite>(Sprites[num]);
			}
			else
			{
				Image.CrossFadeAlpha(0f, 0f, ignoreTimeScale: true);
			}
		}

		public async UniTask SwitchSpriteByIndex(int index)
		{
			if (index >= 0 && index < Sprites.Length)
			{
				Image image = Image;
				image.sprite = await Addressables.LoadAssetAsync<Sprite>(Sprites[index]);
				ActiveSprite = index;
			}
			else
			{
				Debug.Log("Sprite index = " + index + " is out of range!");
			}
		}

		private void OnDestroy()
		{
			AssetReferenceSprite[] sprites = Sprites;
			foreach (AssetReferenceSprite assetReferenceSprite in sprites)
			{
				if (assetReferenceSprite.IsValid() && assetReferenceSprite.IsDone)
				{
					assetReferenceSprite.ReleaseAsset();
				}
			}
		}
	}
}
