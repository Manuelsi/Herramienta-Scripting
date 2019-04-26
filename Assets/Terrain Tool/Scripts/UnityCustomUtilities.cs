using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// compile with: -doc:DocFileName.xml 
namespace CustomMSLibrary.Unity {
	
	public static class MiscUnityUtilities {
		#region Detectors
		#region 2D Detector
		/// <summary>
		/// Compact implementation of a "Line of Sight"/"Detection" algorithm in 2D.
		/// </summary>
		/// <param name="transform">Transform of the detector entity</param>
		/// <param name="detectionLayerMask">Layer mask of detectable targets</param>
		/// <param name="detectionRange">Detection radius from detector's pivot/centre</param>
		/// <param name="detectionFOV">Detection Fielf of View in degrees</param>
		/// <param name="detectionSightlineLayerMask">"Opaque" objects layer mask. Detector will ignore those excluded layers when determining visibility to target.
		/// Target must be included</param>
		/// <param name="target">First acquired target. Null if none found.</param>
		/// <param name="usesRight">When determining field of view, assign to 'true' if 'transform.right' is considered the front. Otherwise it will use 'transform.forward'.</param>
		/// <returns></returns>
		public static bool Detection2D(Transform transform, LayerMask detectionLayerMask, float detectionRange,
			float detectionFOV, LayerMask detectionSightlineLayerMask, out Transform target, bool usesRight = false) {

			Collider2D[] cols = Physics2D.OverlapCircleAll(transform.position, detectionRange, detectionLayerMask);
			if (cols != null && cols.Length > 0)
			{
				foreach (var item in cols)
				{
					Vector2 relativePos = item.transform.position - transform.position;
					if (Vector2.Angle((usesRight ? transform.right : transform.forward), relativePos) < detectionFOV * 0.5f)
					{
						RaycastHit2D hit = Physics2D.Raycast(transform.position, relativePos, detectionRange,
							detectionSightlineLayerMask);
						if (detectionLayerMask.ContainsLayer(hit.transform.gameObject.layer))
						{
							target = hit.transform;
							return true;
						}
					}
				}
			}
			target = null;
			return false;
		}
		#endregion
		#region 3D Detector
		/// <summary>
		/// Compact implementation of a "Line of Sight"/"Detection" algorithm in 3D.
		/// </summary>
		/// <param name="transform">Transform of the detector entity</param>
		/// <param name="detectionLayerMask">Layer mask of detectable targets</param>
		/// <param name="detectionRange">Detection radius from detector's pivot/centre</param>
		/// <param name="detectionFOV">Detection Fielf of View in degrees</param>
		/// <param name="detectionSightlineLayerMask">"Opaque" objects layer mask. Detector will ignore those excluded layers when determining visibility to target.
		/// Target must be included</param>
		/// <param name="target">First acquired target. Null if none found.</param>
		/// <param name="usesRight">When determining field of view, assign to 'true' if 'transform.right' is considered the front. Otherwise it will use 'transform.forward'.</param>
		/// <returns></returns>
		public static bool Detection3D(Transform transform, LayerMask detectionLayerMask, float detectionRange,
			float detectionFOV, LayerMask detectionSightlineLayerMask, out Transform target, bool usesRight = false) {

			Collider[] cols = Physics.OverlapSphere(transform.position, detectionRange, detectionLayerMask);
			if (cols != null && cols.Length > 0)
			{
				foreach (var item in cols)
				{
					Vector3 relativePos = item.transform.position - transform.position;
					if (Vector3.Angle((usesRight ? transform.right : transform.forward), relativePos) < detectionFOV * 0.5f)
					{
						RaycastHit hit;
						bool didHit = Physics.Raycast(transform.position, relativePos, out hit, detectionRange,
							detectionSightlineLayerMask);
						if (didHit && detectionLayerMask.ContainsLayer(hit.transform.gameObject.layer))
						{
							target = hit.transform;
							return true;
						}
					}
				}
			}
			target = null;
			return false;
		}
		public static bool Detection3D(Transform transform, LayerMask detectionLayerMask, float detectionRange,
			float detectionFOV, LayerMask detectionSightlineLayerMask, out Transform target, Vector3 deltaPos, bool usesRight = false) {

			Vector3 refPos = transform.position + (transform.rotation * deltaPos);
			Collider[] cols = Physics.OverlapSphere(refPos, detectionRange, detectionLayerMask);
			if (cols != null && cols.Length > 0)
			{
				foreach (var item in cols)
				{
					Vector3 relativePos = item.transform.position - transform.position;
					if (Vector3.Angle((usesRight ? transform.right : transform.forward), relativePos) < detectionFOV * 0.5f)
					{
						RaycastHit hit;
						bool didHit = Physics.Raycast(refPos, relativePos, out hit, detectionRange,
							detectionSightlineLayerMask);
						if (didHit && detectionLayerMask.ContainsLayer(hit.transform.gameObject.layer))
						{
							target = hit.transform;
							return true;
						}
					}
				}
			}
			target = null;
			return false;
		}
		#endregion
		#region 3D Detector All
		public static bool Detection3DAll(Transform transform, LayerMask detectionLayerMask, float detectionRange,
			float detectionFOV, LayerMask detectionSightlineLayerMask, out List<Transform> targets, Vector3 deltaPos, bool usesRight = false) {

			Vector3 refPos = transform.position + (transform.rotation * deltaPos);
			Collider[] cols = Physics.OverlapSphere(refPos, detectionRange, detectionLayerMask);
			bool found = false;
			targets = new List<Transform>();

			if (cols != null && cols.Length > 0)
			{
				foreach (var item in cols)
				{
					Vector3 relativePos = item.transform.position - transform.position;
					if (Vector3.Angle((usesRight ? transform.right : transform.forward), relativePos) < detectionFOV * 0.5f)
					{
						RaycastHit hit;

						bool didHit = Physics.Raycast(refPos, relativePos, out hit, detectionRange,
							detectionSightlineLayerMask);
						if (didHit && detectionLayerMask.ContainsLayer(hit.transform.gameObject.layer))
						{
							targets.Add(hit.transform);
							found = true;
						}
					}
				}
			}
			return found;
		}

