using UnityEngine;
using UnityEngine.SceneManagement;
public class AvatarButton : MonoBehaviour {

  
    public void OnValueChanged(bool isOn) {
        if (isOn) {
            if (gameObject.name == "boy" || gameObject.name == "girl") {
                AvatarSys._instance.SexChange();
                return;
            }
            string[] names = gameObject.name.Split('-');
            AvatarSys._instance.OnChangePeople(names[0],names[1]);
            switch (names[0]) { 
                case "pants":
                    PlayAnimation("item_pants");
                    break;
                case  "shoes":
                    PlayAnimation("item_boots");
                    break;
                case "top":
                    PlayAnimation("item_shirt");
                    break;
                default:
                    break;
            }
            }
    
    }
    public void PlayAnimation(string animName) { 
        //换装动画名称

        Animation anim = GameObject.FindWithTag("Player").GetComponent<Animation>();
        if (!anim.IsPlaying(animName)) {
            anim.Play(animName);
            anim.PlayQueued("idle1");
        }
    
    }

    public void LoadScenes() {

        SceneManager.LoadScene(1);
    }
}
