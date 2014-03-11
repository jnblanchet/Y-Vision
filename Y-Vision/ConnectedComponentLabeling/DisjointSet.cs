
namespace Y_Vision.ConnectedComponentLabeling
{
   /// <summary>
    /// A Union-Find/Disjoint-Set data structure.
    /// Source: http://www.mathblog.dk/files/2012/07/DisjointSet.cs
    /// Author:  Bjarki Ágúst
    /// Modified by: Jean-Nicola Blanchet (2014)
    /// </summary>
    public class DisjointSet {

        /// <summary>
        /// The number of elements in the universe.
        /// </summary>
        public int Count { get; private set; }

        /// <summary>
        /// The parent of each element in the universe.
        /// </summary>
        private readonly int[] _parent;

        /// <summary>
        /// The rank of each element in the universe.
        /// </summary>
        private readonly int[] _rank;

        /// <summary>
        /// The number of disjoint sets.
        /// </summary>
        public int SetCount { get; private set; }

        /// <summary>
        /// Initializes a new Disjoint-Set data structure, with the specified amount of elements in the universe.
        /// </summary>
        /// <param name='count'>
        /// The number of elements in the universe.
        /// </param>
        public DisjointSet(int count) {
            Count = count;
            SetCount = count;
            _parent = new int[Count];
            _rank = new int[Count];

            for (int i = 0; i < Count; i++) {
                _parent[i] = i;
                _rank[i] = 0;
            }
        }

        /// <summary>
        /// Find the parent of the specified element.
        /// </summary>
        /// <param name='i'>
        /// The specified element.
        /// </param>
        /// <remarks>
        /// All elements with the same parent are in the same set.
        /// </remarks>
        public int Find(int i) {
            if (_parent[i] == i) {
                return i;
            }
            // Recursively find the real parent of i, and then cache it for later lookups.
            _parent[i] = Find(_parent[i]);
            return _parent[i];
        }

        /// <summary>
        /// Unite the sets that the specified elements belong to.
        /// </summary>
        /// <param name='i'>
        /// The first element.
        /// </param>
        /// <param name='j'>
        /// The second element.
        /// </param>
        public void Union(int i, int j) {

            if (i == j)
                return;

            // Find the representatives (or the root nodes) for the set that includes i
            int irep = Find(i),
                // And do the same for the set that includes j
                jrep = Find(j),
                // Get the rank of i's tree
                irank = _rank[irep],
                // Get the rank of j's tree
                jrank = _rank[jrep];

            // Elements are in the same set, no need to unite anything.
            if (irep == jrep)
                return;

            SetCount--;

            // If i's rank is less than j's rank
            if (irank < jrank) {
         
                // Then move i under j
                _parent[irep] = jrep;

            } // Else if j's rank is less than i's rank
            else if (jrank < irank) {
         
                // Then move j under i
                _parent[jrep] = irep;

            } // Else if their ranks are the same
            else {
         
                // Then move i under j (doesn't matter which one goes where)
                _parent[irep] = jrep;
         
                // And increment the the result tree's rank by 1
                _rank[irep]++;
            }
        }
    }
}
