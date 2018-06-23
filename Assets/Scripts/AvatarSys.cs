using System.Collections.Generic;
using UnityEngine;

public class AvatarSys : MonoBehaviour {

    public static AvatarSys _instance;

    private Transform girlSourceTrans;//资源model
    private GameObject girlTarget; //骨架物体，换装的人  
    private Dictionary<string, Dictionary<string, SkinnedMeshRenderer>> girlData = new Dictionary<string, Dictionary<string, SkinnedMeshRenderer>>(); 
    //小女孩所有的资源信息   //部位的名字，部位编号，部位对应的skm
    Transform[] girlHips; //小女孩骨骼信息
    private Dictionary<string, SkinnedMeshRenderer> girlSmr = new Dictionary<string, SkinnedMeshRenderer>();// 换装骨骼身上的skm信息
    //部位的名字，部位对应的skm
    private string[,] girlStr = new string[,] { {"eyes","1"},{"hair","1"},{"top","1"},{"pants","1"},{"shoes","1"},{"face","1"}};
    //初始化信息

    private Transform boySourceTrans;//资源model
    private GameObject boyTarget; //骨架物体，换装的人  
    private Dictionary<string, Dictionary<string, SkinnedMeshRenderer>> boyData = new Dictionary<string, Dictionary<string, SkinnedMeshRenderer>>();
    //小女孩所有的资源信息   //部位的名字，部位编号，部位对应的skm
    Transform[] boyHips; //小女孩骨骼信息
    private Dictionary<string, SkinnedMeshRenderer> boySmr = new Dictionary<string, SkinnedMeshRenderer>();// 换装骨骼身上的skm信息
    //部位的名字，部位对应的skm
    private string[,] boyStr = new string[,] { { "eyes", "1" }, { "hair", "1" }, { "top", "1" }, { "pants", "1" }, { "shoes", "1" }, { "face", "1" } };
    //初始化信息

    public  int nowCount = 0; // 0代表小女孩，1 男孩
    public GameObject girlPanel;
    public GameObject boyPanel;

    void Awake() {
        _instance = this;
        DontDestroyOnLoad(this); //不删除游戏物体
    }

    void Start() {

        GirlAvatar();
        BoyAvatar();
        boyTarget.AddComponent<SpinWithMouse>();
        girlTarget.AddComponent<SpinWithMouse>();
        boyTarget.SetActive(false);


        
    }
   public  void GirlAvatar() {
        InstantiateGirl();
        SaveData(girlSourceTrans,girlData,girlTarget,girlSmr);
        InitAvatarGirl();
    }

   public  void BoyAvatar() { 
        InstantiateBoy();
        SaveData(boySourceTrans,boyData,boyTarget,boySmr);
        InitAvatarBoy();
       
    }
    void InstantiateGirl() {
        GameObject go = Instantiate(Resources.Load("FemaleModel")) as GameObject; //加载资源物体
        girlSourceTrans = go.transform;
        go.SetActive(false);
        girlTarget = Instantiate(Resources.Load("FemaleTarget")) as GameObject;        
        girlHips = girlTarget.GetComponentsInChildren<Transform>();
    }


    void InstantiateBoy()
    {
        GameObject go = Instantiate(Resources.Load("MaleModel")) as GameObject; //加载资源物体
        boySourceTrans = go.transform;
        go.SetActive(false);
        boyTarget = Instantiate(Resources.Load("MaleTarget")) as GameObject;
        boyHips = boyTarget.GetComponentsInChildren<Transform>();
    }


    void SaveData(Transform souceTrans,Dictionary<string, Dictionary<string, SkinnedMeshRenderer>> data,GameObject target,
        Dictionary<string, SkinnedMeshRenderer> smr) {

            data.Clear();
            smr.Clear();

        if (souceTrans == null)
            return;

        SkinnedMeshRenderer[] parts = souceTrans.GetComponentsInChildren<SkinnedMeshRenderer>();// 遍历所有子物体有SkinnedMeshRenderer，进行存储
        foreach (var part in parts) {
            string[] names = part.name.Split('-');
            if (!data.ContainsKey(names[0])) { //每次遍历到一个新的部位
                //骨骼下边生成对应的skm
                GameObject partGo = new GameObject();
                partGo.name = names[0];
                partGo.transform.parent = target.transform;

                smr.Add(names[0],partGo.AddComponent<SkinnedMeshRenderer>()); //把骨骼target身上的skm信息存储，部位只记录一次
                data.Add(names[0],new Dictionary<string,SkinnedMeshRenderer>());
            }
            data[names[0]].Add(names[1],part); //存储所有的skm信息到数据里边
        }

    }

    void ChangeMesh(string part, string num, Dictionary<string, Dictionary<string, SkinnedMeshRenderer>> data,
        Transform[] hips, Dictionary<string, SkinnedMeshRenderer> smr,string[,] str)
    { //传入部位，编号，从data里边拿取对应的skm 

        SkinnedMeshRenderer skm = data[part][num];//要更换的部位

        List<Transform> bones = new List<Transform>();
        foreach (var trans in skm.bones) { 
            foreach(var bone in hips){
                if (bone.name == trans.name) {
                    bones.Add(bone);
                    break;
                }
            }
        }
        //换装实现
        smr[part].bones = bones.ToArray();//绑定骨骼
        smr[part].materials = skm.materials;//替换材质
        smr[part].sharedMesh = skm.sharedMesh;//更换mesh

        SaveData(part,num,str);
    }

    void InitAvatarGirl() { //初始化骨架让他有mesh 材质 骨骼信息
        int length = girlStr.GetLength(0);//获得行数
        for (int i = 0; i < length; i++) {
            ChangeMesh(girlStr[i,0],girlStr[i,1],girlData,girlHips,girlSmr,girlStr); //穿上衣服
        }
    
    }

    void InitAvatarBoy()
    { //初始化骨架让他有mesh 材质 骨骼信息
        int length = girlStr.GetLength(0);//获得行数
        for (int i = 0; i < length; i++)
        {
            ChangeMesh(boyStr[i, 0], boyStr[i, 1], boyData, boyHips, boySmr,boyStr); //穿上衣服
        }

    }

    public void OnChangePeople(string part,string num){
        if (nowCount == 0)
        { //girl
            ChangeMesh(part, num, girlData, girlHips, girlSmr,girlStr);
        }
        else {
            ChangeMesh(part, num, boyData, boyHips, boySmr,boyStr);
        }
    }

    public void SexChange() { //性别转换，人物隐藏，面板隐藏
        if (nowCount == 0)
        {
            nowCount = 1;
            boyTarget.SetActive(true);
            girlTarget.SetActive(false);
            boyPanel.SetActive(true);
            girlPanel.SetActive(false);
        }
        else {
            nowCount = 0;
            boyTarget.SetActive(false);
            girlTarget.SetActive(true);
            boyPanel.SetActive(false);
            girlPanel.SetActive(true);
        }
    }

    void SaveData(string part,string num,string[,] str)  { //更改数据
        int length = str.GetLength(0);//获得行数
        for (int i = 0; i < length; i++)
        {
            if (str[i, 0] == part) {
                str[i, 1] = num;
            }
        }
    }

}
