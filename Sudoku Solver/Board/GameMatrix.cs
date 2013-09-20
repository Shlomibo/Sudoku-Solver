using Sudoku_Solver.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using C = Sudoku_Solver.Utils.GlobalConsts;

namespace Sudoku_Solver.Board
{
	public class GameMatrix : IEnumerable<Cell>
	{
		#region Fields

		private Cell[,] cells = new Cell[C.BOARD_WIDTH, C.BOARD_HEIGHT];
		private bool isSolving = false;
		private object @lock = new object();
		private TaskScheduler currentScheduler = TaskScheduler.FromCurrentSynchronizationContext();
		#endregion

		#region Properties

		private bool IsSolving
		{
			get { lock (this.@lock) { return this.isSolving; } }
			set { lock (this.@lock) { this.isSolving = value; } }
		}

		public int Width { get { return C.BOARD_WIDTH; } }
		public int Height { get { return C.BOARD_HEIGHT; } }

		public int? this[int x, int y]
		{
			get { return this.cells[x, y].Value; }
			set { this.cells[x, y].Value = value; }
		}

		public Indexer<int, IEnumerable<int>> Rows
		{
			get
			{
				return new Indexer<int, IEnumerable<int>>(y => RowAt(y), () => Enumerable.Range(0, this.Height));
			}
		}

		public Indexer<int, IEnumerable<int>> Columns
		{
			get
			{
				return new Indexer<int, IEnumerable<int>>(x => ColumnAt(x), () => Enumerable.Range(0, this.Width));
			}
		}

		public Indexer<int, IEnumerable<int>> SubMatrixes
		{
			get
			{
				return new Indexer<int, IEnumerable<int>>(i => SubMatrixAt(i),
					() => Enumerable.Range(0, C.SUB_MAT_COUNT));
			}
		}
		#endregion

		#region Event

		public event EventHandler<CellEventArgs> CellValueChanged = (s, e) => { };
		public event EventHandler<CellEventArgs> CellHasValueChanged = (s, e) => { };
		#endregion

		#region Ctor

		public GameMatrix()
		{
			for (int x = 0; x < this.Width; x++)
			{
				for (int y = 0; y < this.Height; y++)
				{
					this.cells[x, y] = new Cell(this, x, y);
					this.cells[x, y].ValueChanging += Cell_ValueChanging;
					this.cells[x, y].ValueChanged += Cell_ValueChanged;
					this.cells[x, y].HasValueChanged += Cell_HasValueChanged;
				}
			}
		}
		#endregion

		#region Methods

		public IEnumerable<int> RowAt(int y)
		{
			if ((y < 0) || (y >= this.Height))
			{
				throw new IndexOutOfRangeException();
			}

			for (int x = 0; x < this.Width; x++)
			{
				if (this.cells[x, y].HasValue)
				{
					yield return this.cells[x, y].Value.Value;
				}
			}
		}

		public IEnumerable<int> ColumnAt(int x)
		{
			if ((x < 0) || (x >= this.Width))
			{
				throw new IndexOutOfRangeException();
			}

			for (int y = 0; y < this.Height; y++)
			{
				if (this.cells[x, y].HasValue)
				{
					yield return this.cells[x, y].Value.Value;
				}
			}
		}

		/// <summary>
		/// Convert board coordinates to sub matrix coordinates
		/// </summary>
		/// <param name="x">Horizontal board index</param>
		/// <param name="y">Vertical board index</param>
		/// <param name="subMatrixX">[Out] Sub matrix horizontal location</param>
		/// <param name="subMatrixY">[Out] Sub matrix vertical location</param>
		/// <param name="subIndexX">[Out] Sub matrix internal horizontal index</param>
		/// <param name="subIndexY">[Out] Sub matrix internal vertical index</param>
		public static void ToSubMatrix(
			int x,
			int y,
			out int subMatrixX,
			out int subMatrixY,
			out int subIndexX,
			out int subIndexY)
		{
			subMatrixX = x / C.SUB_MAT_WIDTH;
			subMatrixY = y / C.SUB_MAT_HEIGHT;
			subIndexX = x % C.SUB_MAT_WIDTH;
			subIndexY = y % C.SUB_MAT_HEIGHT;
		}

		/// <summary>
		/// Convert sub matrix coordinates to board coordinates
		/// </summary>
		/// <param name="subIndexX">Sub matrix internal horizontal index</param>
		/// <param name="subIndexY">Sub matrix internal vertical index</param>
		/// <param name="subMatrixX">Sub matrix horizontal location</param>
		/// <param name="subMatrixY">Sub matrix vertical location</param>
		/// <param name="x">[Out] Horizontal board index</param>
		/// <param name="y">[Out] Vertical board index</param>
		public static void FromSubMatrix(
			int subIndexX,
			int subIndexY,
			int subMatrixX,
			int subMatrixY,
			out int x,
			out int y)
		{
			x = subMatrixX * C.SUB_MAT_WIDTH + subIndexX;
			y = subMatrixY * C.SUB_MAT_HEIGHT + subIndexY;
		}

		public Cell GetCellAt(int x, int y)
		{
			return this.cells[x, y];
		}

