using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Hitbox : MonoBehaviour
{
    [SerializeField] private int damage;
    public int Damage { get { return damage; } set { damage = value; } }

    private void OnTriggerEnter(Collider other)
    {
        FighterController enemy = other.GetComponent<FighterController>();
        if (enemy != null)
        {
            enemy.TakeHit(Damage);
        }
    }
}