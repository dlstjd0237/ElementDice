using TMPro;
using UnityEngine;

public class TextMoney : MonoBehaviour
{
    private TMP_Text txt;
    private void Awake()
    {
        txt = GetComponent<TMP_Text>();
        CurrencyManagerScript.Instance.SetMoneyText(txt);
    }
}
