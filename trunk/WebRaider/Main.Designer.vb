Imports System.ComponentModel.Composition.Hosting
Imports System.ComponentModel.Composition
Imports Raider

<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Main
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Main))
		Me.GroupBox1 = New System.Windows.Forms.GroupBox
		Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel
		Me.BtnStart = New System.Windows.Forms.Button
		Me.TxtURL = New System.Windows.Forms.TextBox
		Me.TxtPayload = New System.Windows.Forms.TextBox
		Me.GroupBox2 = New System.Windows.Forms.GroupBox
		Me.LstPlugins = New System.Windows.Forms.ListView
		Me.ColName = New System.Windows.Forms.ColumnHeader
		Me.ColPluginType = New System.Windows.Forms.ColumnHeader
		Me.GroupBox3 = New System.Windows.Forms.GroupBox
		Me.TableLayoutPanel2 = New System.Windows.Forms.TableLayoutPanel
		Me.BtnStartListener = New System.Windows.Forms.Button
		Me.Panel1 = New System.Windows.Forms.Panel
		Me.LblShell = New System.Windows.Forms.Label
		Me.PictureBox1 = New System.Windows.Forms.PictureBox
		Me.Panel2 = New System.Windows.Forms.Panel
		Me.BtnSend = New System.Windows.Forms.Button
		Me.TxtInp = New System.Windows.Forms.TextBox
		Me.TabShells = New System.Windows.Forms.TabControl
		Me.TabPage1 = New System.Windows.Forms.TabPage
		Me.TxtShell0 = New System.Windows.Forms.TextBox
		Me.DlgMsfcli = New System.Windows.Forms.OpenFileDialog
		Me.DlgRuby = New System.Windows.Forms.OpenFileDialog
		Me.MenuStrip1 = New System.Windows.Forms.MenuStrip
		Me.OptionsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
		Me.OptionsToolStripMenuItem1 = New System.Windows.Forms.ToolStripMenuItem
		Me.ToolStripMenuItem2 = New System.Windows.Forms.ToolStripSeparator
		Me.ExitToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
		Me.AboutToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
		Me.HelpToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
		Me.ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator
		Me.ReportABugToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
		Me.ToolStripSeparator2 = New System.Windows.Forms.ToolStripSeparator
		Me.AboutToolStripMenuItem1 = New System.Windows.Forms.ToolStripMenuItem
		Me.OneClickOwnageToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
		Me.GroupBox1.SuspendLayout()
		Me.TableLayoutPanel1.SuspendLayout()
		Me.GroupBox2.SuspendLayout()
		Me.GroupBox3.SuspendLayout()
		Me.TableLayoutPanel2.SuspendLayout()
		Me.Panel1.SuspendLayout()
		CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.Panel2.SuspendLayout()
		Me.TabShells.SuspendLayout()
		Me.TabPage1.SuspendLayout()
		Me.MenuStrip1.SuspendLayout()
		Me.SuspendLayout()
		'
		'GroupBox1
		'
		Me.GroupBox1.AutoSize = True
		Me.GroupBox1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
		Me.GroupBox1.Controls.Add(Me.TableLayoutPanel1)
		Me.GroupBox1.Dock = System.Windows.Forms.DockStyle.Top
		Me.GroupBox1.Location = New System.Drawing.Point(0, 24)
		Me.GroupBox1.Name = "GroupBox1"
		Me.GroupBox1.Size = New System.Drawing.Size(690, 64)
		Me.GroupBox1.TabIndex = 0
		Me.GroupBox1.TabStop = False
		Me.GroupBox1.Text = "Target"
		'
		'TableLayoutPanel1
		'
		Me.TableLayoutPanel1.AutoSize = True
		Me.TableLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
		Me.TableLayoutPanel1.ColumnCount = 2
		Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 87.7193!))
		Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.2807!))
		Me.TableLayoutPanel1.Controls.Add(Me.BtnStart, 0, 0)
		Me.TableLayoutPanel1.Controls.Add(Me.TxtURL, 0, 0)
		Me.TableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill
		Me.TableLayoutPanel1.Location = New System.Drawing.Point(3, 16)
		Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
		Me.TableLayoutPanel1.RowCount = 1
		Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
		Me.TableLayoutPanel1.Size = New System.Drawing.Size(684, 45)
		Me.TableLayoutPanel1.TabIndex = 2
		'
		'BtnStart
		'
		Me.BtnStart.Location = New System.Drawing.Point(603, 3)
		Me.BtnStart.Name = "BtnStart"
		Me.BtnStart.Size = New System.Drawing.Size(75, 23)
		Me.BtnStart.TabIndex = 2
		Me.BtnStart.Text = "&Start"
		Me.BtnStart.UseVisualStyleBackColor = True
		'
		'TxtURL
		'
		Me.TxtURL.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest
		Me.TxtURL.DataBindings.Add(New System.Windows.Forms.Binding("Text", Global.WebRaider.My.MySettings.Default, "Target", True, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged))
		Me.TxtURL.Dock = System.Windows.Forms.DockStyle.Fill
		Me.TxtURL.Location = New System.Drawing.Point(3, 3)
		Me.TxtURL.Multiline = True
		Me.TxtURL.Name = "TxtURL"
		Me.TxtURL.Size = New System.Drawing.Size(594, 39)
		Me.TxtURL.TabIndex = 1
		Me.TxtURL.Text = Global.WebRaider.My.MySettings.Default.Target
		'
		'TxtPayload
		'
		Me.TxtPayload.Dock = System.Windows.Forms.DockStyle.Bottom
		Me.TxtPayload.Location = New System.Drawing.Point(0, 540)
		Me.TxtPayload.Multiline = True
		Me.TxtPayload.Name = "TxtPayload"
		Me.TxtPayload.Size = New System.Drawing.Size(690, 50)
		Me.TxtPayload.TabIndex = 3
		'
		'GroupBox2
		'
		Me.GroupBox2.Controls.Add(Me.LstPlugins)
		Me.GroupBox2.Dock = System.Windows.Forms.DockStyle.Top
		Me.GroupBox2.Location = New System.Drawing.Point(0, 88)
		Me.GroupBox2.Name = "GroupBox2"
		Me.GroupBox2.Size = New System.Drawing.Size(690, 135)
		Me.GroupBox2.TabIndex = 4
		Me.GroupBox2.TabStop = False
		Me.GroupBox2.Text = "Loaded Plugins "
		'
		'LstPlugins
		'
		Me.LstPlugins.CheckBoxes = True
		Me.LstPlugins.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColName, Me.ColPluginType})
		Me.LstPlugins.Dock = System.Windows.Forms.DockStyle.Fill
		Me.LstPlugins.Location = New System.Drawing.Point(3, 16)
		Me.LstPlugins.Name = "LstPlugins"
		Me.LstPlugins.Size = New System.Drawing.Size(684, 116)
		Me.LstPlugins.TabIndex = 3
		Me.LstPlugins.UseCompatibleStateImageBehavior = False
		Me.LstPlugins.View = System.Windows.Forms.View.Details
		'
		'ColName
		'
		Me.ColName.Text = "Name"
		Me.ColName.Width = 384
		'
		'ColPluginType
		'
		Me.ColPluginType.Text = "Plugin Type"
		Me.ColPluginType.Width = 267
		'
		'GroupBox3
		'
		Me.GroupBox3.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
					Or System.Windows.Forms.AnchorStyles.Left) _
					Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.GroupBox3.Controls.Add(Me.TableLayoutPanel2)
		Me.GroupBox3.Location = New System.Drawing.Point(0, 226)
		Me.GroupBox3.Name = "GroupBox3"
		Me.GroupBox3.Size = New System.Drawing.Size(690, 304)
		Me.GroupBox3.TabIndex = 12
		Me.GroupBox3.TabStop = False
		Me.GroupBox3.Text = "Ownage Panel"
		'
		'TableLayoutPanel2
		'
		Me.TableLayoutPanel2.ColumnCount = 2
		Me.TableLayoutPanel2.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 160.0!))
		Me.TableLayoutPanel2.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
		Me.TableLayoutPanel2.Controls.Add(Me.BtnStartListener, 0, 1)
		Me.TableLayoutPanel2.Controls.Add(Me.Panel1, 0, 0)
		Me.TableLayoutPanel2.Controls.Add(Me.Panel2, 1, 1)
		Me.TableLayoutPanel2.Controls.Add(Me.TabShells, 1, 0)
		Me.TableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill
		Me.TableLayoutPanel2.Location = New System.Drawing.Point(3, 16)
		Me.TableLayoutPanel2.Name = "TableLayoutPanel2"
		Me.TableLayoutPanel2.RowCount = 2
		Me.TableLayoutPanel2.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 87.29097!))
		Me.TableLayoutPanel2.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.70903!))
		Me.TableLayoutPanel2.Size = New System.Drawing.Size(684, 285)
		Me.TableLayoutPanel2.TabIndex = 0
		'
		'BtnStartListener
		'
		Me.BtnStartListener.Location = New System.Drawing.Point(3, 251)
		Me.BtnStartListener.Name = "BtnStartListener"
		Me.BtnStartListener.Size = New System.Drawing.Size(90, 23)
		Me.BtnStartListener.TabIndex = 13
		Me.BtnStartListener.Text = "&Start Listener"
		Me.BtnStartListener.UseVisualStyleBackColor = True
		Me.BtnStartListener.Visible = False
		'
		'Panel1
		'
		Me.Panel1.AutoSize = True
		Me.Panel1.Controls.Add(Me.LblShell)
		Me.Panel1.Controls.Add(Me.PictureBox1)
		Me.Panel1.Location = New System.Drawing.Point(3, 3)
		Me.Panel1.Name = "Panel1"
		Me.Panel1.Size = New System.Drawing.Size(154, 139)
		Me.Panel1.TabIndex = 12
		'
		'LblShell
		'
		Me.LblShell.AutoSize = True
		Me.LblShell.BackColor = System.Drawing.Color.Black
		Me.LblShell.Font = New System.Drawing.Font("Georgia", 27.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.LblShell.ForeColor = System.Drawing.Color.White
		Me.LblShell.Location = New System.Drawing.Point(91, 65)
		Me.LblShell.Name = "LblShell"
		Me.LblShell.Size = New System.Drawing.Size(42, 43)
		Me.LblShell.TabIndex = 10
		Me.LblShell.Text = "0"
		'
		'PictureBox1
		'
		Me.PictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center
		Me.PictureBox1.Image = Global.WebRaider.My.Resources.Resources.shell
		Me.PictureBox1.Location = New System.Drawing.Point(0, 3)
		Me.PictureBox1.Name = "PictureBox1"
		Me.PictureBox1.Size = New System.Drawing.Size(154, 133)
		Me.PictureBox1.TabIndex = 9
		Me.PictureBox1.TabStop = False
		'
		'Panel2
		'
		Me.Panel2.Controls.Add(Me.BtnSend)
		Me.Panel2.Controls.Add(Me.TxtInp)
		Me.Panel2.Dock = System.Windows.Forms.DockStyle.Fill
		Me.Panel2.Location = New System.Drawing.Point(163, 251)
		Me.Panel2.Name = "Panel2"
		Me.Panel2.Size = New System.Drawing.Size(518, 31)
		Me.Panel2.TabIndex = 14
		'
		'BtnSend
		'
		Me.BtnSend.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.BtnSend.Location = New System.Drawing.Point(437, 4)
		Me.BtnSend.Name = "BtnSend"
		Me.BtnSend.Size = New System.Drawing.Size(75, 23)
		Me.BtnSend.TabIndex = 16
		Me.BtnSend.Text = "Send"
		Me.BtnSend.UseVisualStyleBackColor = True
		'
		'TxtInp
		'
		Me.TxtInp.AcceptsReturn = True
		Me.TxtInp.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
					Or System.Windows.Forms.AnchorStyles.Left) _
					Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.TxtInp.Font = New System.Drawing.Font("Consolas", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.TxtInp.Location = New System.Drawing.Point(0, 5)
		Me.TxtInp.Name = "TxtInp"
		Me.TxtInp.Size = New System.Drawing.Size(434, 22)
		Me.TxtInp.TabIndex = 18
		'
		'TabShells
		'
		Me.TabShells.Controls.Add(Me.TabPage1)
		Me.TabShells.Dock = System.Windows.Forms.DockStyle.Fill
		Me.TabShells.Location = New System.Drawing.Point(163, 3)
		Me.TabShells.Name = "TabShells"
		Me.TabShells.SelectedIndex = 0
		Me.TabShells.Size = New System.Drawing.Size(518, 242)
		Me.TabShells.TabIndex = 15
		'
		'TabPage1
		'
		Me.TabPage1.Controls.Add(Me.TxtShell0)
		Me.TabPage1.Location = New System.Drawing.Point(4, 22)
		Me.TabPage1.Name = "TabPage1"
		Me.TabPage1.Size = New System.Drawing.Size(510, 216)
		Me.TabPage1.TabIndex = 0
		Me.TabPage1.Text = "Listener 0"
		Me.TabPage1.UseVisualStyleBackColor = True
		'
		'TxtShell0
		'
		Me.TxtShell0.Dock = System.Windows.Forms.DockStyle.Fill
		Me.TxtShell0.Font = New System.Drawing.Font("Consolas", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.TxtShell0.Location = New System.Drawing.Point(0, 0)
		Me.TxtShell0.MaxLength = 320767
		Me.TxtShell0.Multiline = True
		Me.TxtShell0.Name = "TxtShell0"
		Me.TxtShell0.ScrollBars = System.Windows.Forms.ScrollBars.Both
		Me.TxtShell0.Size = New System.Drawing.Size(510, 216)
		Me.TxtShell0.TabIndex = 6
		Me.TxtShell0.WordWrap = False
		'
		'DlgMsfcli
		'
		Me.DlgMsfcli.AddExtension = False
		Me.DlgMsfcli.FileName = "msfcli"
		Me.DlgMsfcli.Title = "Choose Msfcli File (This file might be installed under your users profile. Such a" & _
			"s C:\Users\Administrator\AppData\Local\msf32\)"
		'
		'DlgRuby
		'
		Me.DlgRuby.FileName = "ruby.exe"
		Me.DlgRuby.InitialDirectory = "C:\Program Files\Metasploit\Framework3\bin"
		'
		'MenuStrip1
		'
		Me.MenuStrip1.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(162, Byte))
		Me.MenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.OptionsToolStripMenuItem, Me.AboutToolStripMenuItem})
		Me.MenuStrip1.Location = New System.Drawing.Point(0, 0)
		Me.MenuStrip1.Name = "MenuStrip1"
		Me.MenuStrip1.Size = New System.Drawing.Size(690, 24)
		Me.MenuStrip1.TabIndex = 13
		Me.MenuStrip1.Text = "MenuStrip1"
		'
		'OptionsToolStripMenuItem
		'
		Me.OptionsToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.OptionsToolStripMenuItem1, Me.ToolStripMenuItem2, Me.ExitToolStripMenuItem})
		Me.OptionsToolStripMenuItem.Name = "OptionsToolStripMenuItem"
		Me.OptionsToolStripMenuItem.Size = New System.Drawing.Size(35, 20)
		Me.OptionsToolStripMenuItem.Text = "File"
		'
		'OptionsToolStripMenuItem1
		'
		Me.OptionsToolStripMenuItem1.Name = "OptionsToolStripMenuItem1"
		Me.OptionsToolStripMenuItem1.Size = New System.Drawing.Size(111, 22)
		Me.OptionsToolStripMenuItem1.Text = "Options"
		'
		'ToolStripMenuItem2
		'
		Me.ToolStripMenuItem2.Name = "ToolStripMenuItem2"
		Me.ToolStripMenuItem2.Size = New System.Drawing.Size(108, 6)
		'
		'ExitToolStripMenuItem
		'
		Me.ExitToolStripMenuItem.Name = "ExitToolStripMenuItem"
		Me.ExitToolStripMenuItem.Size = New System.Drawing.Size(111, 22)
		Me.ExitToolStripMenuItem.Text = "Exit"
		'
		'AboutToolStripMenuItem
		'
		Me.AboutToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.HelpToolStripMenuItem, Me.ToolStripSeparator1, Me.OneClickOwnageToolStripMenuItem, Me.ReportABugToolStripMenuItem, Me.ToolStripSeparator2, Me.AboutToolStripMenuItem1})
		Me.AboutToolStripMenuItem.Name = "AboutToolStripMenuItem"
		Me.AboutToolStripMenuItem.Size = New System.Drawing.Size(40, 20)
		Me.AboutToolStripMenuItem.Text = "Help"
		'
		'HelpToolStripMenuItem
		'
		Me.HelpToolStripMenuItem.Image = Global.WebRaider.My.Resources.Resources.help
		Me.HelpToolStripMenuItem.Name = "HelpToolStripMenuItem"
		Me.HelpToolStripMenuItem.Size = New System.Drawing.Size(161, 22)
		Me.HelpToolStripMenuItem.Text = "Help"
		'
		'ToolStripSeparator1
		'
		Me.ToolStripSeparator1.Name = "ToolStripSeparator1"
		Me.ToolStripSeparator1.Size = New System.Drawing.Size(158, 6)
		'
		'ReportABugToolStripMenuItem
		'
		Me.ReportABugToolStripMenuItem.Image = CType(resources.GetObject("ReportABugToolStripMenuItem.Image"), System.Drawing.Image)
		Me.ReportABugToolStripMenuItem.Name = "ReportABugToolStripMenuItem"
		Me.ReportABugToolStripMenuItem.Size = New System.Drawing.Size(161, 22)
		Me.ReportABugToolStripMenuItem.Text = "Report a bug "
		Me.ReportABugToolStripMenuItem.Visible = False
		'
		'ToolStripSeparator2
		'
		Me.ToolStripSeparator2.Name = "ToolStripSeparator2"
		Me.ToolStripSeparator2.Size = New System.Drawing.Size(158, 6)
		'
		'AboutToolStripMenuItem1
		'
		Me.AboutToolStripMenuItem1.Name = "AboutToolStripMenuItem1"
		Me.AboutToolStripMenuItem1.Size = New System.Drawing.Size(161, 22)
		Me.AboutToolStripMenuItem1.Text = "About"
		'
		'OneClickOwnageToolStripMenuItem
		'
		Me.OneClickOwnageToolStripMenuItem.Name = "OneClickOwnageToolStripMenuItem"
		Me.OneClickOwnageToolStripMenuItem.Size = New System.Drawing.Size(161, 22)
		Me.OneClickOwnageToolStripMenuItem.Text = "&One Click Ownage"
		'
		'Main
		'
		Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
		Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		Me.ClientSize = New System.Drawing.Size(690, 590)
		Me.Controls.Add(Me.GroupBox3)
		Me.Controls.Add(Me.TxtPayload)
		Me.Controls.Add(Me.GroupBox2)
		Me.Controls.Add(Me.GroupBox1)
		Me.Controls.Add(Me.MenuStrip1)
		Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
		Me.MainMenuStrip = Me.MenuStrip1
		Me.Name = "Main"
		Me.Text = "WebRaider {0}"
		Me.GroupBox1.ResumeLayout(False)
		Me.GroupBox1.PerformLayout()
		Me.TableLayoutPanel1.ResumeLayout(False)
		Me.TableLayoutPanel1.PerformLayout()
		Me.GroupBox2.ResumeLayout(False)
		Me.GroupBox3.ResumeLayout(False)
		Me.TableLayoutPanel2.ResumeLayout(False)
		Me.TableLayoutPanel2.PerformLayout()
		Me.Panel1.ResumeLayout(False)
		Me.Panel1.PerformLayout()
		CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
		Me.Panel2.ResumeLayout(False)
		Me.Panel2.PerformLayout()
		Me.TabShells.ResumeLayout(False)
		Me.TabPage1.ResumeLayout(False)
		Me.TabPage1.PerformLayout()
		Me.MenuStrip1.ResumeLayout(False)
		Me.MenuStrip1.PerformLayout()
		Me.ResumeLayout(False)
		Me.PerformLayout()

	End Sub
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox


    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()


    End Sub
    Friend WithEvents TxtPayload As System.Windows.Forms.TextBox
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents LstPlugins As System.Windows.Forms.ListView
    Friend WithEvents ColName As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColPluginType As System.Windows.Forms.ColumnHeader
    Friend WithEvents TableLayoutPanel1 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents BtnStart As System.Windows.Forms.Button
    Friend WithEvents TxtURL As System.Windows.Forms.TextBox
    Friend WithEvents GroupBox3 As System.Windows.Forms.GroupBox
    Friend WithEvents TableLayoutPanel2 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents BtnStartListener As System.Windows.Forms.Button
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents LblShell As System.Windows.Forms.Label
    Friend WithEvents PictureBox1 As System.Windows.Forms.PictureBox
    Friend WithEvents BtnSend As System.Windows.Forms.Button
    Friend WithEvents Panel2 As System.Windows.Forms.Panel
    Friend WithEvents TxtInp As System.Windows.Forms.TextBox
    Friend WithEvents TabShells As System.Windows.Forms.TabControl
    Friend WithEvents TabPage1 As System.Windows.Forms.TabPage
    Friend WithEvents TxtShell0 As System.Windows.Forms.TextBox
    Friend WithEvents DlgMsfcli As System.Windows.Forms.OpenFileDialog
    Friend WithEvents DlgRuby As System.Windows.Forms.OpenFileDialog
    Friend WithEvents MenuStrip1 As System.Windows.Forms.MenuStrip
    Friend WithEvents OptionsToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents AboutToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents HelpToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator1 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ReportABugToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator2 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents AboutToolStripMenuItem1 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents OptionsToolStripMenuItem1 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem2 As System.Windows.Forms.ToolStripSeparator
	Friend WithEvents ExitToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
	Friend WithEvents OneClickOwnageToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
End Class
