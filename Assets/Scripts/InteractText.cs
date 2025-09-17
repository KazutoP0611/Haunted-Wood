using UnityEngine;

public class InteractText : MonoBehaviour
{
    [SerializeField] private Vector3 uiOffset;

    private Player player;

    public void Initialize(Player player)
    {
        this.player = player;
    }

    private void LateUpdate()
    {
        if (player)
        {
            transform.position = Camera.main.WorldToScreenPoint(player.transform.position) + uiOffset;
        }
    }
}
