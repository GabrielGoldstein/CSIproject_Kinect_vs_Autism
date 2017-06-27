﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;
using System.Xml.Linq;
using System;


public class EngagementMeter : MonoBehaviour {

	public GameObject leftArrow;
	public GameObject rightArrow;
	private Vector3 leftMeshPos;
	private Vector3 rightMeshPos;
    public GameObject engagement;
	private Vector3 p1shoulderpos;//extrenous remove later
	private Vector3 p1rightShoulderPos;
	private Vector3 p1leftShoulderPos;
	private Vector3 p2rightShoulderPos;
	private Vector3 p2leftShoulderPos;
	private Vector3 p2shoulderpos;
	private Quaternion p1rot;
	private Quaternion p1shoulderrot;
	private Quaternion p2shoulderrot;
	private Quaternion p2rot;
	private float p1shoulderToChin;
	private float p1shoulderToShoulder;
	private float p1TotalEngagement;
	private float p2shoulderToChin;
	private float p2shoulderToShoulder;
	private float p2TotalEngagement;
	private float leftPlayerEngagement;
	private float rightPlayerEngagement;
	private float combinedPlayerEngagement;
	private float leftPlayerEngagementsecondHalf;
	private float rightPlayerEngagementsecondHalf;
	private float combinedPlayerEngagementsecondHalf;

	private int updateCounter;
	private InteractionManager player1;
	private InteractionManager player2;
	private FacetrackingManager faceManagerP1;
	private FacetrackingManager faceManagerP2;
	private KinectManager _KinectManager;

	private long leftid;
	private long rightid;


	private PecCard _PecCard;
	public GameObject face1;
	public GameObject face2;
	private Renderer faceMat1;
	private Renderer faceMat2;
	private Vector3[] avModelVertices1 = null;
	private Vector3[] avModelVertices2 = null;


	private bool once = true;
	private float timeLeft;
	private Animator[] stars;

	//int negativeCounter;
	//int positiveCounter;
	//int notTrNegativeCounter;
	//int notTrPositiveCounter;
	//float delay = 3000f;



	void Awake(){
        //.playerIndex == 0) ? 
        _PecCard =GameObject.FindGameObjectWithTag("MainCamera").GetComponent<PecCard>();
		_KinectManager=GameObject.FindGameObjectWithTag("MainCamera").GetComponent<KinectManager>();
		faceManagerP1=GameObject.FindGameObjectWithTag("MainCamera").GetComponents<FacetrackingManager>()[0];
		faceManagerP2=GameObject.FindGameObjectWithTag("MainCamera").GetComponents<FacetrackingManager>()[1];
		faceMat1=face1.GetComponent<Renderer>();//change to left and right
		faceMat2=face2.GetComponent<Renderer>();//change left and right
		stars=engagement.GetComponentsInChildren<Animator>();
		faceMat1.material.shader=Shader.Find("Specular");
		faceMat2.material.shader=Shader.Find("Specular");
		}
	// Use this for initialization
	void Start () {
		
		 leftPlayerEngagement=0;
		 rightPlayerEngagement=0;
		 combinedPlayerEngagement=0;
		 leftPlayerEngagementsecondHalf=0;
		 rightPlayerEngagementsecondHalf=0;
		 combinedPlayerEngagementsecondHalf=0;
		// updateCounter=0;
		}
	
	// Update is called once per frame
	void FixedUpdate () {
		/*
		updateCounter++;
		float timeRequired=30f;
		float iterationsRequired=timeRequired*50f;

		combinedPlayerEngagement/=50f;
		leftPlayerEngagement/=50f;
		rightPlayerEngagement/=50f;

		//Debug.Log("update counter"+updateCounter);
		if(updateCounter==iterationsRequired)
		{
			
			//Debug.Log("twoengagement "+combinedPlayerEngagement);
			//Debug.Log("leftPlayerEngagement "+leftPlayerEngagement);
			//Debug.Log("rightPlayerEngagement "+rightPlayerEngagement);
			sendToCsv();
			pauseGame();
		}


//		delay -= Time.deltaTime; 
//		if(delay < 0){
//			//Debug.Log(faceManagerP1.GetHeadRotation(false));
//
//			delay = 1f;
//		}

		//sendToCsv();

		//Debug.Log(_KinectManager.GetJointPosition(_KinectManager.GetUserIdByIndex(0),4));
		*/
		//lockArrow();
		leadArrow();
		measureEngagement();
		//Resources.UnloadUnusedAssets();
}

