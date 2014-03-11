
using System.Collections;
using System.Collections.Generic;

namespace Y_Vision.ConnectedComponentLabeling
{
   /// <summary>
    /// A Union-Find/Disjoint-Set data structure.
    /// Source: http://www.mathblog.dk/files/2012/07/DisjointSet.cs
    ///         http://www.emilstefanov.net/Projects/Files/DisjointSets/DisjointSets.cs
    /// </summary>
    public class DynamicDisjointSet {

        /// <summary>
        /// The number of elements in the universe.
        /// </summary>
        public int Count { get; private set; }

       /// <summary>
       /// The parent of each element in the universe.
       /// </summary>
       private readonly List<Node> nodes;

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
        public DynamicDisjointSet(int count) {
            Count = count;
            SetCount = count;
            nodes = new List<Node>(Count);

            for (int i = 0; i < Count; i++)
            {
                nodes[i] = new Node(0, i);
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
            if (nodes[i].ParentIndex == i) {
                return i;
            }
            // Recursively find the real parent of i, and then cache it for later lookups.
            var n = nodes[i];
            n.ParentIndex = Find(nodes[i].ParentIndex);
            return n.ParentIndex;
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
                irank = nodes[irep].Rank,
                // Get the rank of j's tree
                jrank = nodes[irep].Rank;

            // Elements are in the same set, no need to unite anything.
            if (irep == jrep)
                return;

            SetCount--;

            // If i's rank is less than j's rank
            if (irank < jrank) {
         
                // Then move i under j
                nodes[irep].SetParent(jrep);

            } // Else if j's rank is less than i's rank
            else if (jrank < irank) {
         
                // Then move j under i
                nodes[jrep].SetParent(irep);

            } // Else if their ranks are the same
            else {
         
                // Then move i under j (doesn't matter which one goes where)
                nodes[irep].SetParent(jrep);
         
                // And increment the the result tree's rank by 1
                nodes[irep].IncRank();
                nodes[irep].SetParent(jrep);
            }
        }

        private struct Node
        {
            public Node(int rank, int parentIndex)
            {
                Rank = rank;
                ParentIndex = parentIndex;
            }

            public void IncRank()
            {
                Rank = Rank+1;
            }

            public void SetParent(int p)
            {
                ParentIndex = p;
            }

            /// <summary>
            /// This roughly represent the max height of the node in its subtree.
            /// </summary>
            public int Rank;

            /// <summary>
            /// The the index of the parent node of the node.
            /// </summary>
            public int ParentIndex;
        }
    }
}
