using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UCS : SearchingAlgorithm
{
	public override void StartSearching(Node source, Node destination)
	{
		base.StartSearching(source, destination);

		StartCoroutine(Searching());
	}

	IEnumerator Searching()
	{
		// Priority queue implemented using a List
		List<(int cost, Node node)> priorityQueue = new List<(int, Node)>();
		Dictionary<Node, int> cumulativeCosts = new Dictionary<Node, int>();

		// Initialize with the source node
		priorityQueue.Add((0, source));
		cumulativeCosts[source] = 0;

		while (priorityQueue.Count > 0)
		{
			// Sort the queue by cost and take the first element
			priorityQueue = priorityQueue.OrderBy(x => x.cost).ToList();
			var currentPair = priorityQueue[0];
			priorityQueue.RemoveAt(0);
			Node currentNode = currentPair.node;
			int currentCumulativeCost = currentPair.cost;

			// Visit the current node
			currentNode.Visit(visitedMaterial);

			// Wait after visiting the node
			yield return new WaitForSeconds(speedRate);

			// Check if the destination node is reached
			if (currentNode == destination)
			{
				Debug.Log("Destination Found!");
				yield break;
			}

			// Explore neighbors
			foreach (var neighbour in currentNode.neighbourNodes)
			{
				if (!neighbour.isVisited)
				{
					// Calculate the new cumulative cost
					int newCumulativeCost = currentCumulativeCost + neighbour.cost;

					// If the neighbor is not in the queue or the new cost is lower, update it
					if (!cumulativeCosts.ContainsKey(neighbour) || newCumulativeCost < cumulativeCosts[neighbour])
					{
						cumulativeCosts[neighbour] = newCumulativeCost;

						// Enqueue the neighbor
						priorityQueue.Add((newCumulativeCost, neighbour));
					}
				}
			}
		}

		Debug.Log("Destination Not Found!");
	}
}
