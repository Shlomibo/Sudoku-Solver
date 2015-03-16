using System;
using Digit = Sudoku_Solver.Board.Cell;
using System.ComponentModel;

namespace Sudoku_Solver.ViewModel
{
	public class Cell : INotifyPropertyChanged
	{
		#region Fields

		private Digit digit;
		#endregion
		#region Properties

		public Digit Digit
		{
			get { return this.digit; }
			set
			{
				if (this.digit != null)
				{
					value.ValueChanged -= Digit_ValueChanged;
				}

				this.digit = value;

				if (value != null)
				{
					value.ValueChanged += Digit_ValueChanged;
				}
			}
		}

		public string Text
		{
			get { return Digit.ToString(); }
			set
			{
				int val;

				if (string.IsNullOrEmpty(value))
				{
					Digit.Value = null;
				}
				else if (int.TryParse(value, out val))
				{
					Digit.Value = val;
				}
				else
				{
					throw new ArgumentOutOfRangeException(nameof(this.Text));
				}
			}
		}
		#endregion

		#region Events

		public event PropertyChangedEventHandler PropertyChanged = (s, e) => { };
		#endregion

		#region Ctor

		public Cell(Digit digit)
		{
			this.Digit = digit;
		}
		#endregion

		#region Methods

		private void Digit_ValueChanged(object sender, Utils.PropertyChangeEventArgsBase<int?> e)
		{
			PropertyChanged(this, new PropertyChangedEventArgs(nameof(this.Text)));
		}
		#endregion

	}
}
