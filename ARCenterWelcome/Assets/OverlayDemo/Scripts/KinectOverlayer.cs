using UnityEngine;
using System.Collections;

public class KinectOverlayer : MonoBehaviour 
{
//	public Vector3 TopLeft;
//	public Vector3 TopRight;
//	public Vector3 BottomRight;
//	public Vector3 BottomLeft;

	//public GUITexture backgroundImage;
	public KinectWrapper.NuiSkeletonPositionIndex TrackedJoint = KinectWrapper.NuiSkeletonPositionIndex.HandRight;
	public GameObject OverlayObject;
	public float smoothFactor = 5f;
	
	public GUIText debugText;

	private float distanceToCamera = 10f;

	void Update() 
	{
		KinectManager manager = KinectManager.Instance;
		
		if(manager && manager.IsInitialized())
		{	
			int iJointIndex = (int)TrackedJoint;

			if(manager.IsUserDetected())
			{
				uint userId = manager.GetPlayer1ID();
				
				if(manager.IsJointTracked(userId, iJointIndex))
				{
					Vector3 posJoint = manager.GetRawSkeletonJointPos(userId, iJointIndex);

					if(posJoint != Vector3.zero)
					{
						//posJoint is in meters, so first convert to feet
						posJoint = posJoint * 3.28084f + new Vector3(0.0f,-15.0f/12.0f,69.0f/12.0f);

						// (0,0,0) for the kinect seems to actually be 1 meter in the Z direction...
						if(OverlayObject)
						{
							Vector3 vPosOverlay = Quaternion.AngleAxis(25,Vector3.left) * posJoint;
							vPosOverlay.z = -vPosOverlay.z;

							if(debugText)
							{
								debugText.guiText.text = vPosOverlay.ToString();
							}
							OverlayObject.transform.position = Vector3.Lerp(OverlayObject.transform.position, vPosOverlay, smoothFactor * Time.deltaTime);
						}
					}
				}
				
			}
			
		}
	}
}