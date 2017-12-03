using System;
using System.Collections.Generic;

namespace UnityEngine.XR.iOS
{
	public class UnityARHitTestExample : MonoBehaviour
	{
		public Transform m_HitTransform;
		public float findingSquareDist = 0.5f;

		public GameObject BlueFocusSquare;
		public GameObject FocusedSquare;
		public GameObject TapToText;
		public GameObject WholeLamp;

		public Material lampShade;


		bool HitTestWithResultType (ARPoint point, ARHitTestResultType resultTypes)
		{
			List<ARHitTestResult> hitResults = UnityARSessionNativeInterface.GetARSessionNativeInterface ().HitTest (point, resultTypes);
			if (hitResults.Count > 0) {
				BlueFocusSquare.SetActive (false);
				FocusedSquare.SetActive (false);
				Destroy (TapToText);

				lampShade.DisableKeyword ("_EMISSION");


				foreach (var hitResult in hitResults) {
					Debug.Log ("Got hit!");
					m_HitTransform.position = UnityARMatrixOps.GetPosition (hitResult.worldTransform);
					m_HitTransform.rotation = UnityARMatrixOps.GetRotation (hitResult.worldTransform);
					Debug.Log (string.Format ("x:{0:0.######} y:{1:0.######} z:{2:0.######}", m_HitTransform.position.x, m_HitTransform.position.y, m_HitTransform.position.z));
					return true;
				}
				m_HitTransform = null;
//				WholeLamp.transform.DetachChildren ();
				Debug.Log ("REMOVED FROM PARENT");
			}
			return false;
		}

		// Update is called once per frame
		void Update () {
			Vector3 center = new Vector3(Screen.width/2, Screen.height/2, findingSquareDist);
			if (Input.touchCount > 0 && m_HitTransform != null)
			{
				var touch = Input.GetTouch(0);
				if (touch.phase == TouchPhase.Began || touch.phase == TouchPhase.Moved)
				{
					var screenPosition = Camera.main.ScreenToViewportPoint(center);


					ARPoint point = new ARPoint {
						x = screenPosition.x,
						y = screenPosition.y
					};

					// prioritize reults types
					ARHitTestResultType[] resultTypes = {
						ARHitTestResultType.ARHitTestResultTypeExistingPlaneUsingExtent, 
						// if you want to use infinite planes use this:
						//ARHitTestResultType.ARHitTestResultTypeExistingPlane,
						//						ARHitTestResultType.ARHitTestResultTypeHorizontalPlane, 
						//						ARHitTestResultType.ARHitTestResultTypeFeaturePoint
					}; 

					foreach (ARHitTestResultType resultType in resultTypes)
					{
						if (HitTestWithResultType (point, resultType))
						{
							return;
						}
					}
				}
			}
		}


	}
}
