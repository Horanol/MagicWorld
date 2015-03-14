using UnityEngine;
using System.Collections;

public class PlayerControlller : MonoBehaviour {
    Animator animator;

	void Start () {
        animator = GetComponent<Animator>();
	}
	
	void Update () {
        if (Input.GetKeyDown(KeyCode.Q)) animator.CrossFade("attack01", 0.2f);
        if (Input.GetKeyDown(KeyCode.W)) animator.CrossFade("attack02", 0.2f);
        if (Input.GetKeyDown(KeyCode.E)) animator.CrossFade("attack03", 0.2f);
        if (Input.GetKeyDown(KeyCode.R)) animator.CrossFade("hit", 0.2f);

        if (Input.GetKeyDown(KeyCode.A)) animator.CrossFade("death", 0.2f);
        if (Input.GetKeyDown(KeyCode.S)) animator.CrossFade("idle", 0.2f);
        if (Input.GetKeyDown(KeyCode.D)) animator.CrossFade("jump", 0.2f);
        if (Input.GetKeyDown(KeyCode.F)) animator.CrossFade("jumping", 0.2f);

        if (Input.GetKeyDown(KeyCode.Z)) animator.CrossFade("kick", 0.2f);
        if (Input.GetKeyDown(KeyCode.X)) animator.CrossFade("roll", 0.2f);
        if (Input.GetKeyDown(KeyCode.C)) animator.CrossFade("run", 0.2f);
        if (Input.GetKeyDown(KeyCode.V)) animator.CrossFade("walk", 0.2f);
    }
}
