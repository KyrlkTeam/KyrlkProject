using UnityEngine;

[RequireComponent(typeof(Camera))]
public class PlatformerCameraController : MonoBehaviour {
    protected Camera cam;
    protected Transform tForm;
    
    /// <summary>
    /// Camera will be locked on the target position
    /// </summary>
    public Transform target;

    public float lBorder;
    public float rBorder;
    /// <summary>
    /// Camera fixed y position
    /// </summary>

    void Start () {
        cam = GetComponent<Camera>();
        tForm = GetComponent<Transform>();
	}
	
    public Vector3 GetCameraPosition(Vector3 targetPos) {
        var camPos = tForm.position;
        float xOffset = cam.orthographicSize * cam.aspect / 2f;//Screen.width / 200f;
        //Debug.Log(Screen.width + " -> offset = " + xOffset);
        targetPos.x = Mathf.Clamp(targetPos.x, lBorder + xOffset, rBorder - xOffset);
        camPos.x = targetPos.x;
        return camPos;
    }

	void Update () {
        tForm.position = GetCameraPosition(target.position);
	}

    //void OnDrawGizmos() {
    //    Gizmos.color = Color.red;
    //    float xOffset = cam.orthographicSize * cam.aspect / 2f;
    //    Gizmos.DrawLine(new Vector3(lBorder + xOffset, -100f), new Vector3(lBorder + xOffset, 100f));
    //    Gizmos.DrawLine(new Vector3(rBorder - xOffset, -100f), new Vector3(rBorder - xOffset, 100f));
    //    Gizmos.DrawCube(tForm.position + new Vector3(0, 0, 10f), Vector3.one);
    //}
}
