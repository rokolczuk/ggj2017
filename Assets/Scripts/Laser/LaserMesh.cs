using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserMesh : MonoBehaviour {

	public int tiles;
	public float width;
	public float amplitude;
	public float speed;
	public float frequency;
	public float offset;
	public Color color;

	Transform begin;
	Transform end;

	MaterialPropertyBlock material;
	MeshRenderer meshRenderer;

	Vector3[] p;
	MeshFilter mf;
	Mesh mesh;
	float halfWidth;
	Vector3 normal;
	int quads;
	int numVerts;
	int numTris;

	Vector3[] vertices;
	Vector3[] normals;
	Vector2[] uv;
	Vector4[] tanget;
	Vector3 anchor = new Vector3(0, 0, 10);
	int[] tri;

	public void SetBeginEnd(Transform begin, Transform end){
		this.begin = begin;
		this.end = end;
	}

	void Awake() {		
		InitPositions ();
		quads = p.Length - 1;
		numVerts = 2 + (quads * 2);
		numTris = 2 * quads;

		material = new MaterialPropertyBlock ();
		meshRenderer = GetComponent<MeshRenderer> ();
		mf = GetComponent<MeshFilter>();
		mesh = new Mesh ();
		mf.mesh = mesh;

		InitTangent ();
		InitVertices ();
		InitNormals ();
		InitUvs ();
		InitIndices ();

		mesh.vertices = vertices;
		mesh.triangles = tri;
		mesh.normals = normals;
		mesh.uv = uv;
		mesh.tangents = tanget;
	}
		
	void InitPositions () {
		p = new Vector3[tiles + 3];
	}

	void InitVertices() {
		vertices = new Vector3[numVerts];
	}

	void InitNormals() {
		normals = new Vector3[numVerts];

	}

	void InitTangent() {
		tanget = new Vector4[numVerts];
		for (int i = 0; i < numVerts; i+=2) {
			float v = i / (float)(numVerts-2);
			Debug.Log ("V:" + v);
			tanget [i] = new Vector4 (v, v, v, v);
			tanget [i+1] = new Vector4 (v, v, v, v);
		}
	}

	void InitUvs() {
		uv = new Vector2[numVerts];
		for (int i = 0; i < numVerts; i += 2) {
			float v = i / (float)(numVerts-2);

			if (i == 0) {
				uv [i] = new Vector2 (0, 0);
				uv [i + 1] = new Vector2 (1, 0);
			} else if (i < numVerts - 2) {
				uv [i] = new Vector2 (0, 0.5f);
				uv [i + 1] = new Vector2 (1, 0.5f);
			} else {
				uv [i] = new Vector2 (0, 1);
				uv [i + 1] = new Vector2 (1, 1);
			}
		}
	}

	void InitIndices() {
		tri = new int[numTris * 3];
		for (int i = 0; i < numTris * 3; i+=6) {
			int n = i / 6;

			tri [i] = 2 * n;
			tri [i+1] = 2 * n + 2;
			tri [i+2] = 2 * n + 1;

			tri [i+3] = 2 * n + 2;
			tri [i+4] = 2 * n + 3;
			tri [i+5] = 2 * n + 1;
		}
	}
	void UpdatePositions() {
		transform.position = anchor;
		var direction = (end.position - begin.position)/(float)(tiles);

		p [0] = begin.position - direction.normalized*halfWidth;

		for (int i = 0; i < tiles; ++i) {
			
			var pos = begin.position + direction*(float)i;
			p [i+1] = pos;
		}
		p [tiles+1] = end.position;
		p [tiles+2] = end.position + direction.normalized*halfWidth;
	}

	void UpdateMaterials() {
		material.SetFloat ("_Frequency", frequency);
		material.SetFloat ("_Amplitude", amplitude);
		material.SetFloat ("_Speed", speed);
		material.SetFloat ("_Offset", offset);
		material.SetColor ("_Color", color);
		meshRenderer.SetPropertyBlock (material);
	}

	void UpdateWidth() {
		halfWidth = width * 0.5f;
	}

	void LateUpdate() {

        if (begin == null)
            return;

        if (end == null)
            return;

		UpdateMaterials ();
		UpdatePositions ();
		UpdateWidth ();

		for (int i = 0; i < p.Length; ++i) {
			Vector3 prev = i == 0 ? p [0] - (p [1]- p[0]) : p [i - 1];
			Vector3 curr = p [i];
			Vector3 next = i == p.Length - 1 ? p [p.Length-1] + (p [p.Length-1] - p [p.Length-2]) : p [i + 1];

			GenerateVerts (i * 2, prev, curr, next);
		}

		mesh.vertices = vertices;
		mesh.normals = normals;
		mesh.tangents = tanget;
	}

	void GenerateVerts(int i, Vector3 prev, Vector3 curr, Vector3 next){

		float v = i / (float)(numVerts-2);
		float scale = Mathf.Sin (0.1f + v*Mathf.PI*0.9f);

		var prevDirection = curr - prev;
		var nextDirection = next - curr;

		var bisect = (prevDirection + nextDirection) / 2;
		normal.x = bisect.y;
		normal.y = -bisect.x;
		normal.Normalize ();

		normals [i] = normal;
		normals [i+1] = normal;

		tanget [i] = new Vector4 (v, scale, 1, 1);
		tanget [i+1] = new Vector4 (v, scale, 1, 1);
			
		normal *= halfWidth;

		vertices[i] = curr - normal;
		vertices[i+1] = curr + normal;
	}
}
