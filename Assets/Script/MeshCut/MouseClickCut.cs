using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum Angle
{
	Up,
	Forward,
	Right
}
public class MouseClickCut : MonoBehaviour
{
    public Angle angle;
	private VineGrowController growController;


	void Start(){
		growController = this.gameObject.GetComponent<VineGrowController>();
	}
    void Update(){

		if(Input.GetMouseButtonDown(0)){
			RaycastHit hit;

			if(Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit)){

				GameObject victim = hit.collider.gameObject;
				if(victim.tag == "cutable")
				{
                   
                    if(angle == Angle.Up)
					{
                        Cutter.Cut(victim, hit.point, Vector3.up);
						
                    } 
					else if (angle == Angle.Forward)
					{
						Cutter.Cut(victim, hit.point, Vector3.forward);
						
					}
					else if (angle == Angle.Right)
					{
						Cutter.Cut(victim, hit.point, Vector3.right);
						
					}
					growController.StartNextStage();
				}
			}

		}
	}
}
