﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Digit = Sudoku_Solver.Board.Cell;
using C = Sudoku_Solver.Utils.GlobalConsts;
using System.ComponentModel;

namespace Sudoku_Solver.ViewModel
{
	public class Cell : INotifyPropertyChanged
	{
		#region Fields

		private Digit _digit;
		#endregion
		#region Properties

		public Digit Digit
		{
			get { return _digit; }
			set
			{
				if (_digit != null)
				{
					value.ValueChanged -= Digit_ValueChanged;
				}

				_digit = value;

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
					throw new ArgumentOutOfRangeException("Text");
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

		private void Digit_ValueChanged(object sender, Utils.PropertyChangeEventArgs<int?> e)
		{
			PropertyChanged(this, new PropertyChangedEventArgs("Text"));
		}
		#endregion

	}
}
