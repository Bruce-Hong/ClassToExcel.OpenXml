using System;
using System.Collections.Generic;

namespace ClassToExcel
{
    public class ClassToExcelRawRow
    {
        private int _rowNumber;

        public ClassToExcelRawRow()
        {
            Columns = new List<ClassToExcelRawColumn>();
        }

        /// <summary>Row Number (1 based)</summary>
        public int RowNumber
        {
            get { return _rowNumber; }
            set { _rowNumber = value; }
        }


        [Obsolete("Use RowNumber instead.")]
        public int RowIndex
        {
            get { return _rowNumber; }
            set { _rowNumber = value; }
        }

        public List<ClassToExcelRawColumn> Columns { get; set; }
    }
}