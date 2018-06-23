using UnityEngine;

public class SpinWithMouse : MonoBehaviour {

    private bool isClick = false;
    private Vector3 nowPos;
    private Vector3 oldPos;
    public float length = 5;

    void OnMouseUp() { //鼠标抬起
        isClick = false;
    }

    void OnMouseDown() { //鼠标按下

        isClick = true;
    }

    void Update() {
        nowPos = Input.mousePosition;
        if (isClick) { //鼠标按下不松手
            Vector3 offset = nowPos - oldPos;
            if (Mathf.Abs(offset.x) > Mathf.Abs(offset.y) && Mathf.Abs(offset.x) > length) { //进行旋转
                transform.Rotate(Vector3.up,-offset.x);
            }
        }
        oldPos = Input.mousePosition;
    }
}
