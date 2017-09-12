using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonCamera : MonoBehaviour {
	public float XSensitivity = 3f;
    public float YSensitivity = 3f;
    public float MinimumX = -90F;
    public float MaximumX = 90F;
	public float MaximumY = 90f;
	public float MinimumY = -90f;
    public bool lockCursor = true;
	
	public PlayerController pc;

    private PlayerController m_c;
    private Camera cam;
    private bool m_cursorIsLocked = true;
	
	//rotation to apply
	Vector3 R = Vector3.zero;
	
	void Start()
	{
		SetCursorLock(true);
	}

    /// <summary>
    /// Where the camera is located. 
    /// </summary>
    public Vector3 cameraLocation;
	
	void LateUpdate()
	{
		
		R.x -= Input.GetAxis("Mouse Y") * XSensitivity;
		R.y += Input.GetAxis("Mouse X") * YSensitivity;
		
		R.x = Mathf.Clamp(R.x, MinimumX, MaximumX);
		R.y = Mathf.Clamp(R.y, MinimumY, MaximumY);
		
		transform.localEulerAngles = R;
		UpdateCursorLock();
		//LookRotation();
	}

    public void SetCursorLock(bool value){
        lockCursor = value;
        if(!lockCursor){//we force unlock the cursor if the user disable the cursor locking helper
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    public void UpdateCursorLock(){
        //if the user set "lockCursor" we check & properly lock the cursos
        if (lockCursor)
            InternalLockUpdate();
    }

    private void InternalLockUpdate(){
        if(Input.GetKeyUp(KeyCode.Escape)){
            m_cursorIsLocked = false;
        } else if(Input.GetMouseButtonUp(0)){
            m_cursorIsLocked = true;
        }
        if (m_cursorIsLocked){
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        } else if (!m_cursorIsLocked){
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}
