using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class OffAxisPerspective : MonoBehaviour {
	
	public GameObject projectionScreen;
	public bool estimatedViewFrustum = true;
	// Use this for initialization
	void LateUpdate()
	{
		if (null != projectionScreen) {
			// Lower left corner
			Vector3 pa = projectionScreen.transform.TransformPoint(new Vector3(-5f, 0f, -5f));
			// Lower right corner
			Vector3 pb = projectionScreen.transform.TransformPoint(new Vector3(5f, 0f, -05f));
			// Upper left corner
			Vector3 pc = projectionScreen.transform.TransformPoint(new Vector3(-5f, 0f, 5f));
			// Eyes (camera) position
			Vector3 pe = transform.position;
			
			// near clipping plane
			float n = Camera.main.nearClipPlane;
			// far clipping plane
			float f = Camera.main.farClipPlane;

			Vector3 va = Vector3.one; // from pe to pa
			Vector3 vb = Vector3.one; // from pe to pb
			Vector3 vc = Vector3.one; // from pe to pc
			Vector3 vr = Vector3.one; // right acis of screen
			Vector3 vu = Vector3.one; // up axis of screen
			Vector3 vn = Vector3.one; // normal vector of screen
			
			float l = 0; // distance to left screen edge
			float r = 0; // distance to right screen edge
			float b = 0; // distance to bottom screen edge
			float t = 0; // distance to top screen edge
			float d = 0; // distance from eyes to screen
			
			vr = pb - pa;
			vu = pc - pa;
			vr.Normalize();
			vu.Normalize();
			//we need to minus sign because unity use left-handed coordinates system
			vn = -Vector3.Cross(vr,vu);
			vn.Normalize();
			
			va = pa - pe;
			vb = pb - pe;
			vc = pc - pe;

			Debug.DrawRay(pe, va,Color.red);
			Debug.DrawRay(pe, vb,Color.green);
			Debug.DrawRay(pe, vc,Color.yellow);
			
			d = -Vector3.Dot(va,vn);
			l = Vector3.Dot(vr, va) * n / d;
			r = Vector3.Dot(vr, vb) * n / d;
			b = Vector3.Dot(vu, va) * n / d;
			t = Vector3.Dot(vu, vc) * n / d;

			// projection matrix
			Matrix4x4 p = Matrix4x4.identity;
			p.m00 = 2.0f*n/(r-l);
			p.m01 = 0.0f;
			p.m02 = (r+l)/(r-l);
			p.m03 = 0.0f;
			
			p.m10 = 0.0f;
			p.m11 = 2.0f*n/(t-b);
			p.m12 = (t+b)/(t-b);
			p.m13 = 0.0f;
			
			p.m20 = 0.0f;
			p.m21 = 0.0f;
			p.m22 = (f+n)/(n-f);
			p.m23 = 2.0f*f*n/(n-f);
			
			p.m30 = 0.0f;
			p.m31 = 0.0f;
			p.m32 = -1.0f;
			p.m33 = 0.0f;
			
			//rotation matrix
			Matrix4x4 rm = Matrix4x4.identity;
			rm.m00 = vr.x;
			rm.m01 = vr.y;
			rm.m02 = vr.z;
			rm.m03 = 0.0f;
			
			rm.m10 = vu.x;
			rm.m11 = vu.y;
			rm.m12 = vu.z;
			rm.m13 = 0.0f;
			
			rm.m20 = vn.x;
			rm.m21 = vn.y;
			rm.m22 = vn.z;
			rm.m23 = 0.0f;
			
			rm.m30 = 0.0f;
			rm.m31 = 0.0f;
			rm.m32 = 0.0f;
			rm.m33 = 1.0f;
			
			// translation matrix
			Matrix4x4 tm = Matrix4x4.zero;
			tm.m00 = 1.0f;
			tm.m01 = 0.0f;
			tm.m02 = 0.0f;
			tm.m03 = -pe.x;
			
			tm.m10 = 0.0f;
			tm.m11 = 1.0f;
			tm.m12 = 0.0f;
			tm.m13 = -pe.y;
			
			tm.m20 = 0.0f;
			tm.m21 = 0.0f;
			tm.m22 = 1.0f;
			tm.m23 = -pe.z;
			
			tm.m30 = 0.0f;
			tm.m31 = 0.0f;
			tm.m32 = 0.0f;
			tm.m33 = 1.0f;
			
			//set matrices
			Camera.main.projectionMatrix = p;

			Camera.main.worldToCameraMatrix = rm * tm;
			// The original paper puts everything into the projection 
			// matrix (i.e. sets it to p * rm * tm and the other 
			// matrix to the identity), but this doesn't appear to 
			// work with Unity's shadow maps.
			
			if(estimatedViewFrustum)
			{
				Quaternion q = Quaternion.identity;
				//look at the center of the screen
				q.SetLookRotation((0.5f * (pb + pc) - pe), vu);
				Debug.DrawRay(pe, (0.5f * (pb + pc) - pe),Color.cyan);
				//apply rotation
				Camera.main.transform.rotation = q;
				
				//set fieldOfview to a conservative estimate
				if(Camera.main.aspect >= 1.0f)
				{
					Camera.main.fieldOfView = Mathf.Rad2Deg * Mathf.Atan(((pb-pa).magnitude + (pc-pa).magnitude) / va.magnitude);
				}
				else
				{
					// take the camera aspect into account to make the frustum wide enough
					Camera.main.fieldOfView = Mathf.Rad2Deg / Camera.main.aspect * Mathf.Atan(((pb-pa).magnitude + (pc-pa).magnitude) / va.magnitude);
				}
			}

		}
	}
	
}
