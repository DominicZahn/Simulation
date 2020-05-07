using System;
using System.Collections.Generic;
using System.Drawing;

public class TextGrid
{
    private char[] grid;
    private int columns;
    private int rows;
    private Line[] lines;
    private List<Text> texts;
    private List<Button> buttons;
    private int lineCount;
    private int maxLineLength;
    private Color[] charColors;
    private const char empty = ' ';

    public TextGrid(int rows, int columns)
    {
        grid = new char[columns * rows];
        this.columns = columns;
        this.rows = rows;
        for (int i = 0; i < columns * rows; i++)
        {
            grid[i] = empty;
        }
        setupColors();
        setupTexts();
        setupButtons();
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
        setupColors();
        setupTexts();
        setupButtons();
    }

    private void setupTexts()
    {
        texts = new List<Text>();
    }

    private void setupColors()
    {
        charColors = new Color[columns * rows];
        for (int i = 0; i < columns * rows; i++)
        {
            charColors[i] = Color.Transparent; //default colour
        }
    }

    private void setupButtons()
    {
        buttons = new List<Button>();
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
    /// sets a new color for a char in the grid.
    /// This shouldn`t be used to color the whole screen!! It will lead to huge performance issues.
    /// Use it just for partly coloring.
    /// </summary>
    /// <param name="x">rows</param>
    /// <param name="y">colums</param>
    /// <param name="color">the new colour (transparent is default)</param>
    public void setCharColor(int x, int y, Color color)
    {
        charColors[gridIndex(x, y)] = color;
    }

    public Color getCharColor(int x, int y)
    {
        return charColors[gridIndex(x, y)];
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
        }
        catch (ArgumentOutOfRangeException) { }
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
        // use String Builder
        string strGrid = $"";
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
        string strColumn = " ";
        for (int x = 0; x < rows; x++)
        {
            string color = colorToString(charColors[gridIndex(x, y)]);
            if (!color.Equals(colorToString(Color.Transparent)))
            {
                strColumn += "<color=" + color + ">" + getChar(x, y) + "</color>";
            }
            else
            {
                strColumn += getChar(x, y);
            }
        }
        return strColumn;
    }

    private string colorToString(Color color)
    {
        string str = color.ToString();
        str = str.Substring(7); //cause of "Color ["
        str = str.Remove(str.Length - 1); //to remove ]
        return str.ToLower();
    }

    /// <summary>
    /// Expands all lines and creates a new one if neccessary. (Only creates one at the time)
    /// </summary>
    /// <param name="maxLines">maximum amount of lines in a single grid</param>
    /// <param name="maxLength">maximum length of a line</param>
    public void expandLines()
    {
        for (int i = 0; i < lines.Length; i++)
        {
            if (!lines[i].expand())
            {
                lines[i] = createLine();
            }
        }
    }

