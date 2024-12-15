using UnityEngine;
using UnityEngine.SceneManagement;

public class Test : MonoBehaviour
{
	[SerializeField] Transform source;
	[SerializeField] Transform destination;

	[SerializeField] SearchingAlgorithm[] algorithms;

	int index = 0;

	Node sourceNode;
	Node destinationNode;


	// Update is called once per frame
	void Update()
	{
		if (Input.GetMouseButtonDown(0))
		{
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			if (Physics.Raycast(ray, out RaycastHit hit))
			{
				sourceNode = hit.collider.GetComponent<Node>();
				source.transform.position = sourceNode.transform.position + Vector3.up;
			}
		}

		if (Input.GetMouseButtonDown(1))
		{
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			if (Physics.Raycast(ray, out RaycastHit hit))
			{
				destinationNode = hit.collider.GetComponent<Node>();
				destination.transform.position = destinationNode.transform.position + Vector3.up;
			}
		}
	}

	public void StartSearch()
	{
		if (sourceNode != null && destinationNode != null)
			algorithms[index].StartSearching(sourceNode, destinationNode);
	}

	public void SelectAlgorithm(int index)
	{
		this.index = index;
	}

	public void Restart()
	{
		SceneManager.LoadScene(0);
	}
}
