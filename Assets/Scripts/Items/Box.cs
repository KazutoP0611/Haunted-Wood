using UnityEngine;

public class Box : MonoBehaviour, IDamagable
{
    [SerializeField] private int boxHP;
    [SerializeField] private GameObject dropItem;
    [SerializeField] private Vector3 dropItemOffset;
    [SerializeField] private AudioClip chestOpenSound;
    [SerializeField] private AudioClip chestDropSound;

    [Header("Random Drop Item Details")]
    [Tooltip("These values will matter if \"fixedBoxSpawnAmount\" in \"BoxRandomer\" component is false. Set this value between 0 to 1. If this is a king'room key's box, this number will have no effect.")]
    [SerializeField]private float healingItemDropChance = 0.3f;

    private Animator anim;
    private bool autoRandomToDrop = false;
    
    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    public void SetItemToDrop(GameObject dropItem)
    {
        this.dropItem = dropItem;
    }

    public void SetAutoRandomToDrop(bool autoRandomToDrop)
    {
        this.autoRandomToDrop = autoRandomToDrop;
    }

    public void OnTakeDamage(int damage)
    {
        if (boxHP > 0)
        {
            boxHP -= damage;

            anim.SetTrigger("TakeDamage");

            if (boxHP <= 0)
                anim.SetBool("Dead", true);
        }
    }

    public void DropItem()
    {
        if (dropItem == null)
            return;

        if (!autoRandomToDrop)
            SpawnDropObject();
        else
        {
            if (Random.value <= healingItemDropChance)
                SpawnDropObject();
        }
    }

    private void SpawnDropObject()
    {
        Instantiate(dropItem, transform.position + dropItemOffset, Quaternion.identity, transform.parent);
        AudioSource.PlayClipAtPoint(chestDropSound, transform.position);
    }

    private void PlayOpenChestSound()
    {
        AudioSource.PlayClipAtPoint(chestOpenSound, transform.position);
    }

    public void DestroyItem()
    {
        Destroy(gameObject);
    }
}
