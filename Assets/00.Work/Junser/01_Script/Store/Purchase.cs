using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;
using DG.Tweening;

public class Purchase : MonoBehaviour
{
    private DiceSO _storeItemSet;
    public DiceSO StoreItem
    {
        get
        {
            return _storeItemSet;
        }
        set
        {
            if (value != null)
            {
                _storeItemSet = value;
                SetStore();
            }
        }
    }//진열할 아이템 받기


    [SerializeField]
    private Image _storeImage;

    [SerializeField]
    private TMP_Text _storeName, _storeTooltip,_storePrice;


    private void SetStore()//아이템 진열
    {
        if (StoreItem != null && StoreItem.Texture != null)
        {
            Texture2D texture2D = ConvertToTexture2D(StoreItem.Texture);
            _storeImage.sprite = ConvertTextureToSprite(texture2D);
            _storeName.text = StoreItem.DiceName;
            _storeTooltip.text = StoreItem.DiceDescript;
            _storePrice.text = StoreItem.Price.ToString();
        }
        else
        {
            Debug.LogWarning("StoreItem 또는 StoreItem.Texture가 null입니다. 아이템을 진열할 수 없습니다.");
            _storeImage.sprite = null; 

            _storeName.text = StoreItem.DiceName;
            _storeTooltip.text = StoreItem.DiceDescript;
            _storePrice.text = StoreItem.Price.ToString();
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

    public DiceSO PurchaseItem()//아이템 구매
    {
        return _storeItemSet;
    }
}
