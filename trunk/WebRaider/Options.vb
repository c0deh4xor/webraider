Imports System.Threading


Public Class Options

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        OptionsExit()
        Me.Close()
    End Sub

    Private Sub Options_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        OptionsExit()
    End Sub


    Private Sub Options_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Main.Enabled = False

        txtPort.Text = My.Settings.BindPort.ToString()

        cmbPayloads.Items.Clear()
        For Each payload In SharedLibrary.Options.Payloads
            cmbPayloads.Items.Add(payload.ToString())
        Next
        If (cmbPayloads.Items.Count > 0) Then
            cmbPayloads.SelectedIndex = 0
        End If


        cmbGroup.SelectedIndex = cmbGroup.Items.IndexOf(WebRaider.SharedLibrary.Options.GroupNumber.ToString())

        If (WebRaider.SharedLibrary.Options.ParameterType.ToString = ParameterType.Int.ToString()) Then
            rdbInt.Select()
        Else
            rdbStr.Select()
        End If

        'Select Case WebRaider.SharedLibrary.Options.db
        '    Case SharedLibrary.Options.Database.MSSQL
        '        rdbMssql.Select()
        '    Case SharedLibrary.Options.Database.ORACLE
        '        rdbOracle.Select()
        'End Select





        If Not String.IsNullOrEmpty(My.Settings.BindInterface) Then
            cmbInterface.Items.Clear()
            cmbInterface.Items.Add(My.Settings.BindInterface)
        Else
            Dim ipHostEntry As System.Net.IPHostEntry = System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName())
            For Each ipAddress As System.Net.IPAddress In ipHostEntry.AddressList
                If ipAddress.AddressFamily = Net.Sockets.AddressFamily.InterNetwork Then
                    cmbInterface.Items.Add(ipAddress)
                End If
            Next
        End If

        cmbInterface.SelectedIndex = 0
    End Sub
    Private Sub OptionsExit()
        Main.Enabled = True
        Main.Focus()
    End Sub

    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click

        If (cmbInterface.SelectedIndex > -1) Then
            My.Settings.BindInterface = cmbInterface.SelectedItem.ToString()
        Else
            My.Settings.BindInterface = cmbInterface.Text
        End If


        My.Settings.BindPort = Convert.ToInt32(txtPort.Text)

        WebRaider.SharedLibrary.Options.GroupNumber = Convert.ToInt32(cmbGroup.SelectedItem.ToString())
        If (rdbInt.Checked) Then
            WebRaider.SharedLibrary.Options.ParameterType = ParameterType.Int.ToString()
        Else
            WebRaider.SharedLibrary.Options.ParameterType = ParameterType.Str.ToString()
        End If
        WebRaider.SharedLibrary.Options.SelectedPayload = cmbPayloads.SelectedItem.ToString()
 
        My.Settings.GroupNumber = WebRaider.SharedLibrary.Options.GroupNumber.ToString
        My.Settings.ParameterType = WebRaider.SharedLibrary.Options.ParameterType.ToString
        My.Settings.LastPayload = WebRaider.SharedLibrary.Options.SelectedPayload
        My.Settings.Save()


        DirectCast(Me.Owner, Main).LoadPayload()

        Dim firstThread As New Thread(New ThreadStart(AddressOf BuildAll))
        firstThread.Start()



    End Sub
    Private Sub BuildAll()
        Application.UseWaitCursor = True
        GenerateShell()
        GenerateListener()
        Dim Build As Process = New Process()
        With Build
            .StartInfo.FileName = String.Format("""{0}\Utilities\BuildAll.bat""", System.IO.Directory.GetCurrentDirectory)
            .StartInfo.UseShellExecute = False
            .StartInfo.CreateNoWindow = True
            .StartInfo.RedirectStandardInput = True
            .StartInfo.RedirectStandardOutput = True
            .StartInfo.RedirectStandardError = True
            .StartInfo.WorkingDirectory = System.IO.Directory.GetCurrentDirectory & "\Utilities\"
        End With
        Build.Start()
        While (Build.HasExited = False)
            System.Threading.Thread.Sleep(1000)
        End While
        For Each CurrentRaider As WebRaider.Plugins.Raider.IRaider In Main.RaiderPluginsReader.Where(Function(f) f.Enabled = True)
            CurrentRaider.Setup()
        Next
        Application.UseWaitCursor = False

        Invoke(New Action(AddressOf Me.Close))
    End Sub
    Private Sub GenerateShell()
        Dim Shell As Process = New Process()
        With Shell
            .StartInfo.FileName = String.Format("""{0}\Utilities\GenerateShellParameterized.bat""", System.IO.Directory.GetCurrentDirectory)
            .StartInfo.Arguments = String.Format("""{0}"" ""{1}"" {2} {3}", IO.Path.GetFullPath(My.Settings.RubyExecutable), IO.Path.GetFullPath(My.Settings.MsfcliPath) & "\msfpayload", My.Settings.BindInterface, My.Settings.BindPort)
            'MsgBox(.StartInfo.Arguments)
            .StartInfo.UseShellExecute = False
            .StartInfo.CreateNoWindow = True
            .StartInfo.RedirectStandardInput = True
            .StartInfo.RedirectStandardOutput = True
            .StartInfo.RedirectStandardError = True
            .StartInfo.WorkingDirectory = System.IO.Directory.GetCurrentDirectory & "\Utilities\"
        End With
        Shell.Start()
        While (Shell.HasExited = False)
            System.Threading.Thread.Sleep(1000)
        End While
    End Sub

    Private Sub GenerateListener()
        DirectCast(Me.Owner, Main).RefreshShells()
    End Sub
    Enum ParameterType
        Int = 1
        Str = 2
    End Enum

End Class


