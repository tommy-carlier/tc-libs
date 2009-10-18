// TC WinForms Library
// Copyright © 2008 - 2009 Tommy Carlier
// http://tc.codeplex.com
// License: Microsoft Public License (Ms-PL): http://tc.codeplex.com/license

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Security.Permissions;
using System.Text;
using System.Threading;
using System.Windows.Forms;

using TC.WinForms.Commands;
using TC.WinForms.Dialogs;

namespace TC.WinForms.Controls
{
	/// <summary>Displays a hierarchical collection of labeled items, each representing a <see cref="T:TTreeNode"/>.</summary>
	[ToolboxBitmap(typeof(TreeView))]
	public abstract class TTreeView : TreeView
	{
		/// <summary>Initializes a new instance of the <see cref="T:TTreeView"/> class.</summary>
		protected TTreeView()
		{
			_reloadSelectedNodeCommand = new SimpleActionCommand(ReloadSelectedNode);

			ImageList icons = new ImageList();
			icons.ImageSize = new Size(16, 16);
			icons.ColorDepth = ColorDepth.Depth32Bit;
			icons.Images.Add("_LoadingTreeNode", LoadingTreeNodeIcon);
			InitializeIcons(icons);
			ImageList = icons;

			ShowLines = false;
			ShowNodeToolTips = true;
		}

		#region icon-related members

		/// <summary>When overriden in a derived class, gets the icon for the 'Loading…'-node.</summary>
		/// <value>The icon for the 'Loading…'-node.</value>
		protected abstract Image LoadingTreeNodeIcon { get; }

		/// <summary>When overriden in a derived class, initializes the icons.</summary>
		/// <param name="icons">The <see cref="T:ImageList"/> to add icons to.</param>
		protected virtual void InitializeIcons(ImageList icons) { }

		#endregion

		#region overriding properties to change their default values

		/// <summary>Gets or sets a value indicating whether lines are drawn
		/// between tree nodes in the tree view control.</summary>
		/// <returns>true if lines are drawn between tree nodes in the tree view control;
		/// otherwise, false. The default is false.</returns>
		[DefaultValue(false)]
		public new bool ShowLines
		{
			get { return base.ShowLines; }
			set { base.ShowLines = value; }
		}

		/// <summary>Gets or sets a value indicating ToolTips are shown when the mouse pointer
		/// hovers over a <see cref="T:TreeNode"/>.</summary>
		/// <returns>true if ToolTips are shown when the mouse pointer hovers over a <see cref="T:TreeNode"/>;
		/// otherwise, false. The default is true.</returns>
		[DefaultValue(true)]
		public new bool ShowNodeToolTips
		{
			get { return base.ShowNodeToolTips; }
			set { base.ShowNodeToolTips = value; }
		}

		#endregion

		#region overriding OnHandleCreated and CreateParams

		/// <summary>Raises the <see cref="E:HandleCreated"/> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs"/> that contains the event data.</param>
		protected override void OnHandleCreated(EventArgs e)
		{
			base.OnHandleCreated(e);

			// if Windows Vista or later: autoscroll horizontally, autohide the expandos and turn on double buffering:
			// http://www.danielmoth.com/Blog/2007/01/treeviewvista.html
			if (SystemUtilities.IsWindowsVistaOrLater)
			{
				IntPtr handle = Handle;
				int style
					= NativeMethods.SendMessage(
						handle,
						NativeMethods.TVM_GETEXTENDEDSTYLE,
						IntPtr.Zero,
						IntPtr.Zero).ToInt32()
					| NativeMethods.TVS_EX_AUTOHSCROLL
					| NativeMethods.TVS_EX_FADEINOUTEXPANDOS
					| NativeMethods.TVS_EX_DOUBLEBUFFER;

				NativeMethods.SendMessage(
					handle,
					NativeMethods.TVM_SETEXTENDEDSTYLE,
					IntPtr.Zero,
					new IntPtr(style));
			}

			// use the Explorer visual style
			this.SetExplorerWindowTheme();
		}

		/// <summary>Overrides <see cref="P:CreateParams"/>.</summary>
		/// <returns>A <see cref="T:CreateParams"/> that contains the required creation parameters
		/// when the handle to the control is created.</returns>
		protected override CreateParams CreateParams
		{
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
			get
			{
				// if Windows Vista or later: lose the horizontal scrollbar:
				// http://www.danielmoth.com/Blog/2007/01/treeviewvista.html
				CreateParams createParams = base.CreateParams;
				if (SystemUtilities.IsWindowsVistaOrLater)
					createParams.Style |= NativeMethods.TVS_NOHSCROLL;
				return createParams;
			}
		}

		#endregion

		#region loading nodes

		/// <summary>Reloads the top-level nodes.</summary>
		public void ReloadTopLevelNodes()
		{
			if (!Created) CreateHandle();

			BeginUpdate();
			Nodes.Clear();
			Nodes.Add(new LoadingTreeNode());
			EndUpdate();
			ThreadPool.QueueUserWorkItem(delegate
			{
				try
				{
					TTreeNode[] topLevelNodes = LoadTopLevelNodes();
					if (IsHandleCreated)
						this.InvokeAsync(nodes => LoadNodes(Nodes, nodes), topLevelNodes);
				}
				catch (Exception exception)
				{
					if (exception.IsCritical()) throw;
					else if (IsHandleCreated)
						this.InvokeAsync(TMessageDialog.ShowError, this, exception);
				}
			});
		}

