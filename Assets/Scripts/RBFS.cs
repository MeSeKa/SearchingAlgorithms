using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RBFS : SearchingAlgorithm
{
	float maxCost = 15f;


	[SerializeField] protected Material openListMaterial;


	public override void StartSearching(Node source, Node destination)
	{
		base.StartSearching(source, destination);
		StartCoroutine(RecursiveBestFirstSearch(source, destination, maxCost));
	}

	IEnumerator RecursiveBestFirstSearch(Node current, Node destination, float f_limit)
	{
		// Şu anki düğümü ziyaret et
		current.Visit(visitedMaterial);
		yield return new WaitForSeconds(speedRate);

		// Hedefe ulaştıysak işlemi sonlandır
		if (current == destination)
		{
			print("Hedefe ulaşıldı!");
			yield break;
		}

		// Komşu düğümleri al ve f(n) değerine göre hesapla
		List<Node> successors = new List<Node>();
		foreach (Node neighbour in current.neighbourNodes)
		{
			if (!neighbour.isVisited)
			{
				float f_value = neighbour.cost + Heuristic(neighbour, destination); // f(n) = cost + h(n)
				neighbour.text.text = f_value.ToString("F1"); // Görselleştirme için düğüme yaz
				successors.Add(neighbour);
				neighbour.Setmaterial(openListMaterial);
			}
		}

		// Eğer komşu düğüm yoksa geri izleme yap
		if (successors.Count == 0)
		{
			Debug.Log("Daha fazla düğüm yok. Geri izleniyor...");
			yield break;
		}

		// f(n) değerine göre komşuları sırala
		successors.Sort((a, b) => (a.cost + Heuristic(a, destination)).CompareTo(b.cost + Heuristic(b, destination)));

		while (successors.Count > 0)
		{
			// En iyi (en düşük f(n) değerine sahip) düğümü seç
			Node best = successors[0];
			float best_f = best.cost + Heuristic(best, destination);

			// Eğer en iyi düğüm f_limit'i aşarsa geri izlenir
			if (best_f > f_limit)
			{
				Debug.Log("En iyi düğüm f_limit'i aşıyor. Geri izleniyor...");
				yield break;
			}

			// Alternatif sınır (ikinci en iyi düğüm) belirlenir
			float alternative = (successors.Count > 1) ? (successors[1].cost + Heuristic(successors[1], destination)) : float.MaxValue;

			// RBFS için recursive çağrı yapılır
			yield return RecursiveBestFirstSearch(best, destination, Mathf.Min(f_limit, alternative));
		}
	}

	private float Heuristic(Node node, Node destination)
	{
		// Heuristic fonksiyonu (Manhattan veya düz mesafe)
		return Vector3.Distance(node.transform.position, destination.transform.position);
	}

	public void SetLimit(string cost)
	{
		this.maxCost = int.Parse(cost);
	}
}
