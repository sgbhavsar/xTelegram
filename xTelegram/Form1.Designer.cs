namespace xTelegram
{
    partial class MainFrom
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            Telerik.WinControls.UI.TableViewDefinition tableViewDefinition1 = new Telerik.WinControls.UI.TableViewDefinition();
            Telerik.WinControls.UI.TableViewDefinition tableViewDefinition2 = new Telerik.WinControls.UI.TableViewDefinition();
            this.label1 = new System.Windows.Forms.Label();
            this.CodeTxt = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.radStatusStrip1 = new Telerik.WinControls.UI.RadStatusStrip();
            this.LogText = new System.Windows.Forms.RichTextBox();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.MessageLabel = new System.Windows.Forms.Label();
            this.LoadChatsButton = new System.Windows.Forms.Button();
            this.radThemeManager1 = new Telerik.WinControls.RadThemeManager();
            this.radGridView1 = new Telerik.WinControls.UI.RadGridView();
            this.EngStatusLable = new System.Windows.Forms.Label();
            this.radGridView2 = new Telerik.WinControls.UI.RadGridView();
            this.lblChatCounter = new System.Windows.Forms.Label();
            this.lblMessageCounter = new System.Windows.Forms.Label();
            this.chkLoadAll = new System.Windows.Forms.CheckBox();
            this.chkLoadUnknown = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.radStatusStrip1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radGridView1.MasterTemplate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radGridView2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radGridView2.MasterTemplate)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(93, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Enter Logon Code";
            // 
            // CodeTxt
            // 
            this.CodeTxt.Location = new System.Drawing.Point(113, 13);
            this.CodeTxt.Name = "CodeTxt";
            this.CodeTxt.Size = new System.Drawing.Size(203, 20);
            this.CodeTxt.TabIndex = 1;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(332, 9);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(92, 26);
            this.button1.TabIndex = 2;
            this.button1.Text = "LogonBtn";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.Button1_Click);
            // 
            // radStatusStrip1
            // 
            this.radStatusStrip1.Location = new System.Drawing.Point(0, 640);
            this.radStatusStrip1.Name = "radStatusStrip1";
            this.radStatusStrip1.Size = new System.Drawing.Size(1287, 26);
            this.radStatusStrip1.TabIndex = 4;
            // 
            // LogText
            // 
            this.LogText.Location = new System.Drawing.Point(12, 320);
            this.LogText.Name = "LogText";
            this.LogText.Size = new System.Drawing.Size(1263, 314);
            this.LogText.TabIndex = 5;
            this.LogText.Text = "";
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Tick += new System.EventHandler(this.Timer1_Tick);
            // 
            // MessageLabel
            // 
            this.MessageLabel.AutoSize = true;
            this.MessageLabel.Location = new System.Drawing.Point(329, 48);
            this.MessageLabel.Name = "MessageLabel";
            this.MessageLabel.Size = new System.Drawing.Size(76, 13);
            this.MessageLabel.TabIndex = 6;
            this.MessageLabel.Text = "MessageLabel";
            // 
            // LoadChatsButton
            // 
            this.LoadChatsButton.Enabled = false;
            this.LoadChatsButton.Location = new System.Drawing.Point(430, 9);
            this.LoadChatsButton.Name = "LoadChatsButton";
            this.LoadChatsButton.Size = new System.Drawing.Size(121, 27);
            this.LoadChatsButton.TabIndex = 7;
            this.LoadChatsButton.Text = "Load Chats";
            this.LoadChatsButton.UseVisualStyleBackColor = true;
            this.LoadChatsButton.Click += new System.EventHandler(this.Button2_Click);
            // 
            // radGridView1
            // 
            this.radGridView1.Location = new System.Drawing.Point(12, 87);
            // 
            // 
            // 
            this.radGridView1.MasterTemplate.ViewDefinition = tableViewDefinition1;
            this.radGridView1.Name = "radGridView1";
            this.radGridView1.ReadOnly = true;
            this.radGridView1.Size = new System.Drawing.Size(650, 227);
            this.radGridView1.TabIndex = 8;
            this.radGridView1.CellClick += new Telerik.WinControls.UI.GridViewCellEventHandler(this.RadGridView1_CellClick);
            this.radGridView1.CellDoubleClick += new Telerik.WinControls.UI.GridViewCellEventHandler(this.RadGridView1_CellDoubleClick);
            // 
            // EngStatusLable
            // 
            this.EngStatusLable.AutoSize = true;
            this.EngStatusLable.BackColor = System.Drawing.Color.Khaki;
            this.EngStatusLable.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.EngStatusLable.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.EngStatusLable.ForeColor = System.Drawing.Color.Tomato;
            this.EngStatusLable.Location = new System.Drawing.Point(13, 48);
            this.EngStatusLable.Name = "EngStatusLable";
            this.EngStatusLable.Size = new System.Drawing.Size(82, 13);
            this.EngStatusLable.TabIndex = 9;
            this.EngStatusLable.Text = "EngineStatus";
            // 
            // radGridView2
            // 
            this.radGridView2.Location = new System.Drawing.Point(668, 87);
            // 
            // 
            // 
            this.radGridView2.MasterTemplate.ViewDefinition = tableViewDefinition2;
            this.radGridView2.Name = "radGridView2";
            this.radGridView2.Size = new System.Drawing.Size(607, 227);
            this.radGridView2.TabIndex = 10;
            // 
            // lblChatCounter
            // 
            this.lblChatCounter.AutoSize = true;
            this.lblChatCounter.Location = new System.Drawing.Point(13, 71);
            this.lblChatCounter.Name = "lblChatCounter";
            this.lblChatCounter.Size = new System.Drawing.Size(92, 13);
            this.lblChatCounter.TabIndex = 11;
            this.lblChatCounter.Text = "ChatCounterLabel";
            // 
            // lblMessageCounter
            // 
            this.lblMessageCounter.AutoSize = true;
            this.lblMessageCounter.Location = new System.Drawing.Point(665, 71);
            this.lblMessageCounter.Name = "lblMessageCounter";
            this.lblMessageCounter.Size = new System.Drawing.Size(113, 13);
            this.lblMessageCounter.TabIndex = 12;
            this.lblMessageCounter.Text = "MessageCounterLabel";
            // 
            // chkLoadAll
            // 
            this.chkLoadAll.AutoSize = true;
            this.chkLoadAll.Location = new System.Drawing.Point(820, 66);
            this.chkLoadAll.Name = "chkLoadAll";
            this.chkLoadAll.Size = new System.Drawing.Size(64, 17);
            this.chkLoadAll.TabIndex = 13;
            this.chkLoadAll.Text = "Load All";
            this.chkLoadAll.UseVisualStyleBackColor = true;
            // 
            // chkLoadUnknown
            // 
            this.chkLoadUnknown.AutoSize = true;
            this.chkLoadUnknown.Location = new System.Drawing.Point(922, 66);
            this.chkLoadUnknown.Name = "chkLoadUnknown";
            this.chkLoadUnknown.Size = new System.Drawing.Size(99, 17);
            this.chkLoadUnknown.TabIndex = 14;
            this.chkLoadUnknown.Text = "Load Unknown";
            this.chkLoadUnknown.UseVisualStyleBackColor = true;
            // 
            // MainFrom
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1287, 666);
            this.Controls.Add(this.chkLoadUnknown);
            this.Controls.Add(this.chkLoadAll);
            this.Controls.Add(this.lblMessageCounter);
            this.Controls.Add(this.lblChatCounter);
            this.Controls.Add(this.radGridView2);
            this.Controls.Add(this.EngStatusLable);
            this.Controls.Add(this.radGridView1);
            this.Controls.Add(this.LoadChatsButton);
            this.Controls.Add(this.MessageLabel);
            this.Controls.Add(this.LogText);
            this.Controls.Add(this.radStatusStrip1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.CodeTxt);
            this.Controls.Add(this.label1);
            this.Name = "MainFrom";
            this.Text = "xTelegram";
            this.Load += new System.EventHandler(this.MainFrom_Load);
            ((System.ComponentModel.ISupportInitialize)(this.radStatusStrip1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radGridView1.MasterTemplate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radGridView2.MasterTemplate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radGridView2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox CodeTxt;
        private System.Windows.Forms.Button button1;
        private Telerik.WinControls.UI.RadStatusStrip radStatusStrip1;
        private System.Windows.Forms.RichTextBox LogText;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Label MessageLabel;
        private System.Windows.Forms.Button LoadChatsButton;
        private Telerik.WinControls.RadThemeManager radThemeManager1;
        private Telerik.WinControls.UI.RadGridView radGridView1;
        private System.Windows.Forms.Label EngStatusLable;
        private Telerik.WinControls.UI.RadGridView radGridView2;
        private System.Windows.Forms.Label lblChatCounter;
        private System.Windows.Forms.Label lblMessageCounter;
        private System.Windows.Forms.CheckBox chkLoadAll;
        private System.Windows.Forms.CheckBox chkLoadUnknown;
    }
}

