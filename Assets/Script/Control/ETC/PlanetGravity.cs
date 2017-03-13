using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetGravity : MonoBehaviour {

    public GameObject planet;
    public float gravityScale;

    public Vector3 gravityVector;
    public Transform body;
    protected Rigidbody RB;

    void Start()
    {
        RB = this.GetComponent<Rigidbody>();
        RB.constraints = RigidbodyConstraints.FreezeRotation;
        RB.useGravity = false;
        body = this.GetComponent<Transform>();
        gravityScale = 9.8f;
        planet = GameObject.Find("Planet");
    }


    public void GravityofPlanet(float gs)
    {
        float gravity;
        gravityVector = (this.transform.position - planet.transform.position).normalized;
        if (Vector3.Distance(this.transform.position, planet.transform.position) > 30)
        {
            gravity = gs * 100;
        }
        else
        {
            gravity = gs;
        }
        RB.AddForce(gravityVector * -gravity);
    }

    public void RotationControl()
    {
        Quaternion targetrotation = Quaternion.FromToRotation(body.up, gravityVector) * body.rotation;
        body.rotation = Quaternion.Slerp(body.rotation, targetrotation, 50 * Time.deltaTime);
    }

    void Update()
    {
        GravityofPlanet(gravityScale);
        RotationControl();
    }

}
