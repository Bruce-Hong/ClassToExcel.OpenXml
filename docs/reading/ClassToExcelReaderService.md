# ClassToExcelReaderService

The ClassToExcelReaderService service convert Excel rows into class instances.  It takes each row in the Excel spreadsheet and maps the columns to properties on a class which results in a List\<T\>.


## Step 1: Create a class
Create a class that will hold your data.

## Step 2: Decorate instructions
Decorate the properties with the ClassToExcelAttribute. The ClassToExcelAttribute has the following properties:
* ColumnName - This is used when reading or writing files where a header row is present.
* Order - I highly recommend specifying the Order.  When writing, it specifies the order that the column is written to the file (i.e., The property marked with Order = 1 would be in Column A, etc.). If you do not specify order when writing a file, the data will written alphabetically using ColumnName, if specified, or property name.  When reading files, Order is used if you specify that no header column is present in the file. Again, if order isn't specified, the properties are sorted by ColunName, if specified, or property name and then read in that order.
* SyleFormat - This is used only when writing files.  It is used to style the column in the spreadsheet.
* Ignore - This is used when reading or writing files.  It means you don't want data read into the property and you don't want it written to the file.
* IsOptional - This is used only when reading files.  It is an optional property.  If specified, we will not report this as a missing header column if you don't find it in the Excel file.  If you aren't listening for messages when reading data or your data has no header row, you don't need to use this at all.

Here is an example:
```c#
public class Chicken
{
    public Chicken()
    {
        Owner = new Person { FirstName = "Farmer", LastName = "Brown", Age = 56};
    }

    [ClassToExcel(ColumnName = "Bird Name", Order = 1)]
    public string Name { get; set; }

    [ClassToExcel(Order = 2)]
    public int Age { get; set; }

    [ClassToExcel(ColumnName = "Date of Birth", Order = 3)]
    public DateTime DOB { get; set; }

    [ClassToExcel(StyleFormat = "$ #,###0.00", Order = 4)]
    public decimal Value { get; set; }

    [ClassToExcel(ColumnName = "Size of pen (sq. ft.)", StyleFormat = "#,###0", Order = 5)]
    public int SizeOfPenInSquareFeet { get; set; }
        
    /// <summary>Will be ignored.</summary>
    public Person Owner { get; set; }
}
```

Here is a list of built in style format supported by Excel:
* General
* 0
* 0.00
* #,##0
* #,##0.00
* 0%
* 0.00%
* 0.00E+00
* \# ?/?
* \# ??/??
* d/m/yyyy
* d-mmm-yy
* d-mmm
* mmm-yy
* h:mm tt
* h:mm:ss tt
* H:mm
* H:mm:ss
* m/d/yyyy H:mm
* #,##0 ;(#,##0)
* #,##0 ;\[Red\](#,##0)
* #,##0.00;(#,##0.00)
* #,##0.00;\[Red\](#,##0.00)
* mm:ss
* \[h\]:mm:ss
* mmss.0
* ##0.0E+0
* @

Other examples that work with Excel
* mm/dd/yy
* _($* #,##0.00_);_($* (#,##0.00);_($* "" - ""??_);_(@_)   

## Step 3: Read instructions
Create a ClassToExcelReaderService object specifying the class that will be used.  Call ReadWorksheet with an input (file path, stream or byte array), the name of the tab/worksheet, and specify if the first row is a header row (warning, if you say "no" the Order attribute will be used to find data if order is not specified on the class alphabetical order by ColumnName \[property name if ColumnName not specified\] will be assumed and data may be mapped incorrectly).

```c#
// File example
var readerService = new ClassToExcelReaderService<Person>();
List<Person> people = readerService.ReadWorksheet("c:\\temp\\Example1.xlsx", "People", true);
```

Notes
* ClassToExcelWriterService is disposable, but the ClassToExcelReaderService is not.
* If you pass in a stream, you still have dispose of it yourself.
* Objects and Arrays will be IGNORED on the class you created in Step 1.
* This code uses reflection, so it can set a value on a private setter in the class you created in Step 1.