		public static bool Detection3DAll(Transform transform, LayerMask detectionLayerMask, float detectionRange,
			float detectionFOV, LayerMask detectionSightlineLayerMask, out List<Transform> targets, bool usesRight = false) {

			return Detection3DAll(transform, detectionLayerMask, detectionRange, detectionFOV, detectionSightlineLayerMask, out targets, Vector3.zero, usesRight);
		}
		#endregion
		#endregion

		public static Quaternion Slerp2D(Vector2 a, Vector2 b, float t, bool usesRight = false) {

			float fa;
			float fb;

			if (!usesRight)
			{
				fa = Vector2.SignedAngle(Vector2.up, a);
				fb = Vector2.SignedAngle(Vector2.up, b);
			} else
			{
				fa = Vector2.SignedAngle(Vector2.right, a);
				fb = Vector2.SignedAngle(Vector2.right, b);
			}

			Quaternion qa = Quaternion.AngleAxis(fa, Vector3.forward);
			Quaternion qb = Quaternion.AngleAxis(fb, Vector3.forward);

			return Quaternion.Slerp(qa, qb, t);
		}

		public static Quaternion SlerpWithAxises(Vector3 a, Vector3 b, float t, Vector3 reference, Vector3 axis) {

			float fa;
			float fb;
			
			fa = Vector3.SignedAngle(reference, a, axis);
			fb = Vector3.SignedAngle(reference, b, axis);

			Quaternion qa = Quaternion.AngleAxis(fa, axis);
			Quaternion qb = Quaternion.AngleAxis(fb, axis);

			return Quaternion.Slerp(qa, qb, t);
		}

		public static Vector3 Vector3XY(Vector2 vec, float z) => new Vector3(vec.x, vec.y, z);

		public static Vector3 Vector3XZ(Vector2 vec, float y) => new Vector3(vec.x, y, vec.y);

		/// <summary>
		/// Returns true if the angle between vec1 and vec2 is smaller than the given angle.
		/// </summary>
		/// <param name="vec1">First vector</param>
		/// <param name="vec2">Second vector</param>
		/// <param name="comparisonAngle">Angle to compare against in degrees.</param>
		/// <returns></returns>
		public static bool AngleBetweenIsSmaller(Vector3 vec1, Vector3 vec2, float comparisonAngle)
			=> Vector3.Angle(vec1, vec2) < comparisonAngle;

		/// <summary>
		/// Returns Cosine of angle. Listen, it may be easy, but sometimes you need a reminder
		/// </summary>
		public static float DotValueForNormalizedAngles(float degrees) => Mathf.Cos(degrees);

	}

	/// <summary cref="C{T}">
	/// </summary>
	public class AutoRotator2D {
		private float turningSpeed;
		private Transform transform;
		private Vector2 v1;
		private Vector2 v2;

