public enum GameState
{
    MENU,
    GAME,
    SHOP,
    WAVETRANS,
    GAMEOVER,
    WEAPONCHOSE,
    COMPLETE
}
public enum Difficulty
{
    EASY,
    NORMAL,
    HARD
}
public enum Stats
{
    Attack,
    AttackSpeed,
    CritChance,
    CritDamage,
    MoveSpeed,
    MaxHp,
    Range,
    HpRecoveryRate,
    Armor,
    Luck,
    Dodge,
    LifeSteal
}

public static class Enums
{
    public static string FormatStatName(Stats stats)
    {
        string formated ="";
        string unformated = stats.ToString();

        for (int i = 0; i < unformated.Length; i++)
        {
            if (char.IsUpper(unformated[i]))
            {
                formated +=" ";
            }
            formated += unformated[i];
        }
        return formated;
    }
}