using System;

public class BaekPlayerManager : MonoSingleton<BaekPlayerManager>
{
    private Player _player;
    public Player Player { get => _player; set => _player = value; }

    private void Awake()
    {
        if(Instance != null && Instance != this)
            Destroy(gameObject);
    }

    public Player GetPlayer()
    {
        return _player; 
    }

    public void PlayerSet(Player player)
    {
        _player = player;
    }
}
