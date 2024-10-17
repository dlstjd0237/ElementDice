using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    [SerializeField] private Image inventorySlot;

    private DiceSO _item;
    public DiceSO Item //�������� �����ϴ� ������Ƽ(���� ������ ������, ������ ����)
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
        // Texture�� Texture2D�� ��ȯ
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
        // Texture2D�� Sprite�� ��ȯ
        return Sprite.Create(texture2D, new Rect(0, 0, texture2D.width, texture2D.height), new Vector2(0.5f, 0.5f));
    }

}