	public void assignMesh() 
	{
		Vector3 p1MeshPos=faceManagerP1.getMeshTransform();
		Vector3 p2MeshPos=faceManagerP2.getMeshTransform();

		if(_KinectManager.GetUsersCount()==1)
		{
			leftMeshPos=p1MeshPos;
		}

		else if(_KinectManager.GetUsersCount()>1)
		{
			if(p1MeshPos.x < p2MeshPos.x)
			{
				leftMeshPos=p1MeshPos;
				rightMeshPos=p2MeshPos;
			}
			else
			{
			leftMeshPos=p2MeshPos;
			rightMeshPos=p1MeshPos;
			}
		}
	}

	public void leadArrow()
	{
		assignMesh();
		leftArrow.transform.position=new Vector3(leftMeshPos.x+.2f,leftMeshPos.y+.2f,leftMeshPos.z);
	
	}

	public void lockArrow()
	{

	}



	public void sendToCsv()
	{


		//combinedPlayerEngagement/=50f;
		leftPlayerEngagement/=50f;
		rightPlayerEngagement/=50f;
		combinedPlayerEngagement/=50f;
		combinedPlayerEngagementsecondHalf/=50f;
		leftPlayerEngagementsecondHalf/=50f;
		rightPlayerEngagementsecondHalf/=50f;
		string currTime = System.DateTime.Now.ToString("h:mm:ss tt");
		var p1id=_KinectManager.GetUserIdByIndex(0);
		bool ran=false;
		StringBuilder csvcontent=new StringBuilder();
		string csvfile="C:\\Users\\user\\Desktop\\cloud.csv";
			if(!ran)
			{
				
			//csvcontent.AppendLine("LeftPlayerEngagement,RightPlayerEngagement,CombinedPlayerEngagement");
			csvcontent.AppendLine(leftPlayerEngagement.ToString()+","+ rightPlayerEngagement.ToString()+ ","+ combinedPlayerEngagement.ToString()+","+currTime);//System.DateTime.UtcNow.ToString("HH:mm dd MMMM, yyyy")
			csvcontent.AppendLine(leftPlayerEngagementsecondHalf.ToString()+","+ rightPlayerEngagementsecondHalf.ToString()+ ","+ combinedPlayerEngagementsecondHalf.ToString()+","+currTime);
			ran=true;
			File.AppendAllText(csvfile,csvcontent.ToString());
			}
		/*
		avModelVertices1=new Vector3[faceManagerP1.GetFaceModelVertexCount()];
		faceManagerP1.GetUserFaceVertices(p1id, ref avModelVertices1);


			avModelVertices1=new Vector3[faceManagerP1.GetFaceModelVertexCount()];
			faceManagerP1.GetUserFaceVertices(p1id, ref avModelVertices1);
		if(avModelVertices1.Length > 0){
			Debug.Log(avModelVertices1[18].x *100f + "  "+ avModelVertices1[18].z *100f);
		}		for(int i=0;i<faceManagerP1.GetFaceModelVertexCount();i++)
			{

			csvcontent.AppendLine(avModelVertices1[i].x.ToString()+","+ avModelVertices1[i].y.ToString()+"," + avModelVertices1[i].z.ToString());

				}
				*/

		}
	private void pauseGame()
	{
		Time.timeScale=0;

	}
	public void measureEngagement()
	{
        if (faceManagerP1.IsTrackingFace()&&faceManagerP2.IsTrackingFace()&&_KinectManager.IsUserDetected())
		{
			//process left player
			// allocate storage for the point cloud points in a vector
			//get the face point cloud from the FaceTrackingManager and fill up the vector
			//get the joint position for the left shoulder
			avModelVertices1=new Vector3[faceManagerP1.GetFaceModelVertexCount()];
			faceManagerP1.GetUserFaceVertices(leftid, ref avModelVertices1);
			p1shoulderpos=_KinectManager.GetJointPosition(leftid,4);
			//get the distance between the chin and the left shoulder
			p1shoulderToChin=Mathf.Abs((((avModelVertices1[4].x)-(p1shoulderpos.x))*(100f)));
			//get the joint position for the right shoulder
			p1rightShoulderPos=_KinectManager.GetJointPosition(leftid,8);
			//same as above (left)
			p1leftShoulderPos=_KinectManager.GetJointPosition(leftid,4);
			//difference of the .z positions for the shoulders (if body turns)
			p1shoulderToShoulder=(p1rightShoulderPos.z - p1leftShoulderPos.z)*20f;
			//head distance plus shoulder distance
			p1TotalEngagement=p1shoulderToChin + p1shoulderToShoulder;
			Debug.Log("p1TotalEngagement"+p1TotalEngagement);
			// process right player
			p2rightShoulderPos=_KinectManager.GetJointPosition(rightid,8);
			p2leftShoulderPos=_KinectManager.GetJointPosition(rightid,4);
			p2shoulderToShoulder=((p2leftShoulderPos.z)-(p2rightShoulderPos.z))*20f;
			//get the face point cloud from the FaceTrackingManager and fill up the vector
			avModelVertices2=new Vector3[faceManagerP2.GetFaceModelVertexCount()];
			faceManagerP2.GetUserFaceVertices(rightid, ref avModelVertices2);
			p2shoulderpos=_KinectManager.GetJointPosition(rightid,4);
			p2shoulderToChin=Mathf.Abs((((avModelVertices2[4].x)-(p2shoulderpos.x))*(100f)));
			p2TotalEngagement =(p2shoulderToChin)-(p2shoulderToShoulder);
			//Debug.Log("p2TotalEngagement"+p2TotalEngagement);
			/*
			p2rightShoulderPos=_KinectManager.GetJointPosition(rightid,8);
			p2leftShoulderPos=_KinectManager.GetJointPosition(rightid,4);
			p2shoulderToShoulder=((p2rightShoulderPos.z)-(p2leftShoulderPos.z))*20f;
			avModelVertices2=new Vector3[faceManagerP2.GetFaceModelVertexCount()];
			faceManagerP2.GetUserFaceVertices(rightid, ref avModelVertices2);
			p2shoulderpos=_KinectManager.GetJointPosition(rightid,4);
			p2shoulderToChin=(((avModelVertices2[4].x)-(p2shoulderpos.x))*(100f));
			p2TotalEngagement =(p2shoulderToChin)-(p2shoulderToShoulder);
*/
			//Debug.Log("p1TotalEngagement"+p1TotalEngagement+"p2TotalEngagement"+p2TotalEngagement);


			//Debug.Log("leftshoulder.z=" +p1leftShoulderPos.z+"rightshoulder.z"+p1rightShoulderPos.z);
			//Debug.Log("leftshoulder-rightshoulder");
			//Debug.Log("p1leftshoulder.z=" +p1leftShoulderPos.z+"p1rightshoulder.z"+p1rightShoulderPos.z);
			//Debug.Log("p2leftshoulder.z=" +p2leftShoulderPos.z+"p1rightshoulder.z"+p2rightShoulderPos.z);
			//Debug.Log("p1engagement= "+ ((p1shoulderToChin)+(p1shoulderToShoulder)));
			//Debug.Log("p2engagment="+((p2shoulderToChin)-(p2shoulderToShoulder)));
			//Debug.Log("leftshoulder-rightshoulder"+(rightShoulderPos.z-leftShoulderPos.z ));
			if(p1TotalEngagement>=23f&&p2TotalEngagement<=12f)
			{
				faceMat1.material.SetColor("_Color",Color.green);
				faceMat2.material.SetColor("_Color",Color.green);

				if(_PecCard.pecStarted)
				{
					combinedPlayerEngagementsecondHalf++;
				}
				else
				combinedPlayerEngagement++;
				//engagement.SetActive(true);
				//StartCoroutine(wait());
			}
			if(p1TotalEngagement>=25f && !(p1TotalEngagement>=25f&&p2TotalEngagement<=12f))
			{
				
				faceMat1.material.SetColor("_Color",Color.blue);
				if(_PecCard.pecStarted)
				{
					leftPlayerEngagementsecondHalf++;;
				}
				else
					leftPlayerEngagement++;

			}
			if(!(p1TotalEngagement>=25f)&&!(p1TotalEngagement>=25f&&p2TotalEngagement<=12f))
			{
				faceMat1.material.SetColor("_Color",Color.gray);

			}

			if(p2TotalEngagement<=12f && !(p1TotalEngagement>=25f&&p2TotalEngagement<=12f))
			{
				
				faceMat2.material.SetColor("_Color",Color.blue);
				if(_PecCard.pecStarted)
				{
					rightPlayerEngagementsecondHalf++;
				}
				else
				rightPlayerEngagement++;
			}
			if(!(p2TotalEngagement<=12f) && !(p1TotalEngagement>=25f&&p2TotalEngagement<=12f))
			{
				faceMat2.material.SetColor("_Color",Color.gray);
			}

			//return;
		}
		/*
		if(faceManagerP1.IsTrackingFace()&&_KinectManager.IsUserDetected())
			{
			//p1rot=faceManagerP1.GetHeadRotation(leftid,false);//shoulderight=8//shoulderleft=4

			avModelVertices1=new Vector3[faceManagerP1.GetFaceModelVertexCount()];
			 faceManagerP1.GetUserFaceVertices(leftid, ref avModelVertices1);
			 p1shoulderpos=_KinectManager.GetJointPosition(leftid,4);
			 p1shoulderToChin=(((avModelVertices1[4].x)-(p1shoulderpos.x))*(100f));
			 p1rightShoulderPos=_KinectManager.GetJointPosition(leftid,8);
			 p1leftShoulderPos=_KinectManager.GetJointPosition(leftid,4);
			 p1TotalEngagement=p1shoulderToChin+p1shoulderToShoulder;
			 p1shoulderToShoulder=(p1rightShoulderPos.z-p1leftShoulderPos.z)*20f;
			if((p1TotalEngagement>=25f))
				{
				faceMat1.material.SetColor("_Color",Color.blue);
				leftPlayerEngagement++;
				}
			else
				{
				faceMat1.material.SetColor("_Color",Color.gray);
				}
			}




		if(faceManagerP2.IsTrackingFace())
		{
			avModelVertices2=new Vector3[faceManagerP2.GetFaceModelVertexCount()];
			faceManagerP2.GetUserFaceVertices(leftid, ref avModelVertices2);
			p2shoulderpos=_KinectManager.GetJointPosition(rightid,4);
			p2shoulderToChin=(((avModelVertices1[4].x)-(p2shoulderpos.x))*(100f));
			p2rightShoulderPos=_KinectManager.GetJointPosition(rightid,8);
			p2leftShoulderPos=_KinectManager.GetJointPosition(rightid,4);
			//Debug.Log("leftshoulder.z=" +leftShoulderPos.z+"rightshoulder.z"+rightShoulderPos.z);
			//Debug.Log("leftshoulder-rightshoulder"+(rightShoulderPos.z-leftShoulderPos.z ));
		    p2shoulderToShoulder=(p2leftShoulderPos.z-p2rightShoulderPos.z)*20f;
			if(((p2shoulderToChin)-(p2shoulderToShoulder))<=12f)
			{
				faceMat2.material.SetColor("_Color",Color.blue);
				rightPlayerEngagement++;
			}
			else
			{
				faceMat2.material.SetColor("_Color",Color.gray);
			}
		
		}

		}

	IEnumerator wait() {
		
		//yield return new WaitForSeconds(3);

		engagement.SetActive(true);
		for(int i=0;i<5;i++)
		{
			stars[i].SetBool("isSpinning",true);
		}
		yield return new WaitForSeconds(3);	//Wait 3 Secs
		engagement.SetActive(false);
		//engagement.SetActive(false);
		
*/


	}

	
	}









			

	
	