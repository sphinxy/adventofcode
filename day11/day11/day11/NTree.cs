using System.Collections.Generic;

namespace day11
{
	class NTree<T>
	{
		public T data;
		public LinkedList<NTree<T>> children;
		public NTree<T> parent;

		public NTree(T data, NTree<T> parentNode)
		{
			this.data = data;
			children = new LinkedList<NTree<T>>();
			parent = parentNode;
		}

		public void AddChild(T data)
		{
			children.AddFirst(new NTree<T>(data, this));
		}

		public bool HasSameNode(T searchNode)
		{
			if (data.ToString() == searchNode.ToString())
			{
				return true;
			}
			foreach (NTree<T> child in (children))
			{
				return child.HasSameNode(searchNode);
			}
			return false;
		}
			
	}
}