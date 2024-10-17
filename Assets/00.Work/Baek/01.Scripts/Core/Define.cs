public static class Define
{
    public enum EAgentType
    {
        None,
        Player,
        Enemy
    }

    public enum ERewardType
    {
        Gold,
        Dice
    }

    public enum EAudioType
    {
        BGM,
        SFX
    }

    public enum EAudioName
    {
        //===게임 BGM 사운드===
        Title, 
        Menu,
        Shop,
        Jungle,
        Mountain,
        Desert,
        EndingCredits,
        PlayerFaild,
        //===UI 사운드===
        Click,
        Hover,
        Typing,
        //===플레이어 공격 사운드===
        Attack,
        PlayerHit,
        PlayerDie,
        Orb,
        //===주사위 공격 사운드===
        None,
        Burn,
        Slow,
        Frostbite,
        Paralysis,
        Addiction,
        Heal,
        Defense,
        //===적 사운드===
        EnemyHit,
        EnemyDie
    }

}