		/// <summary>When overriden in a derived class, loads the top-level nodes.</summary>
		/// <returns>An array of the top-level nodes.</returns>
		protected abstract TTreeNode[] LoadTopLevelNodes();

		private void LoadNodes(TreeNodeCollection nodeCollection, TTreeNode[] nodes)
		{
			BeginUpdate();
			try
			{
				nodeCollection.Clear();
				nodeCollection.AddRange(nodes);
			}
			finally { EndUpdate(); }
		}

		private readonly ICommand _reloadSelectedNodeCommand;

		/// <summary>Gets the command to reload the child nodes of the selected node.</summary>
		/// <value>The command to reload the child nodes of the selected node.</value>
		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public ICommand ReloadSelectedNodeCommand { get { return _reloadSelectedNodeCommand; } }

		/// <summary>Reloads the child nodes of the selected node.</summary>
		public void ReloadSelectedNode()
		{
			TTreeNode node = SelectedNode as TTreeNode;
			if (node != null && node.HasChildNodes)
			{
				bool expand = node.IsExpanded;
				BeginUpdate();
				node.Nodes.Clear();
				node.Nodes.Add(new LoadingTreeNode());
				node.Collapse();
				EndUpdate();
				if (expand) node.Expand();
			}
		}

		#endregion

		#region interacting with nodes

		/// <summary>Raises the <see cref="E:AfterExpand"/> event.</summary>
		/// <param name="e">A <see cref="T:TreeViewEventArgs"/> that contains the event data.</param>
		protected override void OnAfterExpand(TreeViewEventArgs e)
		{
			base.OnAfterExpand(e);
			TTreeNode node = e.Node as TTreeNode;
			if (node != null && node.Nodes.Count == 1 && node.Nodes[0] is LoadingTreeNode)
				ThreadPool.QueueUserWorkItem(delegate
				{
					try
					{
						TTreeNode[] childNodes = node.LoadChildNodesInternal();
						if (IsHandleCreated)
							this.InvokeAsync(nodes => LoadNodes(node.Nodes, nodes), childNodes);
					}
					catch (Exception exception)
					{
						if (exception.IsCritical()) throw;
						else if (IsHandleCreated)
						{
							this.InvokeAsync(node.Collapse);
							this.InvokeAsync(TMessageDialog.ShowError, this, exception);
						}
					}
				});
		}

		/// <summary>Raises the <see cref="E:ItemDrag"/> event.</summary>
		/// <param name="e">An <see cref="T:ItemDragEventArgs"/> that contains the event data.</param>
		protected override void OnItemDrag(ItemDragEventArgs e)
		{
			base.OnItemDrag(e);
			TTreeNode node = e.Item as TTreeNode;
			if (node != null)
			{
				SelectedNode = node;
				node.RaiseDrag(e);
			}
		}

		/// <summary>Raises the <see cref="E:NodeMouseDoubleClick"/> event.</summary>
		/// <param name="e">A <see cref="T:TreeNodeMouseClickEventArgs"/> that contains the event data.</param>
		protected override void OnNodeMouseDoubleClick(TreeNodeMouseClickEventArgs e)
		{
			base.OnNodeMouseDoubleClick(e);
			TTreeNode node = e.Node as TTreeNode;
			if (node != null && e.Button == MouseButtons.Left)
				node.RaiseDoubleClick(e);
		}

		/// <summary>Raises the <see cref="E:NodeMouseClick"/> event.</summary>
		/// <param name="e">A <see cref="T:TreeNodeMouseClickEventArgs"/> that contains the event data.</param>
		protected override void OnNodeMouseClick(TreeNodeMouseClickEventArgs e)
		{
			base.OnNodeMouseClick(e);
			if (e.Button == MouseButtons.Right)
			{
				TTreeNode node = e.Node as TTreeNode;
				if (node != null)
				{
					SelectedNode = node;
					ContextMenuStrip contextMenu = new ContextMenuStrip();
					node.InitializeContextMenuInternal(contextMenu);
					if (contextMenu.Items.HasItems())
					{
						contextMenu.Closed += delegate { this.InvokeAsync(contextMenu.Dispose); };
						contextMenu.Show(this, e.Location);
					}
					else contextMenu.Dispose();
				}
			}
		}

		#endregion

		#region inner class LoadingTreeNode

		internal sealed class LoadingTreeNode : TreeNode
		{
			public LoadingTreeNode()
			{
				Text = "Loading…";
				NodeFont = SystemFont.Italic.ToFont();
				ForeColor = DrawingUtilities.GetAverageColor(ForeColor, BackColor);
				ImageKey = "_LoadingTreeNode";
				SelectedImageKey = "_LoadingTreeNode";
			}
		}

		#endregion
	}
}
