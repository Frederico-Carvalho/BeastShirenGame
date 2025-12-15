using UnityEngine;


public class SpriteAnimator : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private float frameRate = 0.1f;


    [Header("Sprites")]
    [SerializeField] private Sprite[] idle;
    [SerializeField] private Sprite[] walk;
    [SerializeField] private Sprite[] jump;
    [SerializeField] private Sprite[] _light;
    [SerializeField] private Sprite[] heavy;
    [SerializeField] private Sprite[] jump_attack;
    [SerializeField] private Sprite[] walkForward;
    [SerializeField] private Sprite[] walkBackward;
    [SerializeField] private Sprite[] airDashForward;
    [SerializeField] private Sprite[] airDashBackward;

 



    private Sprite[] current;
    private int frame;
    private float timer;


    public void PlayIdle() { Play(idle); }
    public void PlayWalk() { Play(walk); } 
    public void PlayWalkForward() { Play(walkForward); }
    public void PlayWalkBackward() { Play(walkBackward); }
    public void PlayJump() { Play(jump); }   
    public void PlayAirDashForward() { Play(airDashForward); }
    public void PlayAirDashBackward() { Play(airDashBackward); }
    public void PlayLight() { Play(_light); }
    public void PlayHeavy() { Play(heavy); }
    public void PlayJumpAttack() { Play(jump_attack); }


    private void Play(Sprite[] sprites)
    {
        if (current == sprites) return;
        current = sprites;
        frame = 0;
        timer = 0f;
        spriteRenderer.sprite = current[0];
    }


    private void Update()
    {
        if (current.Length == 0) return;


        timer += Time.deltaTime;
        if (timer >= frameRate)
        {
            timer = 0f;
            frame++;
            if (frame >= current.Length) frame = 0;
            spriteRenderer.sprite = current[frame];
        }
    }
}
