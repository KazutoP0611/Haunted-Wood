using UnityEngine;

public class Area : MonoBehaviour
{
    public delegate void OnPlayerEnterArea(AreaField areaField);
    protected OnPlayerEnterArea onPlayerEnterAreaCallback;

    [SerializeField] protected Vector3 roomPosition;
    [SerializeField] protected float distanceFromRoomUntilEnter;
    [SerializeField] protected Transform enemyParent;

    public Transform GetEnemyParent { get { return enemyParent; } }

    protected bool roomIsActivated = false;

    public Vector3 GetRoomPosition { get { return roomPosition; } }

    protected Player player;

    public virtual void EnterArea()
    {
        
    }

    public virtual void Update()
    {
        
    }

    public virtual void ExitArea()
    {
        roomIsActivated = false;
        DestroyEnemies();
    }

    public void DestroyEnemies()
    {
        foreach (Transform enemyTransform in enemyParent)
            Destroy(enemyTransform.gameObject);
    }
}
