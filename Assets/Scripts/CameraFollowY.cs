
using UnityEngine;

public class CameraFollowY : MonoBehaviour
{
    public Transform target;

    public float offsetY;
    private bool isFollow;
    private float fixedX;
    private float fixedZ;

    private void Start()
    {
        fixedX = transform.position.x;
        fixedZ = transform.position.z;
        isFollow = true;
    }

    private void LateUpdate()
    {
        if (!isFollow) return;

        transform.position = new Vector3(
            fixedX,
            target.position.y - offsetY,
            fixedZ
        );
    }

    public void SetFollow(bool follow)
    {
        isFollow = follow;
    }
}