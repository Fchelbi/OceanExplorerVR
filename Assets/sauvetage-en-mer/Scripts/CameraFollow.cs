using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public Vector3 offset = new Vector3(0f, 5f, -9f);
    public float smooth = 5f;

    void LateUpdate()
    {
        if (target == null) return;

        Vector3 desired = target.position + target.rotation * offset;
        transform.position = Vector3.Lerp(transform.position, desired, smooth * Time.deltaTime);
        transform.LookAt(target.position + Vector3.up * 1.5f);
    }
}