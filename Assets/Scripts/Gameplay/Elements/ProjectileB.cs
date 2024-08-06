using UnityEngine;

public class ProjectileB : MonoBehaviour
{
    [SerializeField] private float speed;
    private float direction;
    private bool hit;
    private float lifetime;

    private Animator anim;
    private BoxCollider2D boxCollider;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
    }
    private void Update()
    {
        if (hit) return;
        float movementSpeed = speed * Time.deltaTime * direction;
        transform.Translate(movementSpeed, 0, 0);

        lifetime += Time.deltaTime;
        if (lifetime > 5) gameObject.SetActive(false);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        hit = true;
        boxCollider.enabled = false;
        transform.localScale = new Vector3(Mathf.Sign(transform.localScale.x + 2), Mathf.Sign(transform.localScale.y + 3), transform.localScale.z);
        anim.SetTrigger("Explode");
        if (collision.gameObject.tag == "Brick")
        {
            if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "OnlineBuilder")
            {
                if (GameObject.Find("Scripts/PlayTestManager").GetComponent<PlayTestManager>().IsTestGameplay)
                    collision.gameObject.GetComponent<BreakingBrick>().Deactivate();
            }
            else
                collision.gameObject.GetComponent<BreakingBrick>().Break();
        }
    }
    public void SetDirection(float _direction, float speed)
    {
        if (speed < 20) speed = 20;
        this.speed = speed;
        lifetime = 0;
        transform.localScale = new Vector3(1, 1, 1);
        direction = _direction;
        gameObject.SetActive(true);
        hit = false;
        boxCollider.enabled = true;

        float localScaleX = transform.localScale.x;
        if (Mathf.Sign(localScaleX) != _direction)
            localScaleX = -localScaleX;

        transform.localScale = new Vector3(localScaleX, transform.localScale.y, transform.localScale.z);
    }
    private void Deactivate()
    {
        gameObject.SetActive(false);
    }
}