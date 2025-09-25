using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;
using TaskMaster.Interfaces;

namespace TaskMaster.UI
{
    public partial class MainForm : Form
    {
        private readonly ITaskService _taskService;
        private string placeholderText = "Lägg till en ny uppgift...";

        public MainForm(ITaskService taskService)
        {
            _taskService = taskService;
            InitializeComponent();
            SetUpEventHandlers();
            SetupPlaceholder();
            SetupButtonHoverEffects();
            SetupPanelShadows();
            SetupHeaderGradient();
            RefreshTaskList();
        }

        private void SetUpEventHandlers()
        {
            btnAdd.Click += BtnAdd_Click;
            btnComplete.Click += BtnComplete_Click;
            btnDelete.Click += BtnDelete_Click;
            txtNewTask.KeyPress += TxtNewTask_KeyPress;

            // Placeholder events
            txtNewTask.Enter += TxtNewTask_Enter;
            txtNewTask.Leave += TxtNewTask_Leave;
        }

        private void SetupPlaceholder()
        {
            txtNewTask.Text = placeholderText;
            txtNewTask.ForeColor = Color.Gray;
        }

        private void SetupHeaderGradient()
        {
            headerPanel.Paint += (sender, e) =>
            {
                using (LinearGradientBrush brush = new LinearGradientBrush(
                    headerPanel.ClientRectangle,
                    Color.FromArgb(52, 73, 94),
                    Color.FromArgb(44, 62, 80),
                    LinearGradientMode.Vertical))
                {
                    e.Graphics.FillRectangle(brush, headerPanel.ClientRectangle);
                }
            };
        }

        private void SetupPanelShadows()
        {
            // Lägg till skugga till alla paneler
            AddDropShadow(panel1);
            AddDropShadow(panel2);
            AddDropShadow(panel3);

            // Rundade hörn
            SetRoundedCorners(panel1, 8);
            SetRoundedCorners(panel2, 8);
            SetRoundedCorners(panel3, 8);
            SetRoundedCorners(btnAdd, 6);
            SetRoundedCorners(btnComplete, 6);
            SetRoundedCorners(btnDelete, 6);
        }

        private void AddDropShadow(Panel panel)
        {
            // Ta bort eventuella borders
            panel.BorderStyle = BorderStyle.None;

            panel.Paint += (sender, e) =>
            {
                // Använd anti-aliasing för mjukare rendering
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

                // Rita skugga
                using (SolidBrush shadowBrush = new SolidBrush(Color.FromArgb(30, 0, 0, 0)))
                {
                    Rectangle shadowRect = new Rectangle(3, 3, panel.Width - 3, panel.Height - 3);
                    e.Graphics.FillRectangle(shadowBrush, shadowRect);
                }

                // Rita panel med rundade hörn
                using (SolidBrush panelBrush = new SolidBrush(panel.BackColor))
                {
                    Rectangle panelRect = new Rectangle(0, 0, panel.Width - 3, panel.Height - 3);
                    e.Graphics.FillRectangle(panelBrush, panelRect);
                }
            };
        }

        private void SetRoundedCorners(Control control, int radius)
        {
            GraphicsPath path = new GraphicsPath();
            Rectangle rect = new Rectangle(0, 0, control.Width, control.Height);

            path.AddArc(rect.X, rect.Y, radius, radius, 180, 90);
            path.AddArc(rect.X + rect.Width - radius, rect.Y, radius, radius, 270, 90);
            path.AddArc(rect.X + rect.Width - radius, rect.Y + rect.Height - radius, radius, radius, 0, 90);
            path.AddArc(rect.X, rect.Y + rect.Height - radius, radius, radius, 90, 90);
            path.CloseAllFigures();

            control.Region = new Region(path);
        }

        private void SetupButtonHoverEffects()
        {
            // Add button hover
            btnAdd.MouseEnter += (s, e) =>
            {
                btnAdd.BackColor = Color.FromArgb(41, 128, 185);
                AddButtonGlow(btnAdd, Color.FromArgb(100, 52, 152, 219));
            };
            btnAdd.MouseLeave += (s, e) =>
            {
                btnAdd.BackColor = Color.FromArgb(52, 152, 219);
                RemoveButtonGlow(btnAdd);
            };

            // Complete button hover
            btnComplete.MouseEnter += (s, e) =>
            {
                btnComplete.BackColor = Color.FromArgb(39, 174, 96);
                AddButtonGlow(btnComplete, Color.FromArgb(100, 46, 204, 113));
            };
            btnComplete.MouseLeave += (s, e) =>
            {
                btnComplete.BackColor = Color.FromArgb(46, 204, 113);
                RemoveButtonGlow(btnComplete);
            };

            // Delete button hover
            btnDelete.MouseEnter += (s, e) =>
            {
                btnDelete.BackColor = Color.FromArgb(192, 57, 43);
                AddButtonGlow(btnDelete, Color.FromArgb(100, 231, 76, 60));
            };
            btnDelete.MouseLeave += (s, e) =>
            {
                btnDelete.BackColor = Color.FromArgb(231, 76, 60);
                RemoveButtonGlow(btnDelete);
            };
        }

