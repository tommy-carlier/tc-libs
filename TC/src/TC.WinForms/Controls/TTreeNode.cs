// TC WinForms Library
// Copyright © 2008-2015 Tommy Carlier
// https://github.com/tommy-carlier/tc-libs/
// License: MIT License (MIT): https://github.com/tommy-carlier/tc-libs/blob/master/LICENSE

using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace TC.WinForms.Controls
{
	/// <summary>Represents a node of a <see cref="T:TTreeView"/>.</summary>
	[Serializable]
	public abstract class TTreeNode : TreeNode
	{
		/// <summary>Initializes a new instance of the <see cref="TTreeNode"/> class.</summary>
		/// <param name="label">The label of the node.</param>
		/// <param name="imageKey">The image key of the node.</param>
		/// <param name="selectedImageKey">The selected image key of the node.</param>
		/// <param name="hasChildNodes">if set to <c>true</c>, the node has child nodes.</param>
		protected TTreeNode(string label, string imageKey, string selectedImageKey, bool hasChildNodes)
		{
			if (label.IsNotNullOrEmpty())
				Text = label;
			
			if (imageKey.IsNotNullOrEmpty())
				ImageKey = imageKey;

			if (selectedImageKey.IsNotNullOrEmpty()) 
				SelectedImageKey = selectedImageKey;

			if (hasChildNodes)
			{
				_hasChildNodes = true;
				Nodes.Add(new TTreeView.LoadingTreeNode());
				Collapse();
			}
		}

		private readonly bool _hasChildNodes;

		internal bool HasChildNodes
		{
			get { return _hasChildNodes; }
		}

		internal TTreeNode[] LoadChildNodesInternal()
		{
			return LoadChildNodes();
		}

		/// <summary>When overriden in a derived class, loads the child-nodes of this node.</summary>
		/// <returns>An array of the child-nodes of this node.</returns>
		protected abstract TTreeNode[] LoadChildNodes();

		internal void RaiseDrag(ItemDragEventArgs e)
		{
			OnDrag(e);
		}

		/// <summary>Is called when the user starts dragging this node.</summary>
		/// <param name="e">The <see cref="ItemDragEventArgs"/> instance containing the event data.</param>
		protected virtual void OnDrag(ItemDragEventArgs e) { }

		internal void RaiseDoubleClick(TreeNodeMouseClickEventArgs e)
		{
			OnDoubleClick(e);
		}

		/// <summary>Is called when the user double-clicks this node.</summary>
		/// <param name="e">The <see cref="TreeNodeMouseClickEventArgs"/> instance containing the event data.</param>
		protected virtual void OnDoubleClick(TreeNodeMouseClickEventArgs e) { }

		internal void InitializeContextMenuInternal(ContextMenuStrip contextMenu)
		{
			InitializeContextMenu(contextMenu);
		}

		/// <summary>When overriden in a derived class, initializes the context-menu of this node.</summary>
		/// <param name="contextMenu">The context-menu to initialize.</param>
		protected virtual void InitializeContextMenu(ContextMenuStrip contextMenu) { }
	}
}
