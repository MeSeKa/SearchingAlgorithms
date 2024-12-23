using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


public abstract class SearchingAlgorithm : MonoBehaviour
{
	[SerializeField] protected Material visitedMaterial;
	protected Node source;
	protected Node destination;
	protected float speedRate = .5f;

	public virtual void StartSearching(Node source, Node destination)
	{
		this.source = source;
		this.destination = destination;
	}

	public void SetRate(Single newRate)
	{
		speedRate = newRate;
	}
}

