using UnityEngine;

public class Box : MonoBehaviour
{
    [SerializeField] private int boxHP;
    [SerializeField] private GameObject dropItem;
    [SerializeField] private Vector3 dropItemOffset;

    private Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    public void SetItemToDrop(GameObject dropItem)
    {
        this.dropItem = dropItem;
    }

    public void TakeDamage(int damage)
    {
        boxHP -= damage;

        anim.SetTrigger("TakeDamage");

        if (boxHP <= 0)
            anim.SetBool("Dead", true);
    }

    public void DropItem()
    {
        if (dropItem != null)
            Instantiate(dropItem, transform.position + dropItemOffset, Quaternion.identity, transform.parent);
    }

    public void DestroyItem()
    {
        Destroy(gameObject);
    }
}
