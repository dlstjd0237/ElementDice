using UnityEngine;

public class AgentHUD : MonoBehaviour
{
    [SerializeField] private Agent _owner;

    private void Awake()
    {
        if (_owner is Player)
        {
            TurnEventBus.Subscribe(TurnEnumType.PlayerAttack, ()=> gameObject.SetActive(false));
            TurnEventBus.Subscribe(TurnEnumType.PlayerTurnEnd, ()=> gameObject.SetActive(true));
            TurnEventBus.Subscribe(TurnEnumType.BattleFail, ()=> gameObject.SetActive(false));
        }
        else
        {
            TurnEventBus.Subscribe(TurnEnumType.EnemyAttack, ()=>
            {
                if(EnemySpawn.Instance.GetEnemy()!=_owner)
                    return;
                gameObject.SetActive(false);
            });
            TurnEventBus.Subscribe(TurnEnumType.EnemyTurnEnd, ()=>
            {
                if(EnemySpawn.Instance.GetEnemy()!=_owner)
                    return;
                gameObject.SetActive(true);
            });
        }

        transform.rotation = Quaternion.Euler(0, _owner.transform.rotation.y, 0);
    }
    
    private void OnDisable()
    {
        if (_owner is Player)
        {
            TurnEventBus.UnSubscribe(TurnEnumType.PlayerAttack, ()=> gameObject.SetActive(false));
            TurnEventBus.UnSubscribe(TurnEnumType.PlayerTurnEnd, ()=> gameObject.SetActive(true));
            TurnEventBus.UnSubscribe(TurnEnumType.BattleFail, ()=> gameObject.SetActive(false));
        }
        else
        {
            TurnEventBus.UnSubscribe(TurnEnumType.EnemyAttack, ()=>
            {
                if(EnemySpawn.Instance.GetEnemy()!=_owner)
                    return;
                gameObject.SetActive(false);
            });
            TurnEventBus.UnSubscribe(TurnEnumType.EnemyTurnEnd, ()=>
            {
                if(EnemySpawn.Instance.GetEnemy()!=_owner)
                    return;
                gameObject.SetActive(true);
            });
        }
    }
}
