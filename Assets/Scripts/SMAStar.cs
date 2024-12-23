using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SMAStar : SearchingAlgorithm
{
	public TextMeshProUGUI textMeshProUGUI;
	public int memoryLimit = 10; // Bellek sınırı (örneğin: 50 düğüm)
	private List<Node> openList = new List<Node>(); // Açık düğümler listesi


	[SerializeField] protected Material removedMaterial;
	[SerializeField] protected Material openListMaterial;

	public override void StartSearching(Node source, Node destination)
	{
		base.StartSearching(source, destination);
		StartCoroutine(Searching(source, destination));
	}

	IEnumerator Searching(Node source, Node destination)
	{
		// Başlangıç düğümünü openList'e ekle
		openList.Add(source);

		while (openList.Count > 0)
		{
			foreach (Node node in openList) { node.Setmaterial(openListMaterial); }
			// En düşük f(n) değerine sahip düğümü seç
			Node current = GetLowestCostNode();

			// Düğümü ziyaret et
			current.Visit(visitedMaterial);
			yield return new WaitForSeconds(speedRate);

			// Hedefe ulaşıldıysa işlemi sonlandır
			if (current == destination)
			{
				Debug.Log("Hedefe ulaşıldı!");
				yield break;
			}

			// Komşuları genişlet
			foreach (Node neighbour in current.neighbourNodes)
			{
				if (!neighbour.isVisited)
				{
					// g(n) = maliyet, h(n) = tahmini mesafe
					neighbour.cost = current.cost + 1;
					float f_value = neighbour.cost + Heuristic(neighbour, destination);
					neighbour.text.text = f_value.ToString("F1"); // Görselleştirme
					openList.Add(neighbour);
					neighbour.Setmaterial(openListMaterial);
					PrintMemoryArray();
				}
			}

			// Bellek sınırını kontrol et
			while (openList.Count > memoryLimit)
			{
				Node nodeToRemove = GetHighestCostNode();
				openList.Remove(nodeToRemove);
				nodeToRemove.Visit(removedMaterial);
				PrintMemoryArray();
				Debug.Log($"Bellek sınırı aşıldı. {nodeToRemove.name} düğümü kaldırıldı.");
			}
		}

		Debug.Log("Hedefe ulaşılamadı.");
	}

	private Node GetLowestCostNode()
	{
		// En düşük f(n) değerine sahip düğümü bul
		Node lowest = openList[0];
		float lowestCost = lowest.cost + Heuristic(lowest, destination);

		foreach (Node node in openList)
		{
			float cost = node.cost + Heuristic(node, destination);
			if (cost < lowestCost)
			{
				lowest = node;
				lowestCost = cost;
			}
		}
		return lowest;
	}

	private Node GetHighestCostNode()
	{
		// En yüksek f(n) değerine sahip düğümü bul
		Node highest = openList[0];
		float highestCost = highest.cost + Heuristic(highest, destination);

		foreach (Node node in openList)
		{
			float cost = node.cost + Heuristic(node, destination);
			if (cost > highestCost)
			{
				highest = node;
				highestCost = cost;
			}
		}
		return highest;
	}

	private float Heuristic(Node node, Node destination)
	{
		// Heuristic fonksiyonu (örneğin düz mesafe)
		return Vector3.Distance(node.transform.position, destination.transform.position);
	}

	public void SetMemoryLimit(string limit)
	{
		memoryLimit = int.Parse(limit);
	}

	void PrintMemoryArray()
	{
		textMeshProUGUI.text = "Memory Limit:"+memoryLimit.ToString() + "\n";
        foreach (var item in openList)
        {
            textMeshProUGUI.text += item.name + "\n";
        }
	}
}
