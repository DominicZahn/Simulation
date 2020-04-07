using System;
using System.Collections.Generic;
using System.Drawing;
public class TextGrid
{
    private char[] grid;
    private int columns;
    private int rows;
    private List<Line> lines = new List<Line>();

    public TextGrid(int rows, int columns)
    {
        grid = new char[columns * rows];
        this.columns = columns;
        this.rows = rows;
        for (int i = 0; i < columns * rows; i++)
        {
            grid[i] = ' ';
        }
    }

    /// <summary>
    /// Constructor that also fills the entire grid with the char c
    /// </summary>
    /// <param name="columns"></param>
    /// <param name="rows"></param>
    /// <param name="c"></param>
    public TextGrid(int rows, int columns, char c)
    {
        grid = new char[columns * rows];
        this.columns = columns;
        this.rows = rows;
        for (int i = 0; i < columns * rows; i++)
        {
            grid[i] = c;
        }
    }

    public int getColumns()
    {
        return this.columns;
    }

    public int getRows()
    {
        return this.rows;
    }

    /// <summary>
    /// Changes the char in the grid at pos (x|y) to c
    /// The top left is 0|0
    /// </summary>
    /// <param name="x">horizontal coordinat</param>
    /// <param name="y">vertical coordinat</param>
    /// <param name="c">new char in the grid</param>
    public void setChar(int x, int y, char c)
    {
        try
        {
            grid[gridIndex(x, y)] = c;
        } catch (ArgumentOutOfRangeException) { }
    }

    /// <summary>
    /// Returns the char in the grid at pos (x|y)
    /// The top left is 0|0
    /// </summary>
    /// <param name="x">horizontal coordinat</param>
    /// <param name="y">vertical coordinat</param>
    /// <returns>char at pos (x|y) in the grid</returns>
    public char getChar(int x, int y)
    {
        return grid[gridIndex(x, y)];
    }

    /// <summary>
    /// Creates a string that represents the grid.
    /// e.x. 0 1 2
    ///      3 4 5
    /// </summary>
    /// <returns>A string that represents the grid.</returns>
    public string gridToString()
    {
        string strGrid = "";
        for (int i = 0; i < columns; i++)
        {
            strGrid += columnToString(i) + "\n";
        }
        return strGrid;
    }

    /// <summary>
    /// Creates a string of a single column
    /// </summary>
    /// <param name="y">The choicen column. (Starts at 0)</param>
    /// <returns>The string which contains a whole column.</returns>
    public string columnToString(int y)
    {
        char[] arrColumn = new char[rows];
        for (int i = 0; i < rows; i++)
        {
            arrColumn[i] = getChar(i, y);
        }
        return new string(arrColumn);
    }

    /// <summary>
    /// Expands all lines and creates a new one if neccessary. (Only creates one at the time)
    /// </summary>
    /// <param name="maxLines">maximum amount of lines in a single grid</param>
    /// <param name="maxLength">maximum length of a line</param>
    public void expandLines(int maxLines, int maxLength)
    {

        foreach (Line curr in lines)
        {
            if (!curr.expand())
            {
                lines.Remove(curr);
            }
        }
        
        //adding new one if it is neccessary
        if (lines.Count < maxLines)
        {
            Random rng = new Random();
            lines.Add(new Line(rng.Next(0, rows), rng.Next(columns), maxLength));
        }
        
        updateGrid();
    }

    private void updateGrid()
    {
        clearGrid();
        foreach (Line curr in lines)
        {
            for (int y = 0; y < curr.getLength() - 1; y++) {
                setChar(curr.getStartPos().X, curr.getStartPos().Y + y, curr.getValue(y));
            }
        }
    }

    private void clearGrid()
    {
        for (int x = 0; x < rows; x++)
            for (int y = 0; y < columns; y++)
                setChar(x, y, ' ');
    }

    private int gridIndex(int x, int y)
    {
        testBounderies(x, y);
        return (y * rows + x);
    }

    private void testBounderies(int x, int y)
    {
        if (x < 0 || x >= rows)
            throw new ArgumentOutOfRangeException("x should be between 0 and " + rows);
        if (y < 0 || y >= columns)
            throw new ArgumentOutOfRangeException("y should be between 0 and " + columns);
    }

    /// <summary>
    /// Class that is used to generate a vertical line.
    /// </summary>
    private class Line
    {
        private bool random;
        private char[] value;
        private Point startPoint;
        private int length;
        private int maxLength;

        /// <summary>
        /// Constructor to create a uniform line with parameters.
        /// </summary>
        /// <param name="startX">x coordinat for the start point</param>
        /// <param name="startY">y coordinat for the start point</param>
        /// <param name="value">the char that should be used for the line</param>
        public Line(int startX, int startY, int maxLength, char value)
        {
            this.random = false;
            this.value = new char[1];
            this.value[0] = value;
            this.startPoint = new Point(startX, startY);
            this.length = 0;
            this.maxLength = maxLength;
        }

        /// <summary>
        /// Constructor to create a random line with only the start point given.
        /// The line changes its characters every expention.
        /// </summary>
        /// <param name="startX">x coordinat for the start point</param>
        /// <param name="startY">y coordinat for the start point</param>
        public Line(int startX, int startY, int maxLength)
        {
            this.random = true;
            this.startPoint = new Point(startX, startY);
            this.length = 0;
            this.maxLength = maxLength;
            this.value = new char[maxLength];
        }

        public int getLength()
        {
            return length;
        }

        public Point getStartPos()
        {
            return startPoint;
        }

        public Point getCurrPos()
        {
            return new Point(startPoint.X, startPoint.Y + length);
        }

        public char getValue(int pos)
        {
            if (pos >= maxLength)
                throw new ArgumentOutOfRangeException("Pos should be between 0 and " + maxLength);
            return value[pos];
        }

        /// <summary>
        /// expands the line by increasing the length
        /// and randomizes the value if neccessary
        /// </summary>
        /// <returns>false if it is at is maximum lenght now</returns>
        public bool expand()
        {
            length++;
            if (length > maxLength)
                return false;
            // randomizes the values of the line
            if (random)
            {
                for (int i = 0; i < maxLength; i++)
                {
                    Random rnd = new Random();
                    value[i] = (char) rnd.Next(48, 58);
                }
            }
            return true;
        }
    }
}
