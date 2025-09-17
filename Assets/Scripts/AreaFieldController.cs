using System.Collections.Generic;
using UnityEngine;

public class AreaFieldController : MonoBehaviour
{
    public delegate void OnPlayEnterRoom(Area area);
    public OnPlayEnterRoom onPlayerEnterRoomCallback;

    [SerializeField] private List<AreaField> listOfArea;

    private Area currentArea;

    private void OnEnable()
    {
        for (int i = 0; i < listOfArea.Count; i++)
            listOfArea[i].InitAreaField(OnPlayerEnterArea);

        currentArea = listOfArea[0];
    }

    public void InitializeAreaField(OnPlayEnterRoom onPlayerEnterRoomCallback)
    {
        this.onPlayerEnterRoomCallback = onPlayerEnterRoomCallback;
    }

    private void OnPlayerEnterArea(AreaField areaField)
    {
        onPlayerEnterRoomCallback?.Invoke(areaField);
        ChangeAreaField(areaField);
    }

    private void ChangeAreaField(AreaField newAreaField)
    {
        currentArea.ExitArea();
        currentArea = newAreaField;
        currentArea.EnterArea();
    }

    public void ExitAreaField()
    {
        currentArea.ExitArea();
    }

    public void ClearRoom()
    {
        currentArea.DestroyEnemies();
    }
}