		private float rotationPercent;
		private bool hasFinished;

		#region Properties
		public float RotationPercent { get { return rotationPercent; } }
		public bool HasFinished { get { return hasFinished; } }
		#endregion Properties

		///<summary>Initializing constructor</summary>
		public AutoRotator2D(Transform transform, float turningSpeed) {
			this.transform = transform;
			this.turningSpeed = turningSpeed;
		}

		///<summary>Use Builder methods if using empty constructor</summary>
		public AutoRotator2D() { }

		public void Start(Vector2 a, Vector2 b) {
			v1 = a;
			v2 = b;
			rotationPercent = 0;
			float deltaAngle = Vector2.SignedAngle(Vector2.right, v1) - Vector2.SignedAngle(Vector2.right, v2);
			if (deltaAngle == 0) rotationPercent = 0;
			hasFinished = false;
		}

		public void Update(float deltaTime) {
			if (hasFinished)
			{
				Debug.LogWarning("Autorotator on " + transform.name + " has already finished rotating." +
					" Add a condition to check on property AutoRotator2D.HasFinished.");
				return;
			}
			rotationPercent += ((turningSpeed != 0f) ? (deltaTime / turningSpeed) : 1);
			transform.rotation = MiscUnityUtilities.Slerp2D(v1, v2, rotationPercent, true);
			if (rotationPercent >= 1)
				hasFinished = true;
		}

		public void Stop() {
			hasFinished = true;
		}

		//Decorator
		public AutoRotator2D SetTransform(Transform transform) {
			this.transform = transform;
			return this;
		}

		public AutoRotator2D SetTurningSpeed(float speed) {
			turningSpeed = speed;
			return this;
		}

	}

	public static class CustomUnityExtensions {
		public static bool ContainsLayer(this LayerMask mask, int layer) {
			return mask == (mask | (1 << layer));
		}

		public static List<Transform> GetAllChildren(this Transform transform) {
			List<Transform> children = new List<Transform>();
			int c = transform.childCount;
			for (int i = 0; i < c; i++)
			{
				var child = transform.GetChild(i);
				if (child != null) children.Add(child);
			}
			return children;
		}

		public static LayerMask ToLayerMask(this int layer) {
			return (1 << layer);
		}

		/// <summary>
		/// Returns a vector from the element calling the method to the parameter target.
		/// </summary>
		/// <returns></returns>
		public static Vector3 RelativePosTo(this Component origin, Component target) {
			return (target.transform.position - origin.transform.position);
		}

		public static Vector2 RotateTowards2D(this Vector2 source, Vector2 target, float maxAngle = 180) {
			float deltaAngle = Vector2.SignedAngle(source, target);
			deltaAngle = Mathf.Abs(deltaAngle) > maxAngle ? maxAngle * Mathf.Sign(deltaAngle) : deltaAngle;
			return Quaternion.Euler(0, 0, deltaAngle) * source;
		}

		public static Vector3 FlipYZ(this Vector3 vector) {
			var z = vector.z;
			vector.z = vector.y;
			vector.y = z;
			return vector;
		}

		/// <summary>
		/// Converts a vector3 to a Vector2 using x and z values instead of x and y.
		/// </summary>
		public static Vector2 ToVector2Z(this Vector3 vector) => new Vector2(vector.x, vector.z);

		/// <summary>
		/// Returns the vector with its x value set to 0.
		/// </summary>
		public static Vector3 ZeroX(this Vector3 vector) {
			vector.x = 0;
			return vector;
		}

		/// <summary>
		/// Returns the vector with its y value set to 0.
		/// </summary>
		public static Vector3 ZeroY(this Vector3 vector) {
			vector.y = 0;
			return vector;
		}

		/// <summary>
		/// Returns the vector with its z value set to 0.
		/// </summary>
		public static Vector3 ZeroZ(this Vector3 vector) {
			vector.z = 0;
			return vector;
		}

		/// <summary>
		/// Returns the vector with its x value set to 1.
		/// </summary>
		public static Vector3 OneX(this Vector3 vector) {
			vector.x = 1;
			return vector;
		}

		/// <summary>
		/// Returns the vector with its y value set to 1.
		/// </summary>
		public static Vector3 OneY(this Vector3 vector) {
			vector.y = 1;
			return vector;
		}

		/// <summary>
		/// Returns the vector with its z value set to 1.
		/// </summary>
		public static Vector3 OneZ(this Vector3 vector) {
			vector.z = 1;
			return vector;
		}

	}
}
