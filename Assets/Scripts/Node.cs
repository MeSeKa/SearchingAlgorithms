using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Node : MonoBehaviour
{
	public List<Node> neighbourNodes = new List<Node>();
	public bool isVisited = false;
	public int cost = 0;
	public TextMeshPro text;

	private void Start()
	{
		text.text = cost.ToString();
	}

	Material previousMaterial;
	public void Visit(Material visitedMaterial)
	{
		isVisited = true;
		previousMaterial = GetComponent<Renderer>().material;
		GetComponent<Renderer>().sharedMaterial = visitedMaterial;
	}

	[ContextMenu("Fill neighbourlist")]
	public void FindNeighbourNodes()
	{
		Vector3 startPos = transform.position;

		Vector3[] positions = new Vector3[6];
		//SAĞ
		positions[0] = startPos + Vector3.right * 2;
		//SAĞ ÜST
		positions[1] = startPos + Vector3.right + Vector3.forward;
		//SOL ÜST
		positions[2] = startPos + Vector3.left + Vector3.forward;
		//SOL
		positions[3] = startPos + Vector3.left * 2;
		//SOL ALT
		positions[4] = startPos + Vector3.left + Vector3.back;
		//SAĞ ALT
		positions[5] = startPos + Vector3.right + Vector3.back;

		List<Node> nodes = new List<Node>();
		foreach (var pos in positions)
		{
			Ray ray = new Ray(pos, Vector3.up);
			if (Physics.Raycast(ray, out RaycastHit raycastHit))
			{
				nodes.Add(raycastHit.collider.GetComponent<Node>());
			}
		}
		neighbourNodes = nodes;
	}

	public void ResetMaterial()
	{
		GetComponent<Renderer>().sharedMaterial = previousMaterial;
	}
}
