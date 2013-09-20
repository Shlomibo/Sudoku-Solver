using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace Sudoku_Solver.UI.Converters
{
	class CellConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, string language)
		{
			string digit =null;

			if (((value is int) || (value is int?)) &&
				targetType == typeof(string))
			{
				if (value != null)
				{
					digit = value.ToString();
				}
			}

			return digit;
		}

		public object ConvertBack(object value, Type targetType, object parameter, string language)
		{
			int number;
			object result = null;
			
			if ((value is string) && 
				targetType.GetTypeInfo().IsAssignableFrom(typeof(int).GetTypeInfo()) &&
				int.TryParse((string)value, out number))
			{
				result = number;
			}

			return result;
		}
	}
}
