using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Chromosome : System.IComparable<Chromosome> {
	public Chromosome (){}

	public abstract void setRandomValue();
	public abstract float fitness();
	public abstract Chromosome crossing( Chromosome c );
	public abstract void mutation( int mutationPropabiblity );
	public abstract bool isBetterThan( Chromosome c );
	public abstract bool isFinalSolution();
	public abstract bool isValid();

	public abstract string toString ();

	#region IComparable implementation

	int System.IComparable<Chromosome>.CompareTo (Chromosome other)
	{
		float thisFit = this.fitness ();
		float otherFit = other.fitness ();
		return ( thisFit > otherFit ? 1 : ( thisFit < otherFit ? -1 : 0 ) );
	}

	#endregion
}