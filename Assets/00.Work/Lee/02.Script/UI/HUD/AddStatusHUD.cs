using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
struct StatusStruct
{
    public StatusType type;
    public Sprite image;
}

public class AddStatusHUD : MonoBehaviour
{
    [SerializeField] private List<StatusStruct> _structs;
    [SerializeField] private StatusHUD baseIcon;

    private Dictionary<StatusType, StatusHUD> _statusHuds = new Dictionary<StatusType, StatusHUD>();
    
    public void AddStatus(StatusType type, string text = null)
    {
        StatusStruct statusStruct = _structs.Find(x => x.type == type);
        
        StatusHUD statusHUD = Instantiate(baseIcon, transform);
        statusHUD.Init(statusStruct.image, text);
        _statusHuds.Add(type, statusHUD);
    }

    public void ChangeStatus(StatusType type, string text = null)
    {
        if(!_statusHuds.ContainsKey(type))
            return;
        _statusHuds[type].Change(text);
    }

    public void RemoveStatus(StatusType type)
    {
        if(!_statusHuds.ContainsKey(type))
            return;
        
        _statusHuds[type].Remove();
        _statusHuds.Remove(type);
    }
}