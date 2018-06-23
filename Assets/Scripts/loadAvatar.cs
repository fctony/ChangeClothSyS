using UnityEngine;

public class loadAvatar : MonoBehaviour {

	void Start () {
        if (AvatarSys._instance.nowCount == 0)
        {
            AvatarSys._instance.GirlAvatar();
        }
        else {
            AvatarSys._instance.BoyAvatar();
        }
	}
	
	void Update () {
		
	}
}
