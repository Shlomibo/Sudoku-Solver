using Sudoku_Solver.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using C = Sudoku_Solver.Utils.GlobalConsts;

namespace Sudoku_Solver.Board
{
	public class Cell : INotifyPropertyChanged
	{
		#region Fields

		private int? value;
		#endregion

		#region Properties

		public bool HasValue { get { return this.value.HasValue; } }

		public int? Value
		{
			get { return this.value; }
			set
			{
				if (this.value.HasValue &&
					((this.value < C.MIN_CELL_VALUE) || (this.value > C.MAX_CELL_VALUE)))
				{
					throw new ArgumentOutOfRangeException("Value");
				}

				if (this.value != value)
				{
					CancelPropertyChangeEventArgs<int?> e = new CancelPropertyChangeEventArgs<int?>(this.value, value);

					this.ValueChanging(this, e);

					if (!e.Cancel)
					{
						this.value = value;

						bool oldIsNull = !this.value.HasValue;

						this.value = value;

						this.ValueChanged(this, e);

						if (oldIsNull == value.HasValue)
						{
							this.HasValueChanged(
								this,
								new PropertyChangeEventArgs<bool>(!value.HasValue, value.HasValue));
						}
					}
				}
			}
		}

		public GameMatrix Board { get; private set; }
		public int X { get; private set; }
		public int Y { get; private set; }

		public int SubMatrix
		{
			get
			{
				int i = (this.Y / C.SUB_MAT_HEIGHT) * C.SUB_MAT_HEIGHT; ;

				i += this.X / 3;

				return i;
			}
		}

		public IEnumerable<int> AvailableValues
		{
			get
			{
				IEnumerable<int> values = Enumerable.Range(C.MIN_CELL_VALUE, 
					C.MAX_CELL_VALUE - C.MIN_CELL_VALUE + 1);
				values = values.Except(this.Board.Columns[this.X]);

				if (values.Any())
				{
					values = values.Except(this.Board.Rows[this.Y]);

					if (values.Any())
					{
						values = values.Except(this.Board.SubMatrixes[this.SubMatrix]);  
					}
				}

				return values.ToArray();
			}
		}
		#endregion

		#region Events

		public event EventHandler<PropertyChangeEventArgs<int?>> ValueChanged = (s, e) => { };
		public event EventHandler<PropertyChangeEventArgs<bool>> HasValueChanged = (s, e) => { };
		public event EventHandler<CancelPropertyChangeEventArgs<int?>> ValueChanging = (s, e) => { };
		public event PropertyChangedEventHandler PropertyChanged = (s, e) => { };
		#endregion

		#region Ctor

		public Cell(GameMatrix board, int x, int y, int? value)
		{
			this.Board = board;
			this.X = x;
			this.Y = y;

			this.Value = value;
		}

		public Cell(GameMatrix board, int x, int y) 
			: this(board, x, y, null) { }
		#endregion

		#region Methods

		public override string ToString()
		{
			if (this.Value.HasValue)
			{
				return this.Value.ToString();
			}
			else
			{
				return "";
			}
		}
		#endregion
	}
}