    public void updateGrid(float glitchPercentageText)
    {
        clearGrid();
        // Lines
        expandLines();
        for (int i = 0; i < lines.Length; i++)
        {
            for (int y = 0; y < lines[i].getLength() - 1; y++)
            {
                setChar(lines[i].getStartPos().X, lines[i].getStartPos().Y + y, lines[i].getValue(y));
            }
        }
        // Buttons
        foreach (Button curr in buttons)
        {
            curr.updateButton();
            int width = curr.getSize().Width;
            int height = curr.getSize().Height;
            // iteterates throw the field where the button should be placed
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    if (!curr.getValue(x, y).Equals(empty))
                        setCharColor(curr.getTopLeftAnchor().X + x, curr.getTopLeftAnchor().Y + y, curr.getColor());
                    setChar(curr.getTopLeftAnchor().X + x, curr.getTopLeftAnchor().Y + y, curr.getValue(x, y));
                }
            }
        }
        // Texts
        foreach (Text curr in texts)
        {
            curr.updateText(glitchPercentageText);

            //draw values
            for (int x = 0; x < curr.getValues().Length; x++)
            {
                setChar(curr.getStartPoint().X + x, curr.getStartPoint().Y, curr.getValues()[x]);
                setCharColor(curr.getStartPoint().X + x, curr.getStartPoint().Y, curr.getColor());
            }
        }
    }

    /// <summary>
    /// creates the first set of lines and sets up the parameters for all following lines
    /// </summary>
    /// <param name="lineCount">The number of lines which are displayed in one frame.</param>
    /// <param name="maxLength">The posible maximum length of a line.</param>
    public void setupLines(int lineCount, int maxLength)
    {
        this.lineCount = lineCount;
        this.maxLineLength = maxLength;
        lines = new Line[lineCount];
        for (int i = 0; i < lineCount; i++)
        {
            lines[i] = createLine();
        }
    }

    /// <summary>
    /// Creates a new Text that will be displayed in the grid.
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="text"></param>
    /// <param name="color"></param>
    public void addText(int x, int y, string text, Color color)
    {
        Text nText = new Text(x, y, text, color);
        texts.Add(nText);
    }

    public void addButton(string text, int textDelta, Size size, Point topLeftAnchor, Color color)
    {
        Button nButton = new Button(text, textDelta, size, topLeftAnchor, color);
        buttons.Add(nButton);
        texts.Add(nButton.getText());
    }

    Random rng = new Random();
    private Line createLine()
    {
        int minLength = (int)(0.1f * maxLineLength);
        return new Line(rng.Next(rows), rng.Next(columns), rng.Next(minLength, maxLineLength));
    }

    private void clearGrid()
    {
        for (int x = 0; x < rows; x++)
            for (int y = 0; y < columns; y++)
                setChar(x, y, empty);
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
    /// Lines that are build for using them as a vertical rain animation.
    /// </summary>
    private class Line
    {
        private bool random;
        private char[] value;
        private Point startPoint;
        private int length;
        private int maxLength;
        private bool reachedMax = false;

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
            Random rnd = new Random();
            //shrinks or expands
            if (reachedMax)
            {
                length--;
                startPoint.Y++;
            }
            else
            {
                length++;
            }
            if (length >= maxLength)
                reachedMax = true;

            if (length <= 0 && reachedMax)
                return false;

            // randomizes the values of the line
            if (random)
            {
                for (int i = 0; i < maxLength; i++)
                {
                    value[i] = (char)rnd.Next(48, 58);
                }
            }
            return true;
        }
    }

    private class Text
    {
        private Point startPoint;
        private string text;
        private Color color;
        private char[] values;
        private int length;

        public Text(int x, int y, string text, Color color)
        {
            setStartPoint(new Point(x, y));
            setText(text);
            setColor(color);
        }

        public void setStartPoint(Point startPoint)
        {
            this.startPoint = startPoint;
        }

        public Point getStartPoint()
        {
            return startPoint;
        }

        public void setText(string text)
        {
            this.text = text.ToUpper();
            this.length = text.Length;
            this.values = new char[length];
        }

        public string getText()
        {
            return text;
        }

        public void setColor(Color color)
        {
            this.color = color;
        }

        public Color getColor()
        {
            return color;
        }

        public int getLength()
        {
            return length;
        }

        public char[] getValues()
        {
            return values;
        }

        Random rng = new Random();
        public void updateText(float wrongCharPercentage)
        {
            // creates glitches
            if (0 > wrongCharPercentage || wrongCharPercentage > 1)
            {
                throw new ArgumentOutOfRangeException(wrongCharPercentage + "is not between 0 and 1.");
            }
            int glitchCount = (int)(length * wrongCharPercentage);
            int[] glitches = new int[glitchCount];

            for (int i = 0; i < glitchCount; i++)
            {
                glitches[i] = rng.Next(length);
            }

            // insert glitches into the values
            values = text.ToCharArray();
            for (int i = 0; i < glitchCount; i++)
            {
                if (values[glitches[i]].Equals(empty))
                    continue;
                values[glitches[i]] = (char)(48 + rng.Next(9));
            }
        }
    }

    private class Button
    {
        private Text text;
        private Size size;
        private Point topLeftAnchor;
        private Color color;
        // starts at the top left and goes clockwise around
        private char[] values;
        private const int borderSize = 1;
        // for every rng aspect of the object
        Random rng = new Random();

        public Button(string text, int textDelta, Size size, Point topLeftAnchor, Color color)
        {
            setSize(size.Width, size.Height);
            setColor(color);
            setTopLeftAnchor(topLeftAnchor.X, topLeftAnchor.Y);
            setText(textDelta, text);
            setupValues();
        }

        public void setText(int delta, string text)
        {
            // check if text is too long
            if (text.Length > (size.Width - (delta + borderSize) * 2))
                throw new ArgumentOutOfRangeException("The text should have a length between 0 and " + (size.Width - (delta + borderSize) * 2));

            int xText = ((size.Width - text.Length) / 2) + topLeftAnchor.X;
            int yText = (size.Height / 2) + topLeftAnchor.Y;
            this.text = new Text(xText, yText, text, color);
        }

        public Text getText()
        {
            return text;
        }

        public void setTopLeftAnchor(int x, int y)
        {
            this.topLeftAnchor = new Point(x, y);
        }

        public Point getTopLeftAnchor()
        {
            return topLeftAnchor;
        }

        public void setSize(int width, int height)
        {
            this.size = new Size(width, height);
        }

        public Size getSize()
        {
            return size;
        }

        public void setColor(Color color)
        {
            this.color = color;
        }

        public Color getColor()
        {
            return color;
        }

        private void setValue(int x, int y, char c)
        {
            values[y * size.Width + x] = c;
        }

        public char getValue(int x, int y)
        {
            if ((x >= size.Width || x < 0) || (y >= size.Height || y < 0))
                throw new ArgumentOutOfRangeException("You are trying to get access to a value outside of a button.");
            return values[y * size.Width + x];
        }

        public void updateButton()
        {
            for (int x = 0; x < size.Width; x++)
            {
                for (int y = 0; y < size.Height; y++)
                {
                    if (x == 0 || y == 0 || x == (size.Width - 1) || y == (size.Height - 1))
                    {
                        setValue(x, y, (char)(48 + rng.Next(9)));
                    }
                    else
                    {
                        setValue(x, y, empty);
                    }
                }
            }
        }

        private void setupValues()
        {
            values = new char[size.Width * size.Height];
            updateButton();
        }
    }
}
