Imports System.ComponentModel.Composition
Imports WebRaider.Plugins.Raider
Imports WebRaider.SharedLibrary

''' <summary>
''' It doesn't WORK YET!
''' </summary>
<Export(GetType(IRaider))> _
Public Class UploadRaider
    Implements IRaider


    Dim _AttackPatterns As List(Of AttackPattern)
    Public ReadOnly Property Attacks() As System.Collections.Generic.List(Of WebRaider.SharedLibrary.AttackPattern) Implements WebRaider.Plugins.Raider.IRaider.Attacks
        Get
            Return _AttackPatterns
        End Get
    End Property

    Public ReadOnly Property Name() As String Implements WebRaider.Plugins.Raider.IRaider.Name
        Get
            Return "Upload Raider"
        End Get
    End Property

    Public Function PrepareAttack(ByVal link As WebRaider.SharedLibrary.Link, ByVal param As WebRaider.SharedLibrary.Parameter, ByVal attackPattern As WebRaider.SharedLibrary.AttackPattern) As Boolean Implements WebRaider.Plugins.Raider.IRaider.PrepareAttack

        'Skip if this is not an upload parameter
        If Not param.Type = Parameter.ParameterType.Upload Then Return False

        param.Value = attackPattern.Pattern
        link.UriManager.AddParameter(param)

        Return True

        'Dim params = From UploadParam In link.UriManager.Params Where param.Type = Parameter.ParameterType.Upload


    End Function

    Public Sub Setup() Implements WebRaider.Plugins.Raider.IRaider.Setup
        _AttackPatterns = New List(Of AttackPattern)(1)
        _AttackPatterns.Add(New AttackPattern("ASP Upload", "path:to:web-shell"))
    End Sub

    Private _Enabled As Boolean = False
    Public Property Enabled() As Boolean Implements WebRaider.Plugins.Raider.IRaider.Enabled
        Get
            Return False
        End Get
        Set(ByVal value As Boolean)
            _Enabled = value
        End Set
    End Property

End Class
