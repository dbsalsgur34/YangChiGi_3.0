using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Donkey : MonoBehaviour {

    public CameraControl camera;
    public Transform Parent;
    public Transform SP;

    private void Start()
    {
        Parent = this.gameObject.GetComponentsInParent<Transform>()[1];
        Parent.rotation = SP.rotation;
    }

    void DonkeyAction()
    {
        camera.IsSkillCutScene = true;
    }
}
