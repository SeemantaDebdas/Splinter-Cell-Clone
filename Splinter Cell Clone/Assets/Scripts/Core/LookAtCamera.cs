using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    private void LateUpdate()
    {
        Vector3 lookDir = Camera.main.transform.forward;
        lookDir.y = 0;

        transform.forward = lookDir;
    }
}
