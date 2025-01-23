using UnityEngine;

public class OffsetTextureAnimate :MonoBehaviour
{
	public float scrollSpeedX = 0.015f;
	public float scrollSpeedY = 0.015f;
	public float scrollSpeedXMaterial2 = 0.015f;
	public float scrollSpeedYMaterial2 = 0.015f;
	public MeshRenderer meshRenderer;
	public bool applyToNormal = true;

	void Start()
	{
		if (meshRenderer == null) meshRenderer = gameObject.GetComponent<MeshRenderer>();
	}

	void Update()
	{
		float offsetX = Time.time * scrollSpeedX % 1;
		float offsetY = Time.time * scrollSpeedY % 1;
		float offset2X = Time.time * scrollSpeedXMaterial2 % 1;
		float offset2Y = Time.time * scrollSpeedYMaterial2 % 1;
		if(applyToNormal) meshRenderer.material.SetTextureOffset("_BumpMap", new Vector2(offsetX, offsetY));
		meshRenderer.material.SetTextureOffset("_MainTex", new Vector2(offsetX, offsetY));
		if (meshRenderer.materials.Length > 1)
		{
			if (applyToNormal) meshRenderer.materials[1].SetTextureOffset("_MainTex", new Vector2(offset2X, offset2Y));
			meshRenderer.materials[1].SetTextureOffset("_BumpMap", new Vector2(offset2X, offset2Y));
		}
	}
}