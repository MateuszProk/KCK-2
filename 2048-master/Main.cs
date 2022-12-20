using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Win32;

namespace u2048 {

  public partial class Main : Form {

    private readonly string currentFileLocation;
    private readonly Game game;
    private bool musicPrevState = true;
    private bool kTop, kRight, kBottom, kLeft;
    
    public Main() {
      InitializeComponent();

      var asm = Assembly.GetExecutingAssembly();

      currentFileLocation = asm.Location;
      Text = $"2048";
      Icon = Icon.ExtractAssociatedIcon(currentFileLocation);

      FormBorderStyle = FormBorderStyle.FixedDialog;
      MaximizeBox = false;
      StartPosition = FormStartPosition.CenterScreen;
      Activate();

      game = new Game();

      var boardSize = 4;
      var lastScore = 0;
      int[][] boardData = null;
      var key = Registry.CurrentUser.CreateSubKey("SOFTWARE\\ada\\2048");
      if (key != null) {
        boardSize = (int) key.GetValue("size", 5);
        game.Best = (int) key.GetValue("best", 0);
        lastScore = (int) key.GetValue("last", 0);
        boardData = BoardDataFromString((string)key.GetValue("lastGame", null));
      }

      this.Closed += (s, e) => {
        if (key != null) {
          key.SetValue("size", game.BoardSize);
          key.SetValue("best", game.Best);
          key.SetValue("last", game.Score);
          key.SetValue("lastGame", BoardDataToString(game.BoardSize, game.Board));
        }
      };

      this.DoubleBuffered = true;
      this.Paint += (s, e) => {
        game.Update();
        using (var canvas = new Bitmap(ClientRectangle.Width, ClientRectangle.Height)) {
          using (var g = Graphics.FromImage(canvas)) {
            g.Clear(BackColor);
            // g.Clear(Color.FromArgb(253, 250, 231));
            game.Draw(g);
            e.Graphics.DrawImage(canvas, new Point(0, 0));
          }
        }
      };

      this.Deactivate += (s, e) => {
        musicPrevState = game.MusicEnabled;
        game.MusicEnabled = false;
        Invalidate();
      };

      this.Activated += (s, e) => {
        game.MusicEnabled = musicPrevState;
        Invalidate();
      };

      StartNewGame((byte) boardSize, lastScore, boardData);

    }

    private string BoardDataToString(int size, int[][] data) {
      var result = "";
      for (var i = 0; i < size; i++) {
        if (i > 0)
          result += "\n";
        for (var j = 0; j < size; j++) {
          if (j > 0) result += ";";
          result += data[i][j].ToString();
        }
      }
      return result;
    }
    
    private int[][] BoardDataFromString(string value) {
      
      if (string.IsNullOrEmpty(value)) return null;

      var parsed = value.Split('\n');
      var result = new int[parsed.Length][];
      for (var i = 0; i < parsed.Length; i++) {
        var items = parsed[i].Split(';');
        result[i] = new int[items.Length];
        for (var j = 0; j < items.Length; j++) {
          result[i][j] = int.Parse(items[j]);
        }
      }

      return result;
    }
    
    private void StartNewGame(byte size, int lastScore = 0, int[][] data = null) {
      Size = new Size(size * 88 + 50, size * 88 + 140);
      game.StartNewGame(size, lastScore, data);
      Invalidate();
    }

    #region main menu items
    
    private void exitToolStripMenuItem_Click(object sender, EventArgs e) {
      Close();
    }

    private void undoMoveToolStripMenuItem_Click(object sender, EventArgs e) {
      game.UndoMove();
      Invalidate();
    }

    private void newToolStripMenuItem_Click(object sender, EventArgs e) {
      game.StartNewGame(game.BoardSize);
    }

    private void new4x4GameToolStripMenuItem_Click(object sender, EventArgs e) {
      StartNewGame(4);
    }

    private void new5x5GameToolStripMenuItem_Click(object sender, EventArgs e) {
      StartNewGame(5);
    }

    private void new6x6GameToolStripMenuItem_Click(object sender, EventArgs e) {
      StartNewGame(6);
    }

    private void new7x7GameToolStripMenuItem_Click(object sender, EventArgs e) {
      StartNewGame(7);
    }

    private void new8x8GameToolStripMenuItem_Click(object sender, EventArgs e) {
      StartNewGame(8);
    }


    #endregion
    
    private void MoveBoard(Game.Direction direction) {
      game.MoveBoard(direction);
      Invalidate();
    }

        private void Options_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }

        private void MainForm_KeyDown(object sender, KeyEventArgs e) {
      if (!kTop && !kRight && !kBottom && (e.KeyCode == Keys.A || e.KeyCode == Keys.Left)) {
        kLeft = true;
        MoveBoard(Game.Direction.Left);
      }
      else if (!kLeft && !kRight && !kBottom && (e.KeyCode == Keys.W || e.KeyCode == Keys.Up)) {
        kTop = true;
        MoveBoard(Game.Direction.Top);
      }
      else if (!kTop && !kLeft && !kBottom && (e.KeyCode == Keys.D || e.KeyCode == Keys.Right)) {
        kRight = true;
        MoveBoard(Game.Direction.Right);
      }
      else if (!kTop && !kRight && !kLeft && (e.KeyCode == Keys.S || e.KeyCode == Keys.Down)) {
        kBottom = true;
        MoveBoard(Game.Direction.Bottom);
      }
    }

    private void MainForm_KeyUp(object sender, KeyEventArgs e) {
      if (kLeft && (e.KeyCode == Keys.A || e.KeyCode == Keys.Left)) {
        kLeft = false;
      }

      if (kTop && (e.KeyCode == Keys.W || e.KeyCode == Keys.Up)) {
        kTop = false;
      }

      if (kRight && (e.KeyCode == Keys.D || e.KeyCode == Keys.Right)) {
        kRight = false;
      }

      if (kBottom && (e.KeyCode == Keys.S || e.KeyCode == Keys.Down)) {
        kBottom = false;
      }
    }

    private void Main_MouseClick(object sender, MouseEventArgs e) {

        }
    }
}
