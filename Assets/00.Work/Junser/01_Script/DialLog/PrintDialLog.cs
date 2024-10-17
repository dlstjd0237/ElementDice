using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static Define;

public class PrintDialLog : MonoBehaviour
{
    public event Action OnEndPrintText;
    [SerializeField]
    DialLogTextSO _dialLogSO;

    [SerializeField]
    TMP_Text _textShowBox;

    int indexer = 0;

    [SerializeField] 
    private float duration;

    private Coroutine printDuration;

    private void Start()
    {
        printText();
    }
    private void printText()
    {
        if (indexer < _dialLogSO.DialLog.Count)
        {
            printDuration = StartCoroutine(PrintDuration(_dialLogSO.DialLog[indexer]));
        }
    }
    private string SetString(string printingText)
    {
        if (printingText.Contains("  "))
        {
            printingText = printingText.Replace("  ", "\n");
        }
        return printingText;
    }

    public void NextText()//이벤트/클릭 연결 필요
    {
        if (printDuration != null)
        {
            StopCoroutine(printDuration);
            printDuration = null;
            _textShowBox.text = SetString(_dialLogSO.DialLog[indexer]);
            indexer++;
        }
        else
        {
            printText();
        }
        
        OnEndPrintText?.Invoke();
    }

    public void NextTutorialText()//이벤트/클릭 연결 필요
    {
        if (printDuration != null) return;
            printText();
        
        OnEndPrintText?.Invoke();
    }

    private IEnumerator PrintDuration(string rawText)
    {
        _textShowBox.text = null;
        string printingText = SetString(rawText);
        for (int i = 0; i < printingText.Length; i++)
        {
            yield return new WaitForSeconds(duration);
            _textShowBox.text += printingText[i];
            SoundManager.PlaySound(EAudioType.SFX, EAudioName.Typing, 0.2f);
        }
        printDuration = null;
        indexer++;
    }
}
