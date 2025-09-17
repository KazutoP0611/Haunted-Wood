using UnityEngine;

public class HealingBox : MonoBehaviour
{
    [SerializeField] private int healPoint;

    public int GetHealPoint()
    {
        return healPoint;
    }
}
