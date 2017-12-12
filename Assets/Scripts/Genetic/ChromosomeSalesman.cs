using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChromosomeSalesman : Chromosome {
	public ChromosomeSalesman():base(){
		// Create Random
	}

	#region implemented abstract members of Chromosome
	protected override float fitness ()
	{
		throw new System.NotImplementedException ();
	}
	public override Chromosome crossing (Chromosome c)
	{
		throw new System.NotImplementedException ();
	}
	public override void mutation ()
	{
		throw new System.NotImplementedException ();
	}
	#endregion
}
