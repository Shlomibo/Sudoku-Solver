using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using C = Sudoku_Solver.Utils.GlobalConsts;

namespace Sudoku_Solver.UI.VMs
{
	class SubMatrixRowVM
	{
		#region Fields

		private BoardVM board;
		private int rowIndex;
		private int subMatIndex;
		private int subMatRowIndex;
		private CellVM[] cells; 
		#endregion

		#region Properties

		public CellVM[] Cells
		{
			get
			{
				if (this.cells == null)
				{
					this.cells = LoadCells();
				}

				return this.cells;
			}
		}
		#endregion

		#region Ctor

		public SubMatrixRowVM(BoardVM board, int rowIndex, int subMatIndex, int subMatRowIndex)
		{
			// TODO: Complete member initialization
			this.board = board;
			this.rowIndex = rowIndex;
			this.subMatIndex = subMatIndex;
			this.subMatRowIndex = subMatRowIndex;
		} 
		#endregion

		#region Methods

		private CellVM[] LoadCells()
		{
			CellVM[] cells = new CellVM[C.SUB_MAT_WIDTH];

			for (int cellIndex = 0; cellIndex < C.SUB_MAT_WIDTH; cellIndex++)
			{
				cells[cellIndex] = new CellVM(
					this.board,
					this.rowIndex,
					this.subMatIndex,
					this.subMatRowIndex,
					cellIndex);
			}

			return cells;
		}
		#endregion
	}
}
