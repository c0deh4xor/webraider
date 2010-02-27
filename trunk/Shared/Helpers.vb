Imports System.Net.Security
Imports System.IO
Imports System.Threading

Public Module Helpers

    ''' <summary>
    ''' Validates the certificates (accept any of them).
    ''' </summary>
    ''' <param name="sender">The sender.</param>
    ''' <param name="certificate">The certificate.</param>
    ''' <param name="chain">The chain.</param>
    ''' <param name="sslPolicyErrors">The SSL policy errors.</param>
    ''' <returns><c>Always True</c></returns>
    Public Function ValidateCertificate(ByVal sender As Object, ByVal certificate As System.Security.Cryptography.X509Certificates.X509Certificate, ByVal chain As System.Security.Cryptography.X509Certificates.X509Chain, ByVal sslPolicyErrors As SslPolicyErrors) As Boolean
        'Accept any certificate
        Return True
    End Function

    ''' <summary>
    ''' Gets the resource path.
    ''' </summary>
    ''' <remarks>
    ''' First it tries to load local copy of file where you execute the application. (in this case security can be a problem, but this gives ability to easier customization so it's a trade). 
    ''' If local folder resource check fails then it tries get the related file from assembly's folder.
    ''' </remarks>
    ''' <returns></returns>
    Public Function GetResourcePath(ByVal fileName As String) As String
        If File.Exists(fileName) Then Return fileName
        Return Path.Combine(My.Application.Info.DirectoryPath, fileName)
    End Function

    ''' <summary>
    ''' Remove supplied removeList from the data.
    ''' </summary>
    ''' <param name="data"></param>
    ''' <param name="removeList"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Internall replaces all supplied array with string.empty (this is not a recursive function, shouldn't be used for blacklisting etc.)
    ''' </remarks>
    Public Function RemoveAll(ByVal data As String, ByVal ParamArray removeList() As String) As String

        For Each ToRemove As String In removeList
            data = Replace(data, ToRemove, String.Empty, 1, -1, CompareMethod.Text)
        Next

        Return data
    End Function

    ''' <summary>
    ''' Open supplied website URL in default browser
    ''' </summary>
    ''' <param name="URL"></param>
    ''' <remarks></remarks>
    Public Sub RunProcess(ByVal URL As String)
        Try
            Process.Start(URL)
        Catch ex As Exception

        End Try

    End Sub

End Module
