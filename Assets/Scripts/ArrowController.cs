using UnityEngine;

public class ArrowController : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private bool reverse;
    [SerializeField] private Vector2 limitX, limitY;
    [SerializeField] private int arrowDMG;

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
        switch (collision.gameObject.tag)
        {
            case "Enemy":
                Enemy enemy = collision.gameObject.GetComponent<Enemy>();
                enemy.TakeDamage(arrowDMG);
                Destroy(gameObject);
                break;
            case "Box":
                Box box = collision.gameObject.GetComponent<Box>();
                box.TakeDamage(arrowDMG);
                Destroy(gameObject);
                break;
        }

        Destroy(gameObject);
    }
}
