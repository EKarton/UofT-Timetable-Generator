using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UoftTimetableGenerator.DataModels
{
    public class RedBlackTree
    {
        public int Num { get; set; }
        public string Color { get; set; }
        public RedBlackTree LeftTree { get; set; }
        public RedBlackTree RightTree { get; set; }
        public RedBlackTree Parent { get; set; }

        public RedBlackTree()
        {
            Num = -1;
            LeftTree = null;
            RightTree = null;
            Color = "Black";
            Parent = null;
        }

        public RedBlackTree(int num)
        {
            Num = num;
            LeftTree = new RedBlackTree();
            RightTree = new RedBlackTree();
            Color = "Black";
            Parent = null;
        }

        public bool IsEmpty
        {
            get { return Num == -1; }
        }


        public RedBlackTree Sibling
        {
            get
            {
                if (Parent.RightTree.Num != Num)
                    return Parent.RightTree;
                else if (Parent.LeftTree.Num != Num)
                    return Parent.LeftTree;
                return null;
            }
        }

        public bool IsValidTree()
        {
            // Testing content of an empty tree
            if (IsEmpty)
            {
                if (Num == -1 && LeftTree == null && RightTree == null && Color == "Black")
                    return true;
                Console.WriteLine("Null Error!");
                return false;
            }

            // Testing color
            if (Color == "Red")
            {
                if (!LeftTree.IsEmpty && LeftTree.Color == "Red")
                {
                    Console.WriteLine("Color Error!");
                    return false;
                }
                if (!RightTree.IsEmpty && RightTree.Color == "Red")
                {
                    Console.WriteLine("Color Error!");
                    return false;
                }
            }

            // Testing content
            if (LeftTree.IsEmpty == false)
            {
                if (LeftTree.Num >= Num || LeftTree.Parent != this)
                {
                    Console.WriteLine("Num error!");
                    return false;
                }
            }
            if (RightTree.IsEmpty == false)
            {
                if (RightTree.Num <= Num || RightTree.Parent != this)
                {
                    Console.WriteLine("Num error!");
                    return false;
                }
            }

            // Recurse to other parts of the tree
            return LeftTree.IsValidTree() && RightTree.IsValidTree();
        }

        public bool Contains(int num)
        {
            if (Num == num)
                return true;

            if (LeftTree != null && num < Num)
                return LeftTree.Contains(num);
            else if (RightTree != null && num > Num)
                return RightTree.Contains(num);
            else
                return false;
        }

        public bool IsLeaf()
        {
            if (LeftTree != null && RightTree != null)
            {
                if (LeftTree.IsEmpty && RightTree.IsEmpty && Num != -1)
                    return true;
            }
            return false;
        }

        public RedBlackTree PerformDeepCopy()
        {
            if (IsEmpty)
                return new RedBlackTree();

            RedBlackTree newTree = new RedBlackTree(Num);
            newTree.Color = Color;

            RedBlackTree leftSubtree_Copy = LeftTree.PerformDeepCopy();
            RedBlackTree rightSubtree_Copy = RightTree.PerformDeepCopy();

            newTree.LeftTree = leftSubtree_Copy;
            leftSubtree_Copy.Parent = newTree;
            newTree.RightTree = rightSubtree_Copy;
            rightSubtree_Copy.Parent = newTree;
            return newTree;
        }

        public bool IsEqualTo(RedBlackTree otherTree)
        {
            if (Num != otherTree.Num)
                return false;

            if (IsLeaf() && otherTree.IsLeaf())
                return true;

            if (LeftTree != null && otherTree.LeftTree != null)
            {
                if (LeftTree.Parent != this || otherTree.LeftTree.Parent != otherTree)
                    return false;
            }

            if (RightTree != null && otherTree.RightTree != null)
            {
                if (RightTree.Parent != this || otherTree.RightTree.Parent != otherTree)
                    return false;
            }

            bool isLeftTreesEqual = LeftTree.IsEqualTo(otherTree.LeftTree);
            bool isRightTreesEqual = RightTree.IsEqualTo(otherTree.RightTree);
            return isLeftTreesEqual && isRightTreesEqual;
        }

        public void PrintTree(string level = "")
        {
            Console.WriteLine(level + Num + ": " + Color);
            if (LeftTree != null)
                LeftTree.PrintTree(level + "  -");
            if (RightTree != null)
                RightTree.PrintTree(level + "  -");
        }

        public bool Insert(RedBlackTree node)
        {
            // Left tree
            if (node.Num < Num)
            {
                if (LeftTree.IsEmpty)
                {
                    LeftTree = node;
                    node.Parent = this;
                    node.Color = "Red";
                    RebalanceTree(node);
                    return true;
                }
                else
                    return LeftTree.Insert(node);
            }

            // Right tree
            else if (node.Num > Num)
            {
                if (RightTree.IsEmpty)
                {
                    RightTree = node;
                    node.Parent = this;
                    node.Color = "Red";
                    RebalanceTree(node);
                    return true;
                }
                else
                    return RightTree.Insert(node);
            }
            return false;
        }

        private void RebalanceTree(RedBlackTree newNode)
        {
            // Case 1
            if (Color == "Black")
                return;

            // Case 2
            else if (Color == "Red")
            {
                // Case 2A:
                if (Sibling.IsEmpty || Sibling.Color == "Black")
                {
                    if (newNode == LeftTree && this == Parent.LeftTree)
                        PerformInsertionReordering1(newNode);

                    else if (newNode == RightTree && this == Parent.LeftTree)
                        PerformInsertionReordering2(newNode);

                    else if (newNode == RightTree && this == Parent.RightTree)
                        PerformInsertionReordering3(newNode);

                    else if (newNode == LeftTree && this == Parent.RightTree)
                        PerformInsertionReordering4(newNode);
                }

                // Case 2B:
                else if (Sibling.Color == "Red")
                {
                    PerformInsertionRecoloring(newNode);
                }
            }
        }

        private void PerformInsertionReordering1(RedBlackTree newNode)
        {
            RedBlackTree k = newNode;
            RedBlackTree p = this;
            RedBlackTree g = Parent;
            RedBlackTree s = Sibling;

            RedBlackTree newG = new RedBlackTree(g.Num);
            newG.Color = "Red";
            newG.LeftTree = p.RightTree;
            p.RightTree.Parent = newG;
            newG.RightTree = s;
            s.Parent = newG;

            g.Num = p.Num;
            g.LeftTree = k;
            k.Parent = g;
            g.RightTree = newG;
            newG.Parent = g;
        }

        private void PerformInsertionReordering2(RedBlackTree newNode)
        {
            RedBlackTree k = newNode;
            RedBlackTree p = this;
            RedBlackTree g = Parent;
            RedBlackTree s = Sibling;

            RedBlackTree newG = new RedBlackTree(g.Num);
            newG.Color = "Red";
            newG.RightTree = s;
            s.Parent = newG;

            g.Num = k.Num;
        }

        private void PerformInsertionReordering3(RedBlackTree newNode)
        {
            RedBlackTree k = newNode;
            RedBlackTree p = this;
            RedBlackTree g = Parent;
            RedBlackTree s = Sibling;

            RedBlackTree newG = new RedBlackTree(g.Num);
            newG.Color = "Red";
            newG.LeftTree = s;
            s.Parent = newG;
            newG.RightTree = p.LeftTree;
            p.LeftTree.Parent = newG;

            g.Num = p.Num;
            g.LeftTree = newG;
            newG.Parent = g;
            g.RightTree = k;
            k.Parent = g;
        }

        private void PerformInsertionReordering4(RedBlackTree newNode)
        {
            RedBlackTree k = newNode;
            RedBlackTree p = this;
            RedBlackTree g = Parent;
            RedBlackTree s = Sibling;

            RedBlackTree newG = new RedBlackTree(g.Num);
            newG.Color = "Red";
            newG.LeftTree = s;
            s.Parent = newG;
            newG.RightTree = p.LeftTree;
            p.LeftTree.Parent = newG;

            g.Num = p.Num;
            g.LeftTree = newG;
            newG.Parent = g;
            g.RightTree = k;
            k.Parent = g;
        }

        private void PerformInsertionRecoloring(RedBlackTree newNode)
        {
            RedBlackTree k = newNode;
            RedBlackTree p = this;
            RedBlackTree g = Parent;
            RedBlackTree s = Sibling;

            g.Color = "Red";
            p.Color = "Black";
            s.Color = "Black";

            // Handle case if g is the root node
            if (g.Parent == null)
                g.Color = "Black";

            else if (g.Parent.Color == "Red")
                g.Parent.RebalanceTree(g);
        }

        public bool Delete(int number)
        {
            // Check if it is an empty node
            if (IsEmpty)
                return false;

            // If it is found
            if (Num == number)
            {
                // If it is a leaf, make it a null node
                if (LeftTree.IsEmpty && RightTree.IsEmpty)
                {
                    Num = -1;
                    LeftTree = null;
                    RightTree = null;
                }

                // If it has only one child (the left tree)
                else if (LeftTree.IsEmpty == false && RightTree.IsEmpty)
                {
                    RedBlackTree nextLeftTree = LeftTree.LeftTree;
                    RedBlackTree nextRightTree = LeftTree.RightTree;
                    string oldColor = Color;

                    Num = LeftTree.Num;
                    LeftTree = nextLeftTree;
                    nextLeftTree.Parent = this;
                    RightTree = nextRightTree;
                    nextRightTree.Parent = this;
                }

                // If it has only one child (the right tree)
                else if (Num == number && LeftTree.IsEmpty && RightTree.IsEmpty == false)
                {
                    RedBlackTree nextLeftTree = RightTree.LeftTree;
                    RedBlackTree nextRightTree = RightTree.RightTree;
                    string oldColor = Color;

                    Num = RightTree.Num;
                    LeftTree = nextLeftTree;
                    nextLeftTree.Parent = this;
                    RightTree = nextRightTree;
                    nextRightTree.Parent = this;
                }

                // If it has two children
                else
                {
                    // Get the max node of the left child
                    RedBlackTree minRightNode = RightTree.GetMin();

                    // Move the value of the max left child to the current node
                    Num = minRightNode.Num;

                    PerformDeletionRebalance(minRightNode);
                }
                return true;
            }

            // Recurse
            if (number < Num)
                return LeftTree.Delete(number);
            else if (number > Num)
                return RightTree.Delete(number);
            return false;
        }

        private void PerformDeletionRebalance(RedBlackTree removedNode)
        {
            // Case 1: If the in-order successor was red
            if (removedNode.Color == "Red")
                return;

            else if (removedNode.Color == "Black")
            {
                // Case 2: If the child of the in order successor was red
                if (removedNode.LeftTree.Color == "Red")
                {
                    removedNode.Color = "Black";
                    removedNode.LeftTree = removedNode.LeftTree.LeftTree;
                    removedNode.LeftTree.LeftTree.Parent = removedNode;
                }

                // Case 3-8: If the child of the in order successor was black
                else if (removedNode.LeftTree.Color == "Black")
                {
                    removedNode.Color = "Double Black";

                    if (removedNode.IsLeaf())
                    {
                        removedNode.Num = -1;
                        removedNode.LeftTree = null;
                        removedNode.RightTree = null;
                    }
                    else
                    {
                        removedNode.LeftTree = removedNode.LeftTree.LeftTree;
                        removedNode.LeftTree.LeftTree.Parent = removedNode;
                    }

                    PerformTransformations(removedNode);
                }
            }
        }

        private void PerformTransformations(RedBlackTree removedNode)
        {
            // Case 1: If it is the root of the tree
            if (removedNode.Parent == null)
            {
                PerformTransformationCase1(removedNode);
                return;
            }

            RedBlackTree p = removedNode.Parent;
            RedBlackTree n = removedNode;
            RedBlackTree s = removedNode.Sibling;
            RedBlackTree x = s.LeftTree;
            RedBlackTree y = s.RightTree;

            // Case 2:
            if (p.Color == "Black" && s.Color == "Red" && x.Color == "Black" && y.RightTree.Color == "Black")
                PerformTransformationCase2(p, n, s, x, y);

            // Case 3
            else if (p.Color == "Black" && s.Color == "Black" && x.Color == "Black" && y.Color == "Black")
                PerformTransformationCase3(p, n, s, x, y);

            // Case 4
            else if (p.Color == "Red" && s.Color == "Black" && x.Color == "Black" && y.Color == "Black")
                PerformTransformationCase4(p, n, s);

            // Case 5
            else if (s.Color == "Black" && x.Color == "Red" && y.Color == "Black")
                PerformTransformationCase5(p, n, s, x, y);

            // Case 6
            else if (s.Color == "Black" && y.Color == "Red")
                PerformTransformationCase6(p, n, s, x, y);
        }


        private void PerformTransformationCase1(RedBlackTree removedNode)
        {
            removedNode.Color = "Black";
        }

        private void PerformTransformationCase2(RedBlackTree p, RedBlackTree n, RedBlackTree s, RedBlackTree x, RedBlackTree y)
        {
            RedBlackTree newP = new RedBlackTree(p.Num);
            newP.Color = "Red";
            newP.LeftTree = n;
            n.Parent = newP;
            newP.RightTree = x;
            x.Parent = newP;

            RedBlackTree newS = p;
            newS.Num = s.Num;
            newS.LeftTree = newP;
            newP.Parent = newS;
            newS.RightTree = y;
            y.Parent = newS;

            PerformTransformations(n);
        }

        private void PerformTransformationCase3(RedBlackTree p, RedBlackTree n, RedBlackTree s, RedBlackTree x, RedBlackTree y)
        {
            RedBlackTree newS = new RedBlackTree(s.Num);
            s.Color = "Red";
            s.LeftTree = x;
            x.Parent = s;
            s.RightTree = y;
            y.Parent = s;

            p.RightTree = newS;
            newS.Parent = p;

            n.Color = "Black";
            p.Color = "Double Black";
            PerformTransformations(p);
        }

        private void PerformTransformationCase4(RedBlackTree p, RedBlackTree n, RedBlackTree s)
        {
            string oldParentColor = p.Color;
            p.Color = s.Color;
            s.Color = oldParentColor;

            n.Color = "Black";
        }

        private void PerformTransformationCase5(RedBlackTree p, RedBlackTree n, RedBlackTree s, RedBlackTree x, RedBlackTree y)
        {
            RedBlackTree newS = new RedBlackTree(s.Num);
            newS.Color = "Red";
            newS.LeftTree = x.RightTree;
            x.RightTree.Parent = newS;
            newS.RightTree = y;
            y.Parent = newS;

            RedBlackTree newX = new RedBlackTree(x.Num);
            newX.LeftTree = x.LeftTree;
            x.LeftTree.Parent = newX;
            newX.RightTree = newS;
            newS.Parent = newX;

            PerformTransformations(n);
        }

        private void PerformTransformationCase6(RedBlackTree p, RedBlackTree n, RedBlackTree s, RedBlackTree x, RedBlackTree y)
        {
            RedBlackTree newP = new RedBlackTree(p.Num);
            newP.LeftTree = n;
            n.Parent = newP;
            newP.RightTree = x;
            x.Parent = newP;
            n.Color = "Black";

            RedBlackTree newY = new RedBlackTree(y.Num);
            newY.Color = "Black";

            RedBlackTree newS = p;
            newS.Num = s.Num;
            newS.LeftTree = newP;
            newP.Parent = newS;
            newS.RightTree = newY;
            newY.Parent = newS;
        }

        private RedBlackTree GetMax()
        {
            if (RightTree.IsEmpty)
                return this;
            return RightTree.GetMax();
        }

        private RedBlackTree GetMin()
        {
            if (LeftTree.IsEmpty)
                return this;
            return LeftTree.GetMin();
        }
    }
}
