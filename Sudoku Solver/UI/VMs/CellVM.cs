using Sudoku_Solver.Board;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using static Sudoku_Solver.Utils.GlobalConsts;
using MatrixCell = Sudoku_Solver.Board.Cell;

namespace Sudoku_Solver.UI.VMs
{
	internal sealed class CellVM : INotifyPropertyChanged
	{
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
			if (board == null)
			{
				throw new NullReferenceException(nameof(board));
			}

			Check(rowIndex, BOARD_HEIGHT - 1, nameof(rowIndex));
			Check(subMatIndex, SUB_MAT_COUNT - 1, nameof(subMatIndex));
			Check(subMatRowIndex, SUB_MAT_HEIGHT - 1, nameof(subMatRowIndex));
			Check(cellIndex, MAX_CELL_VALUE - MIN_CELL_VALUE, nameof(cellIndex));

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

		private void Check(int number, int maxValidValue, string paramName)
		{
			if ((number < 0) || (number > maxValidValue))
			{
				throw new ArgumentOutOfRangeException(paramName);
			}
		}

		public void Cell_ValueChanged(object sender, Utils.PropertyChangeEventArgsBase<int?> e)
		{
			OnPropertyChanged(nameof(Cell.Value));
		}

		private void GetActualCoordinates(out int x, out int y)
		{
			int innerX = this.cellIndex;
			int innerY = this.subMatRowIndex;

			int subMatX = this.subMatIndex;
			int subMatY = this.rowIndex;

			GameMatrix.FromSubMatrix(innerX, innerY, subMatX, subMatY, out x, out y);
		}

		private void OnPropertyChanged([CallerMemberName]string propertyName = null)
		{
			if (propertyName != null)
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		} 
		#endregion
	}
}
