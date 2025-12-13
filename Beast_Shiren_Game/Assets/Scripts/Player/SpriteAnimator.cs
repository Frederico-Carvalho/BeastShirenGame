using UnityEngine;
public class SpriteAnimator : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    // Sprites
    [SerializeField] private Sprite[] idleSprites;
    [SerializeField] private Sprite[] walkSprites;
    [SerializeField] private Sprite[] attackSprites;
    //configuração
    [SerializeField] private float frameRate = 0.1f;

    private Sprite[] currentSprites;
    private int frame;
    private float timer;

    public void PlayIdle() { Play(idleSprites); }
    public void PlayWalk() { Play(walkSprites); }
    public void PlayAttack() { Play(attackSprites); }

    private void Play(Sprite[] sprites)
    {
        if (currentSprites == sprites) return;
        currentSprites = sprites;
        frame = 0;
        timer = 0f;
        if (currentSprites.Length > 0)
            spriteRenderer.sprite = currentSprites[0];
    }

    private void Update()
    {
        if (currentSprites.Length == 0) return;

        timer += Time.deltaTime;
        if (timer >= frameRate)
        {
            timer = 0f;
            frame++;
            if (frame >= currentSprites.Length) frame = 0;
            spriteRenderer.sprite = currentSprites[frame];
        }
    }
}
