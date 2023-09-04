using UnityEngine;

namespace TrenchWarfare {
	public class HexMapCamera : MonoBehaviour {
		Camera mainCamera;

		float rotationAngle;

		float zoom = 0f;

		// [40:65] for the game mode, [40: 150 for the editor mode]
        public float minSize, maxSize;

        public float moveSpeedMinZoom, moveSpeedMaxZoom;

		public float rotationSpeed;

		public HexGrid grid;

        public bool Locked {
			set {
				enabled = !value;
			}
		}

		void Awake () {
			mainCamera = transform.GetChild(0).GetComponent<Camera>();

			setStartZoomAndPosition();
		}

		void Update () {
			float zoomDelta = Input.GetAxis("Mouse ScrollWheel");
			if (zoomDelta != 0f) {
				AdjustZoom(zoomDelta);
			}

			float rotationDelta = Input.GetAxis("Rotation");
			if (rotationDelta != 0f) {
				AdjustRotation(rotationDelta);
			}

			float xDelta = Input.GetAxis("Horizontal");
			float zDelta = Input.GetAxis("Vertical");
			if (xDelta != 0f || zDelta != 0f) {
				AdjustPosition(xDelta, zDelta);
			}
		}

		void AdjustZoom (float delta) {
			zoom = Mathf.Clamp01(zoom + delta);

			float size = Mathf.Lerp(minSize, maxSize, zoom);
			mainCamera.orthographicSize = size;
		}

		void AdjustPosition (float xDelta, float zDelta) {
			Vector3 direction = transform.localRotation * new Vector3(xDelta, 0f, zDelta).normalized;
			float damping = Mathf.Max(Mathf.Abs(xDelta), Mathf.Abs(zDelta));
			float distance =
				Mathf.Lerp(moveSpeedMinZoom, moveSpeedMaxZoom, zoom) *
				damping * Time.deltaTime;

			Vector3 position = transform.localPosition;
			position += direction * distance;
			transform.localPosition = ClampPosition(position);
		}

		Vector3 ClampPosition (Vector3 position) {
			float xMax = getMaxXPosition();
			position.x = Mathf.Clamp(position.x, 0f, xMax);

			float zMax = getMaxZPosition();
			position.z = Mathf.Clamp(position.z, 0f, zMax);

			return position;
		}


        private float getMaxXPosition() {
			return ((grid.CellCountX - 0.5f) * (2f * HexMetrics.innerRadius));
		}

		private float getMaxZPosition() {
			return (grid.CellCountZ - 1) * (1.5f * HexMetrics.outerRadius);
		}

		void AdjustRotation (float delta) {
			rotationAngle += delta * rotationSpeed * Time.deltaTime;
			if (rotationAngle < 0f) {
				rotationAngle += 360f;
			}
			else if (rotationAngle >= 360f) {
				rotationAngle -= 360f;
			}
			transform.localRotation = Quaternion.Euler(0f, rotationAngle, 0f);
		}

		public void setStartZoomAndPosition() {
			AdjustZoom(0);

			Vector3 startPosition = transform.localPosition;
			startPosition.x = getMaxXPosition() / 2f;
			startPosition.z = getMaxZPosition() / 2f;

			transform.localPosition = startPosition;
		}
    }
}