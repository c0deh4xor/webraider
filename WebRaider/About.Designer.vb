<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class About
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
		Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel
		Me.GroupBox1 = New System.Windows.Forms.GroupBox
		Me.LinkLabel4 = New System.Windows.Forms.LinkLabel
		Me.Label3 = New System.Windows.Forms.Label
		Me.LblDetails = New System.Windows.Forms.Label
		Me.PictureBox1 = New System.Windows.Forms.PictureBox
		Me.GroupBox2 = New System.Windows.Forms.GroupBox
		Me.Label6 = New System.Windows.Forms.Label
		Me.Label5 = New System.Windows.Forms.Label
		Me.Label4 = New System.Windows.Forms.Label
		Me.Label2 = New System.Windows.Forms.Label
		Me.Label1 = New System.Windows.Forms.Label
		Me.Button1 = New System.Windows.Forms.Button
		Me.TableLayoutPanel1.SuspendLayout()
		Me.GroupBox1.SuspendLayout()
		CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.GroupBox2.SuspendLayout()
		Me.SuspendLayout()
		'
		'TableLayoutPanel1
		'
		Me.TableLayoutPanel1.ColumnCount = 2
		Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle)
		Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle)
		Me.TableLayoutPanel1.Controls.Add(Me.GroupBox1, 1, 0)
		Me.TableLayoutPanel1.Controls.Add(Me.PictureBox1, 0, 0)
		Me.TableLayoutPanel1.Controls.Add(Me.GroupBox2, 1, 1)
		Me.TableLayoutPanel1.Controls.Add(Me.Button1, 1, 2)
		Me.TableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill
		Me.TableLayoutPanel1.Location = New System.Drawing.Point(9, 9)
		Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
		Me.TableLayoutPanel1.RowCount = 3
		Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle)
		Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle)
		Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
		Me.TableLayoutPanel1.Size = New System.Drawing.Size(556, 284)
		Me.TableLayoutPanel1.TabIndex = 6
		'
		'GroupBox1
		'
		Me.GroupBox1.Controls.Add(Me.LinkLabel4)
		Me.GroupBox1.Controls.Add(Me.Label3)
		Me.GroupBox1.Controls.Add(Me.LblDetails)
		Me.GroupBox1.Dock = System.Windows.Forms.DockStyle.Fill
		Me.GroupBox1.Location = New System.Drawing.Point(228, 3)
		Me.GroupBox1.Name = "GroupBox1"
		Me.GroupBox1.Size = New System.Drawing.Size(325, 150)
		Me.GroupBox1.TabIndex = 11
		Me.GroupBox1.TabStop = False
		'
		'LinkLabel4
		'
		Me.LinkLabel4.AutoSize = True
		Me.LinkLabel4.Location = New System.Drawing.Point(77, 104)
		Me.LinkLabel4.Name = "LinkLabel4"
		Me.LinkLabel4.Size = New System.Drawing.Size(61, 13)
		Me.LinkLabel4.TabIndex = 6
		Me.LinkLabel4.TabStop = True
		Me.LinkLabel4.Text = "WebRaider"
		'
		'Label3
		'
		Me.Label3.AutoSize = True
		Me.Label3.Location = New System.Drawing.Point(6, 104)
		Me.Label3.Name = "Label3"
		Me.Label3.Size = New System.Drawing.Size(68, 13)
		Me.Label3.TabIndex = 5
		Me.Label3.Text = "Homepage : "
		'
		'LblDetails
		'
		Me.LblDetails.AutoSize = True
		Me.LblDetails.Location = New System.Drawing.Point(6, 16)
		Me.LblDetails.Name = "LblDetails"
		Me.LblDetails.Size = New System.Drawing.Size(39, 13)
		Me.LblDetails.TabIndex = 3
		Me.LblDetails.Text = "Details"
		'
		'PictureBox1
		'
		Me.PictureBox1.BackColor = System.Drawing.Color.White
		Me.PictureBox1.Image = Global.WebRaider.My.Resources.Resources.MSL_Big_Trans_2
		Me.PictureBox1.Location = New System.Drawing.Point(3, 3)
		Me.PictureBox1.Name = "PictureBox1"
		Me.TableLayoutPanel1.SetRowSpan(Me.PictureBox1, 2)
		Me.PictureBox1.Size = New System.Drawing.Size(219, 248)
		Me.PictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage
		Me.PictureBox1.TabIndex = 10
		Me.PictureBox1.TabStop = False
		'
		'GroupBox2
		'
		Me.GroupBox2.AutoSize = True
		Me.GroupBox2.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
		Me.GroupBox2.Controls.Add(Me.Label6)
		Me.GroupBox2.Controls.Add(Me.Label5)
		Me.GroupBox2.Controls.Add(Me.Label4)
		Me.GroupBox2.Controls.Add(Me.Label2)
		Me.GroupBox2.Controls.Add(Me.Label1)
		Me.GroupBox2.Dock = System.Windows.Forms.DockStyle.Fill
		Me.GroupBox2.Location = New System.Drawing.Point(228, 159)
		Me.GroupBox2.Name = "GroupBox2"
		Me.GroupBox2.Size = New System.Drawing.Size(325, 92)
		Me.GroupBox2.TabIndex = 13
		Me.GroupBox2.TabStop = False
		Me.GroupBox2.Text = "Credits"
		'
		'Label6
		'
		Me.Label6.AutoSize = True
		Me.Label6.Location = New System.Drawing.Point(123, 63)
		Me.Label6.Name = "Label6"
		Me.Label6.Size = New System.Drawing.Size(65, 13)
		Me.Label6.TabIndex = 8
		Me.Label6.Text = "Mesut Timur"
		'
		'Label5
		'
		Me.Label5.AutoSize = True
		Me.Label5.Location = New System.Drawing.Point(123, 41)
		Me.Label5.Name = "Label5"
		Me.Label5.Size = New System.Drawing.Size(84, 13)
		Me.Label5.TabIndex = 7
		Me.Label5.Text = "Ferruh Mavituna"
		'
		'Label4
		'
		Me.Label4.AutoSize = True
		Me.Label4.Location = New System.Drawing.Point(123, 16)
		Me.Label4.Name = "Label4"
		Me.Label4.Size = New System.Drawing.Size(84, 13)
		Me.Label4.TabIndex = 6
		Me.Label4.Text = "Ferruh Mavituna"
		'
		'Label2
		'
		Me.Label2.AutoSize = True
		Me.Label2.Location = New System.Drawing.Point(41, 41)
		Me.Label2.Name = "Label2"
		Me.Label2.Size = New System.Drawing.Size(76, 13)
		Me.Label2.TabIndex = 5
		Me.Label2.Text = "Development :"
		'
		'Label1
		'
		Me.Label1.AutoSize = True
		Me.Label1.Location = New System.Drawing.Point(83, 16)
		Me.Label1.Name = "Label1"
		Me.Label1.Size = New System.Drawing.Size(34, 13)
		Me.Label1.TabIndex = 3
		Me.Label1.Text = "Idea :"
		'
		'Button1
		'
		Me.Button1.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.Button1.DialogResult = System.Windows.Forms.DialogResult.Cancel
		Me.Button1.Location = New System.Drawing.Point(478, 257)
		Me.Button1.Name = "Button1"
		Me.Button1.Size = New System.Drawing.Size(75, 23)
		Me.Button1.TabIndex = 14
		Me.Button1.Text = "OK"
		Me.Button1.UseVisualStyleBackColor = True
		'
		'About
		'
		Me.AcceptButton = Me.Button1
		Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
		Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		Me.CancelButton = Me.Button1
		Me.ClientSize = New System.Drawing.Size(574, 302)
		Me.Controls.Add(Me.TableLayoutPanel1)
		Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
		Me.MaximizeBox = False
		Me.MinimizeBox = False
		Me.Name = "About"
		Me.Padding = New System.Windows.Forms.Padding(9)
		Me.ShowInTaskbar = False
		Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
		Me.Text = "About"
		Me.TableLayoutPanel1.ResumeLayout(False)
		Me.TableLayoutPanel1.PerformLayout()
		Me.GroupBox1.ResumeLayout(False)
		Me.GroupBox1.PerformLayout()
		CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
		Me.GroupBox2.ResumeLayout(False)
		Me.GroupBox2.PerformLayout()
		Me.ResumeLayout(False)

	End Sub
    Friend WithEvents TableLayoutPanel1 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents LinkLabel4 As System.Windows.Forms.LinkLabel
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents LblDetails As System.Windows.Forms.Label
    Friend WithEvents PictureBox1 As System.Windows.Forms.PictureBox
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Button1 As System.Windows.Forms.Button

End Class
