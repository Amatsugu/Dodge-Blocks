using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LuminousVector.LevelGenerator.Execeptions
{
	public class InvalidSliceExeception : Exception
	{
		public InvalidSliceExeception(string message) : base ("This slice is invalid: " + message)
		{

		}
	}
}
