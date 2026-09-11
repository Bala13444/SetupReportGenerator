using SetupReportGenerator.ViewModels;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;

namespace SetupReportGenerator
{
    public partial class Form1 : Form
    {
        private readonly MainViewModel viewModel;

        // ---------------------------------------------------------------
        // Premium theme palette (matches the report's dark-blue theme)
        // ---------------------------------------------------------------
        private static readonly Color ColorDarkBlue = ColorTranslator.FromHtml("#0A2F52");
        private static readonly Color ColorMidBlue = ColorTranslator.FromHtml("#0B5394");
        private static readonly Color ColorBrightBlue = ColorTranslator.FromHtml("#1E7FC2");
        private static readonly Color ColorLightBlue = ColorTranslator.FromHtml("#EAF2FA");
        private static readonly Color ColorBackground = ColorTranslator.FromHtml("#EEF2F6");
        private static readonly Color ColorGreen = ColorTranslator.FromHtml("#1E8F5A");
        private static readonly Color ColorGreenHover = ColorTranslator.FromHtml("#146641");
        private static readonly Color ColorGold = ColorTranslator.FromHtml("#FFE9A6");
        private static readonly Color ColorCardBorder = ColorTranslator.FromHtml("#D7E2EC");
        private static readonly Color ColorMuted = ColorTranslator.FromHtml("#5A7085");
        private static readonly Color ColorCancel = ColorTranslator.FromHtml("#B3B9C0");
        private static readonly Color ColorCancelHover = ColorTranslator.FromHtml("#8A9199");

        // Runtime-built cards / chrome
        private RoundedPanel toolbarCard;
        private RoundedPanel gridCard;
        private Label lblGridTitle;

        // Grid auto-size limits
        private const int GRID_ROW_HEIGHT = 30;
        private const int GRID_MIN_HEIGHT = 160;
        private const int GRID_MAX_HEIGHT = 480;

        // Top row (title + action buttons) geometry inside the grid card
        private const int TOP_ROW_Y = 12;
        private const int TOP_ROW_HEIGHT = 34;
        private const int GRID_TOP_Y = TOP_ROW_Y + TOP_ROW_HEIGHT + 14; // grid starts below the button row

        public Form1()
        {
            InitializeComponent();

            viewModel = new MainViewModel();
            ApplyPremiumTheme();
            BindGrid();
            WireViewModelEvents();
        }

        // ---------------------------------------------------------------
        // Grid binds once, directly, to the ViewModel's BindingList - no
        // more manual "dgvRecipes.DataSource = setupDetails" reassignment
        // on every load/save.
        // ---------------------------------------------------------------
        private void BindGrid()
        {
            var dgvRecipes = FindControl<DataGridView>("dgvRecipes");
            if (dgvRecipes != null)
            {
                dgvRecipes.DataSource = viewModel.SetupDetails;
            }
        }

        private void WireViewModelEvents()
        {
            viewModel.RequestOpenXmlDialog += _ =>
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = "XML Files (*.xml)|*.xml";
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    viewModel.LoadXmlFile(openFileDialog.FileName);

                    var txtXmlPath = FindControl<TextBox>("txtXmlPath");
                    if (txtXmlPath != null) txtXmlPath.Text = openFileDialog.FileName;

                    var dgvRecipes = FindControl<DataGridView>("dgvRecipes");
                    if (dgvRecipes != null) AutoSizeGridToContent(dgvRecipes);

                    UpdateGridTitle();
                }
            };

            viewModel.ShowInfoMessage += message =>
                MessageBox.Show(message, "Setup Report Generator", MessageBoxButtons.OK, MessageBoxIcon.Information);

            viewModel.DataSaved += () =>
            {
                var txtXmlPath = FindControl<TextBox>("txtXmlPath");
                txtXmlPath?.Clear();

                var dgvRecipes = FindControl<DataGridView>("dgvRecipes");
                if (dgvRecipes != null) AutoSizeGridToContent(dgvRecipes);

                UpdateGridTitle();
            };

