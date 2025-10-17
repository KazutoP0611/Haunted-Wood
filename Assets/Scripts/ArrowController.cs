using UnityEngine;

public class ArrowController : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private bool reverse;
    [SerializeField] private Vector2 limitX, limitY;
    [SerializeField] private int arrowDMG;
    [SerializeField] private AudioClip arrowShotSound;

    private void Start()
    {
        AudioSource.PlayClipAtPoint(arrowShotSound, transform.position);
    }

    private void Update()
    {
        transform.position += transform.up * (reverse ? -1 : 1) * speed * Time.deltaTime;

        if (transform.position.x < limitX.x ||
            transform.position.x > limitX.y ||
            transform.position.y < limitY.x ||
            transform.position.y > limitY.y)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        IDamagable hitObject = collision.gameObject.GetComponent<IDamagable>();
        hitObject?.OnTakeDamage(arrowDMG);

        Destroy(gameObject);
    }
}
