uniform float _Curvature;
uniform float _CameraOffset;

float4 CurveVertex(float4 v)
{
	// Transform the vertex coordinates from model space into world space
	float4 vv = mul(unity_ObjectToWorld, v);

	// Now adjust the coordinates to be relative to the camera position

	vv.xyz -= _WorldSpaceCameraPos.xyz - _CameraOffset;

	// Reduce the y coordinate (i.e. lower the "height") of each vertex based
	// on the square of the distance from the camera in the z axis, multiplied
	// by the chosen curvature factor

	//float curvature = ((vv.z * vv.z) + (vv.x * vv.x)) * - _Curvature / 100.0;
	float curvature = (vv.z * vv.z) * -_Curvature / 100.0f;

	vv = float4(0.0f, curvature, 0.0f, 0.0f);

	// Now apply the offset back to the vertices in model space
	v += mul(unity_WorldToObject, vv);

	return v;
}