using Sudoku_Solver.Board;
using Sudoku_Solver.UI.VMs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using C = Sudoku_Solver.Utils.GlobalConsts;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace Sudoku_Solver.UI
{
	public sealed partial class Board : UserControl
	{
		#region Fields

		private BoardVM vm;
		#endregion

		public Board()
		{
			this.InitializeComponent();

			this.vm = new BoardVM();
			this.DataContext = this.vm;
		}
	}
}
