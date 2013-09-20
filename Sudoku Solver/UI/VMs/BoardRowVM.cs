using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using C = Sudoku_Solver.Utils.GlobalConsts;

namespace Sudoku_Solver.UI.VMs
{
	class BoardRowVM
	{
		#region Fields

		private BoardVM board;
		private int rowIndex;
		private SubMatrixVM[] subMatrix;
		#endregion

		#region Properties

		public SubMatrixVM[] SubMatrix
		{
			get
			{
				if (this.subMatrix == null)
				{
					this.subMatrix = LoadSubMatrix();
				}

				return this.subMatrix;
			}
		}
		#endregion

		#region Ctor

		public BoardRowVM(BoardVM boardVM, int rowIndex)
		{
			// TODO: Complete member initialization
			this.board = boardVM;
			this.rowIndex = rowIndex;
		} 
		#endregion

		#region Methods

		private SubMatrixVM[] LoadSubMatrix()
		{
			SubMatrixVM[] subMarices = new SubMatrixVM[C.BOARD_WIDTH / C.SUB_MAT_WIDTH];

			for (int subMatIndex = 0; subMatIndex < C.BOARD_WIDTH / C.SUB_MAT_WIDTH; subMatIndex++)
			{
				subMarices[subMatIndex] = new SubMatrixVM(this.board, this.rowIndex, subMatIndex);
			}

			return subMarices;
		} 
		#endregion
	}
}
