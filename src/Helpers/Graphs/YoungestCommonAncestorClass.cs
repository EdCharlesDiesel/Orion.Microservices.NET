namespace Orion.Helpers.Graphs
{
    public abstract class YoungestCommonAncestorClass
    {
        // O(d) time | O(1) space - where d is the depth (height) of the ancestral tree
        public static AncestralTree GetYoungestCommonAncestor(
        AncestralTree topAncestor,
        AncestralTree descendantOne,
        AncestralTree descendantTwo
        )
        {
            int depthOne = GetDescendantDepth(descendantOne, topAncestor);
            int depthTwo = GetDescendantDepth(descendantTwo, topAncestor);
            if (depthOne > depthTwo)
            {
                return BacktrackAncestralTree(descendantOne, descendantTwo,
                depthOne - depthTwo);
            }

            return BacktrackAncestralTree(descendantTwo, descendantOne,
                depthTwo - depthOne);
        }

        public static int GetDescendantDepth(AncestralTree descendant, AncestralTree topAncestor)
        {
            int depth = 0;
            while (descendant != topAncestor)
            {
                depth++;
                descendant = descendant.Ancestor;
            }
            return depth;
        }

        private static AncestralTree BacktrackAncestralTree(
            AncestralTree lowerDescendant,
            AncestralTree higherDescendant,
            int diff)
        {
            while (diff > 0)
            {
                lowerDescendant = lowerDescendant.Ancestor;
                diff--;
            }
            while (lowerDescendant != higherDescendant)
            {
                lowerDescendant = lowerDescendant.Ancestor;
                higherDescendant = higherDescendant.Ancestor;
            }
            return lowerDescendant;
        }

        public abstract class AncestralTree
        {
            public char Name;
            public AncestralTree Ancestor;

            protected AncestralTree(char name, AncestralTree ancestor)
            {
                this.Name = name;
                this.Ancestor = ancestor;
                ancestor = null;
            }
            // This method is for testing only.
            public void AddAsAncestor(AncestralTree[] descendants)
            {
                foreach (AncestralTree descendant in descendants)
                {
                    descendant.Ancestor = this;
                }
            }
        }
    }
}
    