		public IEnumerable<int> SubMatrixAt(int i)
		{
			int x = i % C.SUB_MAT_WIDTH;
			int y = (i / C.SUB_MAT_HEIGHT) * C.SUB_MAT_HEIGHT;

			for (int xIndex = x; xIndex < C.SUB_MAT_WIDTH; xIndex++)
			{
				for (int yIndex = y; yIndex < C.SUB_MAT_HEIGHT; yIndex++)
				{
					if (this.cells[xIndex, yIndex].HasValue)
					{
						yield return this.cells[xIndex, yIndex].Value.Value;
					}
				}
			}
		}

		private void Cell_HasValueChanged(object sender, PropertyChangeEventArgs<bool> e)
		{
			Task callerTask = new Task(() =>
				this.CellHasValueChanged(this, new CellEventArgs { Cell = sender as Cell }));
			callerTask.Start(this.currentScheduler);
			callerTask.Wait();
		}

		private void Cell_ValueChanged(object sender, PropertyChangeEventArgs<int?> e)
		{
			Task callerTask = new Task(() =>
				this.CellValueChanged(this, new CellEventArgs { Cell = sender as Cell }));
			callerTask.Start(this.currentScheduler);
			callerTask.Wait();
		}

		private void Cell_ValueChanging(object sender, CancelPropertyChangeEventArgs<int?> e)
		{
			Cell cell = sender as Cell;

			if (e.NewValue.HasValue)
			{
				if (this.Rows[cell.Y].Contains(e.NewValue.Value) ||
					this.Columns[cell.X].Contains(e.NewValue.Value) ||
					this.SubMatrixes[cell.SubMatrix].Contains(e.NewValue.Value))
				{
					e.Cancel = true;
					throw new InvalidOperationException(string.Format("Cell cannot contain the value '{0}'",
						e.NewValue));
				}
			}
		}

		IEnumerator<Cell> IEnumerable<Cell>.GetEnumerator()
		{
			for (int x = 0; x < this.Width; x++)
			{
				for (int y = 0; y < this.Height; y++)
				{
					yield return this.cells[x, y];
				}
			}
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return (this as IEnumerable<Cell>).GetEnumerator();
		}

		public bool TrySetCell(int x, int y, int? value)
		{
			bool isValid = true;

			if ((x < 0) || (x >= this.Width) ||
				(y < 0) || (y >= this.Height))
			{
				isValid = false;
			}

			if (isValid && value.HasValue)
			{
				if (this.Columns[x].Contains(value.Value) ||
					this.Rows[y].Contains(value.Value) ||
					this.SubMatrixes[this.cells[x, y].SubMatrix].Contains(value.Value))
				{
					isValid = false;
				}
				else
				{
					try
					{
						this.cells[x, y].Value = value;
					}
					catch (ArgumentOutOfRangeException)
					{
						isValid = false;
					}
				}
			}

			return isValid;
		}

		public Task<bool> SolveAsync(CancellationToken cancellationToken)
		{
			if (this.IsSolving)
			{
				throw new InvalidOperationException("Solving operation is already running");
			}
			else
			{
				this.IsSolving = true;
			}

			Task<bool> task = new Task<bool>(() =>
				{
					Cell bestCell = GetBestCell(cancellationToken);
					bool isSolved = SolveCell(bestCell, cancellationToken);

					return isSolved;
				},
				cancellationToken, TaskCreationOptions.LongRunning);

			task.Start(TaskScheduler.Default);

			this.IsSolving = false;

			return task;
		}

		public Task<bool> SolveAsync()
		{
			return SolveAsync(CancellationToken.None);
		}

		public bool Solve(int milliscondsTimeout)
		{
			Task<bool> solveTask = SolveAsync();
			solveTask.Wait(milliscondsTimeout);

			if (solveTask.IsFaulted)
			{
				throw new Exception("Solve task failed", solveTask.Exception);
			}

			return solveTask.Result;
		}

		public bool Solve()
		{
			return Solve(Timeout.Infinite);
		}

		private bool SolveCell(Cell cell, CancellationToken token)
		{
			IEnumerable<int> values = cell.AvailableValues;

			try
			{
				foreach (var value in values)
				{
					token.ThrowIfCancellationRequested();

					cell.Value = value;

					Cell bestCell = GetBestCell(token);

					token.ThrowIfCancellationRequested();

					if (SolveCell(bestCell, token))
					{
						return true;
					}
				}

				cell.Value = null;
				return false;
			}
			catch
			{
				cell.Value = null;
				throw;
			}
		}

		private Cell GetBestCell(CancellationToken token)
		{
			IEnumerable<Cell> cells = from cell in this
									  where !cell.HasValue
									  select cell;

			Cell bestCell = cells.FirstOrDefault();

			token.ThrowIfCancellationRequested();

			int best = (bestCell != null) ?
				bestCell.AvailableValues.Count() :
				C.MAX_CELL_VALUE - C.MIN_CELL_VALUE + 1;

			foreach (var cell in cells)
			{
				token.ThrowIfCancellationRequested();

				int validValues = cell.AvailableValues.Count();

				if (validValues == 0)
				{
					return cell;
				}
				else if (best > validValues)
				{
					best = validValues;
					bestCell = cell;
				}
			}

			return bestCell;
		}
		#endregion
	}
}
