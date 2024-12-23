using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IDS : SearchingAlgorithm
{
	public override void StartSearching(Node source, Node destination)
	{
		base.StartSearching(source, destination);

		StartCoroutine(Searching());
	}

	IEnumerator Searching()
	{
		int depthLimit = 0;

		while (true)
		{
			Debug.Log($"Searching with depth limit: {depthLimit}");

			// Reset all nodes for each iteration
			ResetNodes();

			// Perform Depth-Limited Search
			bool found = false;
			yield return StartCoroutine(DepthLimitedSearch(source, destination, depthLimit, (result) => found = result));

			if (found)
			{
				Debug.Log("Destination Found!");
				yield break;
			}

			// Increase depth limit for the next iteration
			depthLimit++;
		}
	}

	IEnumerator DepthLimitedSearch(Node currentNode, Node destination, int depthLimit, System.Action<bool> onComplete)
	{
		// If depth limit is reached, stop recursion
		if (depthLimit == 0)
		{
			onComplete(false);
			yield break;
		}

		// Visit the current node
		currentNode.Visit(visitedMaterial);

		// Wait after visiting the node
		yield return new WaitForSeconds(speedRate);

		// Check if the destination node is reached
		if (currentNode == destination)
		{
			onComplete(true);
			yield break;
		}

		// Explore neighbors
		foreach (var neighbour in currentNode.neighbourNodes)
		{
			if (!neighbour.isVisited)
			{
				// Recursive call for depth-limited search
				bool found = false;
				yield return StartCoroutine(DepthLimitedSearch(neighbour, destination, depthLimit - 1, (result) => found = result));

				if (found)
				{
					onComplete(true);
					yield break;
				}
			}
		}

		onComplete(false);
	}

	void ResetNodes()
	{
		// Reset the state of all nodes to allow re-searching
		foreach (var node in FindObjectsOfType<Node>())
		{
			node.isVisited = false;
			node.Setmaterial(node.PreviousMaterial);
		}
	}
}
