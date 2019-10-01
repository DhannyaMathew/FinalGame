using UnityEngine;

public class StartPoint : MonoBehaviour
{
    public void ResetPlayer(Player player)
    {
        player.transform.position = transform.position;
    }
}