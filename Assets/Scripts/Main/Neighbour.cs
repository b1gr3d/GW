using System.Collections.Generic;

public interface Neighbour<N> {
	IEnumerable<N> Neighbours { get; }
}