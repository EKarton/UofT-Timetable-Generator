using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UoftTimetableGenerator.Generator
{
    public class RedBlackTree<T> : IOrderedCollection<T> where T: IComparable
    {
        private string color = "Black";
        private T content = default(T);  // returns null if T is a class type
        private RedBlackTree<T> leftTree = null;
        private RedBlackTree<T> rightTree = null;
        private RedBlackTree<T> parent = null;

        public RedBlackTree<T> LeftTree
        {
            get { return leftTree; }
            set { leftTree = value; }
        }

        public RedBlackTree<T> RightTree
        {
            get { return rightTree; }
            set { rightTree = value; }
        }

        public RedBlackTree<T> Parent
        {
            get { return parent; }
            set { parent = value; }
        }

        public T Content
        {
            get { return content; }
            set { content = value; }
        }

        public bool IsEmpty
        {
            get { return content == null;  }            
        }

        public bool IsLeaf
        {
            get
            {
                if (leftTree != null && rightTree != null)
                {
                    if (leftTree.IsEmpty && rightTree.IsEmpty && content != null)
                        return true;
                }
                return false;
            }
        }

        public RedBlackTree<T> Sibling
        {
            get
            {
                if (parent != null && parent.rightTree.content != null && !parent.rightTree.content.Equals(content))
                    return parent.rightTree;
                else if (parent != null && parent.leftTree.content != null && !parent.leftTree.content.Equals(content))
                    return parent.leftTree;
                else
                    return null;
            }
        }

        public RedBlackTree() {  }

        public RedBlackTree(T item)
        {
            content = item;
            leftTree = new RedBlackTree<T>();
            rightTree = new RedBlackTree<T>();
        }

        /// <summary>
        /// Prints the tree onto the console. Uses the .ToString() for printing out object info
        /// </summary>
        /// <param name="tree">A node in the tree</param>
        public void Show()
        {
            Show("");
        }

        /// <summary>
        /// Prints the tree onto the console. Uses the .ToString() for printing out object info
        /// </summary>
        /// <param name="tree">A node in the tree</param>
        /// <param name="tabs">The number of tabs to offset the println()</param>
        private void Show(string tabs)
        {
            if (IsEmpty)
            {
                Console.WriteLine(tabs + " --NULL [" + color + "]");
                return;
            }

            Console.WriteLine(tabs + " -" + Content.ToString() + " [" + color + "]");
            leftTree.Show(tabs + "  ");
            rightTree.Show(tabs + "  ");
        }

        public IOrderedCollection<T> MakeCopy()
        {
            if (IsEmpty)
                return new RedBlackTree<T>();

            RedBlackTree<T> newTree = new RedBlackTree<T>(content);
            newTree.color = color;

            RedBlackTree<T> leftSubtree_Copy = (RedBlackTree<T>) leftTree.MakeCopy();
            RedBlackTree<T> rightSubtree_Copy = (RedBlackTree<T>) rightTree.MakeCopy();

            newTree.leftTree = leftSubtree_Copy;
            leftSubtree_Copy.parent = newTree;
            newTree.rightTree = rightSubtree_Copy;
            rightSubtree_Copy.parent = newTree;
            return newTree;
        }

        public bool Contains(T item)
        {
            if (IsEmpty)
                return false;

            if (content.Equals(item))
                return true;

            if (leftTree != null && item.CompareTo(content) < 0)
                return leftTree.Contains(item);
            else if (rightTree != null && item.CompareTo(content) > 0)
                return rightTree.Contains(item);
            else
                return false;
        }

        public bool CanAdd(T newItem)
        {
            if (IsEmpty)
                return true;

            if (leftTree != null && newItem.CompareTo(content) < 0)
                return leftTree.CanAdd(newItem);
            else if (rightTree != null && newItem.CompareTo(content) > 0)
                return rightTree.CanAdd(newItem);
            else
                return false;
        }

        public bool Add(T newItem)
        {
            // If it is an empty tree
            if (IsEmpty)
            {
                content = newItem;
                color = "Black";
                leftTree = new RedBlackTree<T>();
                rightTree = new RedBlackTree<T>();
                return true;
            }

            // Left tree
            if (newItem.CompareTo(content) < 0)
            {
                if (leftTree.IsEmpty)
                {
                    leftTree = new RedBlackTree<T>(newItem);
                    leftTree.parent = this;
                    leftTree.color = "Red";
                    RebalanceTree(leftTree);
                    return true;
                }
                else
                    return leftTree.Add(newItem);
            }

            // Right tree
            else if (newItem.CompareTo(content) > 0)
            {
                if (rightTree.IsEmpty)
                {
                    rightTree = new RedBlackTree<T>(newItem);
                    rightTree.parent = this;
                    rightTree.color = "Red";
                    RebalanceTree(rightTree);
                    return true;
                }
                else
                    return rightTree.Add(newItem);
            }
            return false;
        }

        private void RebalanceTree(RedBlackTree<T> addedItem)
        {
            // Case 1
            if (color == "Black")
                return;

            // Case 2
            else if (color == "Red" && Sibling != null)
            {
                // Case 2A:
                if (Sibling.IsEmpty || Sibling.color == "Black")
                {
                    if (addedItem == leftTree && this == parent.leftTree)
                        PerformInsertionReordering1(addedItem);

                    else if (addedItem == rightTree && this == parent.leftTree)
                        PerformInsertionReordering2(addedItem);

                    else if (addedItem == rightTree && this == parent.rightTree)
                        PerformInsertionReordering3(addedItem);

                    else if (addedItem == leftTree && this == parent.rightTree)
                        PerformInsertionReordering4(addedItem);
                }

                // Case 2B:
                else if (Sibling.color == "Red")
                {
                    PerformInsertionRecoloring(addedItem);
                }
            }
        }

        private void PerformInsertionReordering1(RedBlackTree<T> newNode)
        {
            RedBlackTree<T> k = newNode;
            RedBlackTree<T> p = this;
            RedBlackTree<T> g = parent;
            RedBlackTree<T> s = Sibling;

            RedBlackTree<T> newG = new RedBlackTree<T>(g.content);
            newG.color = "Red";
            newG.leftTree = p.rightTree;
            p.rightTree.parent = newG;
            newG.rightTree = s;
            s.parent = newG;

            g.content = p.content;
            g.leftTree = k;
            k.parent = g;
            g.rightTree = newG;
            newG.parent = g;
        }

        private void PerformInsertionReordering2(RedBlackTree<T> newNode)
        {
            RedBlackTree<T> k = newNode;
            RedBlackTree<T> p = this;
            RedBlackTree<T> g = parent;
            RedBlackTree<T> s = Sibling;

            RedBlackTree<T> newG = new RedBlackTree<T>(g.content);
            newG.color = "Red";
            newG.rightTree = s;
            s.parent = newG;

            g.content = k.content;
        }

        private void PerformInsertionReordering3(RedBlackTree<T> newNode)
        {
            RedBlackTree<T> k = newNode;
            RedBlackTree<T> p = this;
            RedBlackTree<T> g = parent;
            RedBlackTree<T> s = Sibling;

            RedBlackTree<T> newG = new RedBlackTree<T>(g.content);
            newG.color = "Red";
            newG.leftTree = s;
            s.parent = newG;
            newG.rightTree = p.leftTree;
            p.leftTree.parent = newG;

            g.content = p.content;
            g.leftTree = newG;
            newG.parent = g;
            g.rightTree = k;
            k.parent = g;
        }

        private void PerformInsertionReordering4(RedBlackTree<T> newNode)
        {
            RedBlackTree<T> k = newNode;
            RedBlackTree<T> p = this;
            RedBlackTree<T> g = parent;
            RedBlackTree<T> s = Sibling;

            RedBlackTree<T> newG = new RedBlackTree<T>(g.content);
            newG.color = "Red";
            newG.leftTree = s;
            s.parent = newG;
            newG.rightTree = p.leftTree;
            p.leftTree.parent = newG;

            g.content = p.content;
            g.leftTree = newG;
            newG.parent = g;
            g.rightTree = k;
            k.parent = g;
        }

        private void PerformInsertionRecoloring(RedBlackTree<T> newNode)
        {
            RedBlackTree<T> k = newNode;
            RedBlackTree<T> p = this;
            RedBlackTree<T> g = parent;
            RedBlackTree<T> s = Sibling;

            g.color = "Red";
            p.color = "Black";
            s.color = "Black";

            // Handle case if g is the root node
            if (g.parent == null)
                g.color = "Black";

            else if (g.parent.color == "Red")
                g.parent.RebalanceTree(g);
        }

        public bool Delete(T item)
        {
            throw new NotImplementedException();
        }

        public List<T> GetContents()
        {
            if (IsEmpty)
                return new List<T>();

            List<T> contents = new List<T>();
            if (!leftTree.IsEmpty)
                contents.AddRange(leftTree.GetContents());
            contents.Add(content);
            if (!rightTree.IsEmpty)
                contents.AddRange(rightTree.GetContents());
            return contents;
        }
    }
}
