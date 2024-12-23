using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BFS : SearchingAlgorithm
{
	public override void StartSearching(Node source, Node destination)
	{
		base.StartSearching(source, destination);

		StartCoroutine(Searching());
	}

	IEnumerator Searching()
	{
		Queue<Node> queue = new Queue<Node>();
		queue.Enqueue(source);

		while (queue.Count > 0)
		{
			Node node = queue.Dequeue();
			node.Visit(visitedMaterial);

			foreach (var neighbour in node.neighbourNodes)
			{
				if (!neighbour.isVisited)
				{
					yield return new WaitForSeconds(speedRate);
					neighbour.Visit(visitedMaterial);
					if (neighbour == destination)// Founded
					{
						yield break;
					}
					queue.Enqueue(neighbour);
				}
			}
		}
	}
}
