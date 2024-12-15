using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


public class SearchingAlgorithm : MonoBehaviour
{
	[SerializeField] protected Material visitedMaterial;
	protected Node source;
	protected Node destination;

	public virtual void StartSearching(Node source, Node destination)
	{
		this.source = source;
		this.destination = destination;
	}
}

