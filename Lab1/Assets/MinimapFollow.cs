using UnityEngine;

public class MinimapFollow : MonoBehaviour
{
    [SerializeField] private Transform target; 
    [SerializeField] private float height = 30f;

    private void LateUpdate()
    {
        if (!target) return;
        Vector3 p = target.position;
        transform.position = new Vector3(p.x, p.y + height, p.z);
        transform.rotation = Quaternion.Euler(90f, 0f, 0f);
    }
}