        private void AddButtonGlow(Button button, Color glowColor)
        {
            button.FlatAppearance.BorderColor = glowColor;
            button.FlatAppearance.BorderSize = 2;
        }

        private void RemoveButtonGlow(Button button)
        {
            button.FlatAppearance.BorderSize = 0;
        }

        private void TxtNewTask_Enter(object sender, EventArgs e)
        {
            if (txtNewTask.Text == placeholderText)
            {
                txtNewTask.Text = "";
                txtNewTask.ForeColor = Color.FromArgb(64, 64, 64);
            }
        }

        private void TxtNewTask_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNewTask.Text))
            {
                txtNewTask.Text = placeholderText;
                txtNewTask.ForeColor = Color.Gray;
            }
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            string taskText = txtNewTask.Text == placeholderText ? "" : txtNewTask.Text;

            if (!string.IsNullOrWhiteSpace(taskText))
            {
                _taskService.AddTask(taskText);
                txtNewTask.Text = placeholderText;
                txtNewTask.ForeColor = Color.Gray;
                RefreshTaskList();
                ShowStatus("✅ Uppgift tillagd!", Color.FromArgb(46, 204, 113));
            }
            else
            {
                ShowStatus("❌ Ange en uppgift!", Color.FromArgb(231, 76, 60));
            }
        }

        private void BtnComplete_Click(object sender, EventArgs e)
        {
            if (lstTasks.SelectedIndex >= 0)
            {
                var taskText = lstTasks.SelectedItem.ToString();
                // Extrahera ID från början av texten
                var taskId = int.Parse(taskText.Split('.')[0].Replace("✓", "").Replace("○", "").Trim());
                _taskService.ToggleTaskCompletion(taskId);
                RefreshTaskList();
                ShowStatus("🔄 Status ändrad!", Color.FromArgb(52, 152, 219));
            }
            else
            {
                ShowStatus("⚠️ Välj en uppgift först!", Color.FromArgb(231, 76, 60));
            }
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if (lstTasks.SelectedIndex >= 0)
            {
                var taskText = lstTasks.SelectedItem.ToString();
                var taskId = int.Parse(taskText.Split('.')[0].Replace("✓", "").Replace("○", "").Trim());
                _taskService.RemoveTask(taskId);
                RefreshTaskList();
                ShowStatus("🗑️ Uppgift borttagen!", Color.FromArgb(46, 204, 113));
            }
            else
            {
                ShowStatus("⚠️ Välj en uppgift först!", Color.FromArgb(231, 76, 60));
            }
        }

        private void TxtNewTask_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                BtnAdd_Click(sender, e);
                e.Handled = true;
            }
        }

        private void RefreshTaskList()
        {
            lstTasks.Items.Clear();
            var tasks = _taskService.GetAllTasks();
            foreach (var task in tasks)
            {
                string statusIcon = task.IsCompleted ? "✅" : "⏳";
                string displayText = task.IsCompleted
                    ? $"{statusIcon} {task.Id}. {task.Title}"
                    : $"{statusIcon} {task.Id}. {task.Title}";
                lstTasks.Items.Add(displayText);
            }
        }

        private void ShowStatus(string message, Color color)
        {
            lblStatus.Text = message;
            lblStatus.ForeColor = color;

            // Lägg till en subtil animation
            var timer = new Timer();
            timer.Interval = 100;
            int counter = 0;
            timer.Tick += (s, e) =>
            {
                if (counter < 30) // 3 sekunder
                {
                    // Gör statusen lite mer synlig genom att byta opacity
                    lblStatus.ForeColor = counter % 20 < 10 ? color : Color.FromArgb(200, color);
                    counter++;
                }
                else
                {
                    lblStatus.Text = "💻 Redo att gå";
                    lblStatus.ForeColor = Color.FromArgb(127, 140, 141);
                    timer.Stop();
                    timer.Dispose();
                }
            };
            timer.Start();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            // Modern ljusgrå bakgrund
            using (SolidBrush brush = new SolidBrush(Color.FromArgb(248, 249, 250)))
            {
                e.Graphics.FillRectangle(brush, this.ClientRectangle);
            }
            base.OnPaint(e);
        }
    }
}