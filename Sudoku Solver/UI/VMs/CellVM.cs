using Sudoku_Solver.Board;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using C = Sudoku_Solver.Utils.GlobalConsts;
using MatrixCell = Sudoku_Solver.Board.Cell;

namespace Sudoku_Solver.UI.VMs
{
	class CellVM : INotifyPropertyChanged
	{
		#region Consts

		private const string PRP_VALUE = "Value"; 
		#endregion

		#region Fields

		private BoardVM board;
		private int rowIndex;
		private int subMatIndex;
		private int subMatRowIndex;
		private int cellIndex; 
		#endregion

		#region Properties

		public MatrixCell Cell { get; set; }

		public int? Value
		{
			get { return this.Cell.Value; }
			set { this.Cell.Value = value; }
		} 
		#endregion

		#region Events

		public event PropertyChangedEventHandler PropertyChanged = (s, e) => { }; 
		#endregion

		#region Ctor

		public CellVM(BoardVM board, int rowIndex, int subMatIndex, int subMatRowIndex, int cellIndex)
		{
			// TODO: Complete member initialization
			this.board = board;
			this.rowIndex = rowIndex;
			this.subMatIndex = subMatIndex;
			this.subMatRowIndex = subMatRowIndex;
			this.cellIndex = cellIndex;

			int x;
			int y;

			GetActualCoordinates(out x, out y);
			this.Cell = this.board.Board.GetCellAt(x, y);
			this.Cell.ValueChanged += Cell_ValueChanged;
		} 
		#endregion

		#region Methods

		public void Cell_ValueChanged(object sender, Utils.PropertyChangeEventArgs<int?> e)
		{
			OnPropertyChanged(PRP_VALUE);
		}

		private void GetActualCoordinates(out int x, out int y)
		{
			int innerX = this.cellIndex;
			int innerY = this.subMatRowIndex;

			int subMatX = this.subMatIndex;
			int subMatY = this.rowIndex;

			GameMatrix.FromSubMatrix(innerX, innerY, subMatX, subMatY, out x, out y);
		}

		protected virtual void OnPropertyChanged([CallerMemberName]string propertyName = null)
		{
			if (propertyName != null)
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		} 
		#endregion
	}
}
