using UnityEngine;

[CreateAssetMenu(fileName = "NewAttack", menuName = "ScriptableObjects/Attacks")]
public class AttackDataSO : ScriptableObject
{
    [SerializeField] private int startup = 5;
    [SerializeField] private int active = 3;
    [SerializeField] private int recovery = 10;
    [SerializeField] private int damage = 5;

    public int Startup { get { return startup; } }
    public int Active { get { return active; } }
    public int Recovery { get { return recovery; } }
    public int Damage { get { return damage; } }
    public int TotalFrames { get { return startup + active + recovery; } }
}