            viewModel.RequestShowReportViewer += () =>
            {
                SetupReportViewerForm reportForm = new SetupReportViewerForm();
                reportForm.Show();
            };
        }

        // ---- Designer-wired event handlers: one-line delegations to the ViewModel ----

        private void btnBrowse_Click(object sender, EventArgs e)
            => viewModel.BrowseCommand.Execute();

        private void btnSave_Click(object sender, EventArgs e)
            => viewModel.SaveCommand.Execute();

        private void btnReport_Click(object sender, EventArgs e)
            => viewModel.ViewReportCommand.Execute();

        // =================================================================
        // PREMIUM THEME - runs once at startup. Rebuilds the layout into
        // report-style cards without touching the Designer file: existing
        // controls are re-parented into runtime panels, so their Name/
        // event wiring from the Designer keeps working untouched.
        // =================================================================

        private void ApplyPremiumTheme()
        {
            this.BackColor = ColorBackground;
            this.Font = new Font("Segoe UI", 9.5f);
            this.MinimumSize = new Size(900, 560);

            var header = BuildHeaderBar();
            this.Controls.Add(header);

            BuildToolbarCard();
            BuildGridCard();

            LayoutCards();
            this.Resize += (s, e) => LayoutCards();

            var dgvRecipes = FindControl<DataGridView>("dgvRecipes");
            if (dgvRecipes != null)
            {
                AutoSizeGridToContent(dgvRecipes);
                dgvRecipes.RowsAdded += (s, e) => AutoSizeGridToContent(dgvRecipes);
                dgvRecipes.RowsRemoved += (s, e) => AutoSizeGridToContent(dgvRecipes);
                dgvRecipes.DataBindingComplete += (s, e) =>
                {
                    ConfigureGridColumns(dgvRecipes);
                    AutoSizeGridToContent(dgvRecipes);
                };
                dgvRecipes.CellFormatting += Grid_CellFormatting;
            }

            header.BringToFront();

            // Final safety-net layout pass once the form has fully loaded
            // and its real size is known - guarantees all three action
            // buttons (Cancel / Save to Database / View Report) end up in
            // their correct top-right position.
            this.Shown += (s, e) => PositionGridActionButtons();
        }

        // ---------------------------------------------------------------
        // Header bar (matches the report header: logo chip + title block)
        // ---------------------------------------------------------------
        private GradientPanel BuildHeaderBar()
        {
            var header = new GradientPanel
            {
                Dock = DockStyle.Top,
                Height = 68,
                StartColor = ColorMidBlue,
                EndColor = ColorDarkBlue,
                Angle = 0f
            };

            var logoChip = new RoundedPanel
            {
                Size = new Size(44, 44),
                Location = new Point(20, 12),
                BackColor = Color.White,
                CornerRadius = 10,
                BackgroundOverride = Color.White
            };
            var logoText = new Label
            {
                Text = "ASMPT",
                ForeColor = ColorMidBlue,
                Font = new Font("Segoe UI", 7.5f, FontStyle.Bold),
                AutoSize = false,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter
            };
            logoChip.Controls.Add(logoText);

            var title = new Label
            {
                Text = "SIPLACE SETUP REPORT",
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 14f, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(76, 10),
                BackColor = Color.Transparent
            };

            var subtitle = new Label
            {
                Text = "ASMPT India Private Limited  •  Recipe Importer",
                ForeColor = Color.FromArgb(210, 226, 242),
                Font = new Font("Segoe UI", 8.75f),
                AutoSize = true,
                Location = new Point(78, 36),
                BackColor = Color.Transparent
            };

            header.Controls.Add(logoChip);
            header.Controls.Add(title);
            header.Controls.Add(subtitle);

            return header;
        }

        // ---------------------------------------------------------------
        // Toolbar card: "XML File" label + textbox + Browse button
        // ---------------------------------------------------------------
        private void BuildToolbarCard()
        {
            toolbarCard = new RoundedPanel
            {
                BackColor = Color.White,
                CornerRadius = 12,
                BorderColor = ColorCardBorder,
                Padding = new Padding(20, 16, 20, 16)
            };
            this.Controls.Add(toolbarCard);

            var lbl = FindControl<Label>("lblXmlFile") ?? FindControlByTextHint<Label>("XML File");
            var txt = FindControl<TextBox>("txtXmlPath");
            var btn = FindControl<Button>("btnBrowse");

            if (lbl != null) Reparent(lbl, toolbarCard);
            if (txt != null) Reparent(txt, toolbarCard);
            if (btn != null) Reparent(btn, toolbarCard);

            if (lbl != null)
            {
                lbl.Location = new Point(20, 24);
                lbl.ForeColor = ColorDarkBlue;
                lbl.Font = new Font("Segoe UI", 9.5f, FontStyle.Bold);
                lbl.AutoSize = true;
            }
            if (txt != null)
            {
                txt.Location = new Point(96, 20);
                txt.Width = 480;
                txt.Height = 28;
                StyleTextBox(txt);
                txt.ReadOnly = true;
            }
            if (btn != null)
            {
                btn.Location = new Point(96 + (txt?.Width ?? 480) + 12, 18);
                btn.Size = new Size(120, 32);
                btn.Text = "Browse";
                StylePillButton(btn, ColorBrightBlue, ColorMidBlue);
            }

            toolbarCard.Height = 68;
        }

        // ---------------------------------------------------------------
        // Grid card: title + action buttons (top-right) + editable,
        // auto-sizing grid below them.
        // ---------------------------------------------------------------
        private void BuildGridCard()
        {
            gridCard = new RoundedPanel
            {
                BackColor = Color.White,
                CornerRadius = 12,
                BorderColor = ColorCardBorder,
                Padding = new Padding(20, 16, 20, 16)
            };
            this.Controls.Add(gridCard);

            lblGridTitle = new Label
            {
                Text = "Loaded Recipe Data",
                ForeColor = ColorDarkBlue,
                Font = new Font("Segoe UI", 11f, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(20, TOP_ROW_Y + 4)
            };
            gridCard.Controls.Add(lblGridTitle);

            var grid = FindControl<DataGridView>("dgvRecipes");
            if (grid != null)
            {
                Reparent(grid, gridCard);
                grid.Location = new Point(20, GRID_TOP_Y);
                StyleGrid(grid);
                grid.ReadOnly = false; // editable
                grid.EditMode = DataGridViewEditMode.EditOnKeystrokeOrF2;
                grid.AllowUserToAddRows = false; // keep off: rows map to DB inserts
                grid.AllowUserToDeleteRows = false;
            }

            var btnSave = FindControl<Button>("btnSave");
            var btnReport = FindControl<Button>("btnReport");

            if (btnSave != null)
            {
                Reparent(btnSave, gridCard);
                btnSave.Size = new Size(140, TOP_ROW_HEIGHT);
                btnSave.Text = "Save to Database";
                StylePillButton(btnSave, ColorGreen, ColorGreenHover);
            }

            // View Report may or may not exist in the Designer file - create
            // it here if it's missing, so it's guaranteed to show up.
            if (btnReport == null)
            {
                btnReport = new Button { Name = "btnReport", Text = "View Report" };
                btnReport.Click += btnReport_Click;
                gridCard.Controls.Add(btnReport);
            }
            else
            {
                Reparent(btnReport, gridCard);
            }
            btnReport.Size = new Size(140, TOP_ROW_HEIGHT);
            btnReport.Text = "View Report";
            StylePillButton(btnReport, ColorMidBlue, ColorDarkBlue);
            btnReport.Visible = true;

            // Cancel button doesn't exist in the Designer, so it's created here.
            var btnCancel = FindControl<Button>("btnCancel");
            if (btnCancel == null)
            {
                btnCancel = new Button { Name = "btnCancel", Text = "Cancel" };
                btnCancel.Click += (s, e) => this.Close();
                gridCard.Controls.Add(btnCancel);
            }
            btnCancel.Size = new Size(100, TOP_ROW_HEIGHT);
            StylePillButton(btnCancel, ColorCancel, ColorCancelHover);

            PositionGridActionButtons();
        }

        private void UpdateGridTitle()
        {
            if (lblGridTitle == null) return;
            lblGridTitle.Text = viewModel.SetupDetails.Count > 0
                ? $"Loaded Recipe Data  ({viewModel.SetupDetails.Count} rows)"
                : "Loaded Recipe Data";
        }

        // ---------------------------------------------------------------
        // Layout: stacks header -> toolbar card -> grid card, and keeps
        // everything responsive as the form / grid content resizes.
        // ---------------------------------------------------------------
        private void LayoutCards()
        {
            if (toolbarCard == null || gridCard == null) return;

            int margin = 20;
            int headerHeight = 68;
            int top = headerHeight + margin;
            int contentWidth = this.ClientSize.Width - margin * 2;

            toolbarCard.Location = new Point(margin, top);
            toolbarCard.Width = contentWidth;
            top += toolbarCard.Height + margin;

            gridCard.Location = new Point(margin, top);
            gridCard.Width = contentWidth;

            PositionGridActionButtons();

            var grid = FindControl<DataGridView>("dgvRecipes");
            if (grid != null)
            {
                grid.Width = gridCard.Width - 40; // padding L+R
            }

            UpdateGridCardHeight();
        }

        // ---------------------------------------------------------------
        // Positions Cancel / Save to Database / View Report at the
        // TOP-RIGHT of the grid card, on the same row as the "Loaded
        // Recipe Data" title, instead of below the grid.
        // ---------------------------------------------------------------
        private void PositionGridActionButtons()
        {
            if (gridCard == null) return;

            var btnSave = FindControl<Button>("btnSave");
            var btnReport = FindControl<Button>("btnReport");
            var btnCancel = FindControl<Button>("btnCancel");
            int rightMargin = 20;

            int cursorRight = gridCard.Width - rightMargin;

            if (btnReport != null && btnReport.Visible)
            {
                btnReport.Location = new Point(cursorRight - btnReport.Width, TOP_ROW_Y);
                cursorRight = btnReport.Left - 12;
            }
            if (btnSave != null)
            {
                btnSave.Location = new Point(cursorRight - btnSave.Width, TOP_ROW_Y);
                cursorRight = btnSave.Left - 12;
            }
            if (btnCancel != null)
            {
                btnCancel.Location = new Point(cursorRight - btnCancel.Width, TOP_ROW_Y);
            }
        }

        // ---------------------------------------------------------------
        // Grid card height now simply wraps the grid content (buttons
        // live in the fixed top row, not below the grid anymore).
        // ---------------------------------------------------------------
        private void UpdateGridCardHeight()
        {
            if (gridCard == null) return;

            var grid = FindControl<DataGridView>("dgvRecipes");
            int gridBottom = grid != null ? grid.Bottom : GRID_TOP_Y;

            gridCard.Height = gridBottom + 20;
        }

        // ---------------------------------------------------------------
        // Auto-size the grid's height to fit its current row count,
        // between GRID_MIN_HEIGHT and GRID_MAX_HEIGHT (scrolls beyond).
        // ---------------------------------------------------------------
        private void AutoSizeGridToContent(DataGridView grid)
        {
            if (grid == null) return;

            int rowCount = grid.Rows.Count;
            int headerHeight = grid.ColumnHeadersVisible ? grid.ColumnHeadersHeight : 0;
            int desired = headerHeight + Math.Max(rowCount, 1) * GRID_ROW_HEIGHT + 4;

            grid.Height = Math.Max(GRID_MIN_HEIGHT, Math.Min(GRID_MAX_HEIGHT, desired));
            grid.ScrollBars = desired > GRID_MAX_HEIGHT ? ScrollBars.Vertical : ScrollBars.None;

            UpdateGridCardHeight();
        }

        // ---------------------------------------------------------------
        // Styling helpers
        // ---------------------------------------------------------------
        private void StyleTextBox(TextBox txt)
        {
            txt.BorderStyle = BorderStyle.FixedSingle;
            txt.BackColor = ColorLightBlue;
            txt.ForeColor = ColorDarkBlue;
            txt.Font = new Font("Segoe UI", 9.5f);
        }

        private void StylePillButton(Button btn, Color baseColor, Color hoverColor)
        {
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.Cursor = Cursors.Hand;
            btn.Font = new Font("Segoe UI", 9.5f, FontStyle.Bold);
            btn.ForeColor = Color.White;
            btn.BackColor = baseColor;
            btn.MouseEnter -= ButtonHoverEnter;
            btn.MouseLeave -= ButtonHoverLeave;
            btn.Tag = new Tuple<Color, Color>(baseColor, hoverColor);
            btn.MouseEnter += ButtonHoverEnter;
            btn.MouseLeave += ButtonHoverLeave;

            ApplyRoundedCorners(btn, btn.Height / 2); // pill shape
        }

        private void ButtonHoverEnter(object sender, EventArgs e)
        {
            if (sender is Button btn && btn.Tag is Tuple<Color, Color> colors)
                btn.BackColor = colors.Item2;
        }

        private void ButtonHoverLeave(object sender, EventArgs e)
        {
            if (sender is Button btn && btn.Tag is Tuple<Color, Color> colors)
                btn.BackColor = colors.Item1;
        }

        private void StyleGrid(DataGridView grid)
        {
            grid.BorderStyle = BorderStyle.None;
            grid.BackgroundColor = Color.White;
            grid.GridColor = ColorCardBorder;

            grid.EnableHeadersVisualStyles = false;
            grid.ColumnHeadersDefaultCellStyle.BackColor = ColorMidBlue;
            grid.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            grid.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9.5f, FontStyle.Bold);
            grid.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            grid.ColumnHeadersDefaultCellStyle.Padding = new Padding(8, 0, 0, 0);
            grid.ColumnHeadersHeight = 36;
            grid.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            grid.RowHeadersVisible = false;

            grid.RowsDefaultCellStyle.BackColor = Color.White;
            grid.AlternatingRowsDefaultCellStyle.BackColor = ColorLightBlue;
            grid.DefaultCellStyle.SelectionBackColor = ColorGold;
            grid.DefaultCellStyle.SelectionForeColor = Color.Black;
            grid.DefaultCellStyle.Font = new Font("Segoe UI", 9.5f);
            grid.DefaultCellStyle.Padding = new Padding(8, 4, 4, 4);

            grid.RowTemplate.Height = GRID_ROW_HEIGHT;
            grid.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            grid.AllowUserToResizeRows = false;
            grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            grid.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        }

        // ---------------------------------------------------------------
        // Inserts a leading, unbound "S.No" column (1, 2, 3...) and hides
        // SetupDetailId / RecipeId, which are always 0 until the row is
        // actually saved to the database.
        // ---------------------------------------------------------------
        private void ConfigureGridColumns(DataGridView grid)
        {
            if (grid == null) return;

            if (grid.Columns.Contains("SetupDetailId"))
                grid.Columns["SetupDetailId"].Visible = false;
            if (grid.Columns.Contains("RecipeId"))
                grid.Columns["RecipeId"].Visible = false;

            if (!grid.Columns.Contains("colSNo"))
            {
                var snoCol = new DataGridViewTextBoxColumn
                {
                    Name = "colSNo",
                    HeaderText = "S.No",
                    ReadOnly = true,
                    SortMode = DataGridViewColumnSortMode.NotSortable,
                    AutoSizeMode = DataGridViewAutoSizeColumnMode.None,
                    Width = 60,
                    MinimumWidth = 60
                };
                snoCol.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                snoCol.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                grid.Columns.Insert(0, snoCol);
            }

            grid.Columns["colSNo"].DisplayIndex = 0;
        }

        // ---------------------------------------------------------------
        // Fills the unbound "S.No" column with the current row number.
        // ---------------------------------------------------------------
        private void Grid_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            var grid = sender as DataGridView;
            if (grid == null) return;
            if (e.ColumnIndex < 0 || e.ColumnIndex >= grid.Columns.Count) return;

            if (grid.Columns[e.ColumnIndex].Name == "colSNo")
            {
                e.Value = (e.RowIndex + 1).ToString();
                e.FormattingApplied = true;
            }
        }

        private void ApplyRoundedCorners(Control ctrl, int radius)
        {
            void SetRegion()
            {
                if (ctrl.Width > 0 && ctrl.Height > 0)
                {
                    ctrl.Region = new Region(RoundedRect(ctrl.ClientRectangle, radius));
                }
            }

            ctrl.Resize += (s, e) => SetRegion();
            SetRegion();
        }

        private static GraphicsPath RoundedRect(Rectangle bounds, int radius)
        {
            int d = radius * 2;
            d = Math.Max(2, Math.Min(d, Math.Min(bounds.Width, bounds.Height)));
            var path = new GraphicsPath();
            path.AddArc(bounds.X, bounds.Y, d, d, 180, 90);
            path.AddArc(bounds.Right - d, bounds.Y, d, d, 270, 90);
            path.AddArc(bounds.Right - d, bounds.Bottom - d, d, d, 0, 90);
            path.AddArc(bounds.X, bounds.Bottom - d, d, d, 90, 90);
            path.CloseFigure();
            return path;
        }

        // ---------------------------------------------------------------
        // Small reflection-free helpers for finding / moving Designer
        // controls without ever editing the Designer file.
        // ---------------------------------------------------------------
        private T FindControl<T>(string name) where T : Control
        {
            var found = this.Controls.Find(name, true);
            return found.Length > 0 ? found[0] as T : null;
        }

        private T FindControlByTextHint<T>(string textContains) where T : Control
        {
            return AllControls(this).OfType<T>()
                .FirstOrDefault(c => c.Text != null &&
                    c.Text.IndexOf(textContains, StringComparison.OrdinalIgnoreCase) >= 0);
        }

        private IEnumerable<Control> AllControls(Control root)
        {
            foreach (Control c in root.Controls)
            {
                yield return c;
                foreach (var sub in AllControls(c)) yield return sub;
            }
        }

        private void Reparent(Control ctrl, Control newParent)
        {
            var oldParent = ctrl.Parent;
            oldParent?.Controls.Remove(ctrl);
            newParent.Controls.Add(ctrl);
        }
    }

    /// <summary>
    /// A Panel that paints a diagonal gradient background. Used for the
    /// premium header bar.
    /// </summary>
    public class GradientPanel : Panel
    {
        public Color StartColor { get; set; } = Color.FromArgb(10, 47, 82);
        public Color EndColor { get; set; } = Color.FromArgb(30, 127, 194);
        public float Angle { get; set; } = 120f;

        public GradientPanel()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint
                    | ControlStyles.UserPaint
                    | ControlStyles.OptimizedDoubleBuffer
                    | ControlStyles.ResizeRedraw, true);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (Width > 0 && Height > 0)
            {
                using (var brush = new LinearGradientBrush(ClientRectangle, StartColor, EndColor, Angle))
                {
                    e.Graphics.FillRectangle(brush, ClientRectangle);
                }
            }
            base.OnPaint(e);
        }
    }

    /// <summary>
    /// A Panel with rounded corners and a thin border, used to build the
    /// white "card" sections that mirror the report viewer's look.
    /// </summary>
    public class RoundedPanel : Panel
    {
        public int CornerRadius { get; set; } = 10;
        public Color BorderColor { get; set; } = Color.FromArgb(215, 226, 236);
        public Color? BackgroundOverride { get; set; } = null;

        public RoundedPanel()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint
                    | ControlStyles.UserPaint
                    | ControlStyles.OptimizedDoubleBuffer
                    | ControlStyles.ResizeRedraw, true);
            this.Resize += (s, e) => ApplyRegion();
        }

        private void ApplyRegion()
        {
            if (Width <= 0 || Height <= 0) return;
            int d = CornerRadius * 2;
            d = Math.Max(2, Math.Min(d, Math.Min(Width, Height)));
            var rect = new Rectangle(0, 0, Width, Height);
            var path = new GraphicsPath();
            path.AddArc(rect.X, rect.Y, d, d, 180, 90);
            path.AddArc(rect.Right - d, rect.Y, d, d, 270, 90);
            path.AddArc(rect.Right - d, rect.Bottom - d, d, d, 0, 90);
            path.AddArc(rect.X, rect.Bottom - d, d, d, 90, 90);
            path.CloseFigure();
            this.Region = new Region(path);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            using (var bg = new SolidBrush(BackgroundOverride ?? this.BackColor))
            {
                e.Graphics.FillRectangle(bg, this.ClientRectangle);
            }

            int d = CornerRadius * 2;
            d = Math.Max(2, Math.Min(d, Math.Min(Width, Height)));
            var rect = new Rectangle(0, 0, Width - 1, Height - 1);
            using (var path = new GraphicsPath())
            using (var pen = new Pen(BorderColor, 1))
            {
                path.AddArc(rect.X, rect.Y, d, d, 180, 90);
                path.AddArc(rect.Right - d, rect.Y, d, d, 270, 90);
                path.AddArc(rect.Right - d, rect.Bottom - d, d, d, 0, 90);
                path.AddArc(rect.X, rect.Bottom - d, d, d, 90, 90);
                path.CloseFigure();
                e.Graphics.DrawPath(pen, path);
            }
        }

        protected override void OnResize(EventArgs eventargs)
        {
            base.OnResize(eventargs);
            ApplyRegion();
        }
    }
}
