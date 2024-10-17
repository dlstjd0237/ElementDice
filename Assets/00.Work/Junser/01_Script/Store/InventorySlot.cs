using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    [SerializeField] private Image inventorySlot;

    private DiceSO _item;
    public DiceSO Item //아이템을 설정하는 프로퍼티(값이 있으면 불투명, 없으면 투명)
    {
    get { return _item; }
    set
        {
            _item = value;
            if (_item != null&& _item.Texture!=null) 
            {
                Texture2D texture2D = ConvertToTexture2D(_item.Texture);
                inventorySlot.sprite = ConvertTextureToSprite(texture2D);
                inventorySlot.color = new Color(1, 1, 1, 1);
            }
            else
            {
                inventorySlot.color = new Color(1, 1, 1, 0);
            }
        }
    }
    private Texture2D ConvertToTexture2D(Texture texture)
    {
        // Texture를 Texture2D로 변환
        Texture2D texture2D = new Texture2D(texture.width, texture.height, TextureFormat.RGBA32, false);
        RenderTexture currentRT = RenderTexture.active;

        RenderTexture renderTexture = new RenderTexture(texture.width, texture.height, 32);
        Graphics.Blit(texture, renderTexture);
        RenderTexture.active = renderTexture;

        texture2D.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
        texture2D.Apply();

        RenderTexture.active = currentRT;
        return texture2D;
    }
    private Sprite ConvertTextureToSprite(Texture2D texture2D)
    {
        // Texture2D를 Sprite로 변환
        return Sprite.Create(texture2D, new Rect(0, 0, texture2D.width, texture2D.height), new Vector2(0.5f, 0.5f));
    }

}
