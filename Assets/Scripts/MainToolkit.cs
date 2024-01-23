using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MainToolkit : MonoBehaviour
{
	public float rotspeed = 20f;
	public GameObject tableObject;
	public GameObject chairObject;
	public GameObject mugObject;

	public Slider redSlider;
	public Slider greenSlider;
	public Slider blueSlider;

	public TMP_InputField redInputField;
	public TMP_InputField greenInputField;
	public TMP_InputField blueInputField;

	public GameObject selectedObject;

	private bool onResetPosition;

	public float scaleSpeed = 0.25f;
    public float minScale = 0.1f;
    public float maxScale = 3.0f;

	void Start() {
		redSlider.onValueChanged.AddListener(delegate { OnRedSliderChange(); });
		greenSlider.onValueChanged.AddListener(delegate { OnGreenSliderChange(); });
		blueSlider.onValueChanged.AddListener(delegate { OnBlueSliderChange(); });
	}

	void OnRedSliderChange() {
		redInputField.text = redSlider.value.ToString();
		ChangeColorGameObject();
	}

	void OnGreenSliderChange() {
		greenInputField.text = greenSlider.value.ToString();
		ChangeColorGameObject();
	}

	void OnBlueSliderChange() {
		blueInputField.text = blueSlider.value.ToString();
		ChangeColorGameObject();
	}

	public void OnRedInputFiledChange(string change) {
		float colorNumber = float.Parse(change);

		if (colorNumber > 255) {
			colorNumber = 255;
		}

		if (colorNumber < 0) {
			colorNumber = 0;
		}

		redSlider.value = colorNumber;

		ChangeColorGameObject();
	}

	public void OnGreenInputFiledChange(string change) {
		float colorNumber = float.Parse(change);

		if (colorNumber > 255) {
			colorNumber = 255;
		}

		if (colorNumber < 0) {
			colorNumber = 0;
		}

		greenSlider.value = colorNumber;

		ChangeColorGameObject();
	}

	public void OnBlueInputFiledChange(string change) {
		float colorNumber = float.Parse(change);

		if (colorNumber > 255) {
			colorNumber = 255;
		}

		if (colorNumber < 0) {
			colorNumber = 0;
		}

		blueSlider.value = colorNumber;

		ChangeColorGameObject();
	}
	

	void OnMouseDrag() {
		float rotationX = Input.GetAxis("Mouse X") * rotspeed * Mathf.Deg2Rad;
		float rotationY = Input.GetAxis("Mouse Y") * rotspeed * Mathf.Deg2Rad;

		transform.Rotate( Vector3.up, -rotationX, Space.World);
		transform.Rotate( Vector3.right, rotationY, Space.World);
	}

	IEnumerator LerpRotation(Quaternion targetRotation, float duration) {
        Quaternion startRotation = transform.rotation;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            transform.rotation = Quaternion.Lerp(startRotation, targetRotation, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.rotation = targetRotation;
    }

	IEnumerator LerpScale(Vector3 targetScale, float duration) {
        Vector3 startScale = transform.localScale;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            transform.localScale = Vector3.Lerp(startScale, targetScale, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.localScale = targetScale;
    }

	IEnumerator LerpColor(Color targetColor, float duration) {
        Renderer objRenderer = selectedObject.GetComponent<Renderer>();
        Color startColor = objRenderer.material.color;

        float elapsed = 0f;

        while (elapsed < duration)
        {
            Color lerpedColor = Color.Lerp(startColor, targetColor, elapsed / duration);
			objRenderer.material.SetColor("_Color", lerpedColor);

            elapsed += Time.deltaTime;
            yield return null;
        }

        objRenderer.material.SetColor("_Color", targetColor);
    }

	public void ResetPosition() {
		// Reset Rotation, Scale and Color to Default
		StartCoroutine(LerpRotation(Quaternion.identity, 0.20f));
		StartCoroutine(LerpScale(new Vector3(2, 2, 2), 0.20f));
		StartCoroutine(LerpColor(Color.white, 0.20f));
		
		// Reset Value on GUI
		redInputField.text = "255";
		greenInputField.text = "255";
		blueInputField.text = "255";
		redSlider.value = 255;
		greenSlider.value = 255;
		blueSlider.value = 255;
 	}

	public void ClearChildObject() {
		if (transform.childCount > 0) {
			Transform firstChild = transform.GetChild(0);

			if (firstChild != null)
			{
				Destroy(firstChild.gameObject);
			}
		}
	}

	private void ChangeColorGameObject() {
		var objRenderer = selectedObject.GetComponent<Renderer>();

		// Convert to the 0-1 float range
        float redFloat = redSlider.value / 255.0f;
        float greenFloat = greenSlider.value / 255.0f;
        float blueFloat = blueSlider.value / 255.0f;

        // Create a Color object using the converted float values
        Color newColor = new Color(redFloat, greenFloat, blueFloat);

        objRenderer.material.SetColor("_Color", newColor);
	}

	public void HandleObjectSelector(int index) {
		ResetPosition();
		ClearChildObject();
		switch (index)
		{
			case 0:
				selectedObject = Instantiate(tableObject, transform);
				break;
			case 1:
				selectedObject = Instantiate(chairObject, transform);
				break;
			case 2:
				selectedObject = Instantiate(mugObject, transform);
				break;
			default:
				break;
		}

		ChangeColorGameObject();
	}

	void Update() {
		// Untuk Scale Object (aka zoom?)
		float scrollInput = Input.GetAxis("Mouse ScrollWheel");
        ScaleObject(scrollInput);
	}

	void ScaleObject(float scrollInput) {
        Vector3 newScale = transform.localScale + Vector3.one * scrollInput * scaleSpeed;

        newScale = new Vector3(
            Mathf.Clamp(newScale.x, minScale, maxScale),
            Mathf.Clamp(newScale.y, minScale, maxScale),
            Mathf.Clamp(newScale.z, minScale, maxScale)
        );

        transform.localScale = newScale;
    }
}
