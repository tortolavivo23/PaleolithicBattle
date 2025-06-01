using UnityEngine;

public class FPSController : MonoBehaviour
{
    [SerializeField] Transform cameraHolder;
	[SerializeField] float mouseSensitivity = 1;

	float verticalLookRotation;
    float horizontalLookRotation;

    void Update()
    {
        horizontalLookRotation +=Input.GetAxisRaw("Mouse X") * mouseSensitivity;
        horizontalLookRotation = Mathf.Clamp(horizontalLookRotation, -55f, 55f);
        transform.localEulerAngles = new Vector3(0, horizontalLookRotation, 0);
		verticalLookRotation -= Input.GetAxisRaw("Mouse Y") * mouseSensitivity;
		verticalLookRotation = Mathf.Clamp(verticalLookRotation, -90f, 90f);
		cameraHolder.localEulerAngles = new Vector3(verticalLookRotation, 0, 0);
	}
}
