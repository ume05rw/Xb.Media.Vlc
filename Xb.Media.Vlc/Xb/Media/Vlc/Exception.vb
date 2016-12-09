Option Strict On

''' <summary>
''' 例外管理クラス
''' </summary>
''' <remarks></remarks>
Public Class [Exception]
    Inherits System.Exception

    Public Shared Sub ThrowIfExist(ByVal functionName As String)
        Dim err As Xb.Media.Vlc.Exception = New Xb.Media.Vlc.Exception()

        If (err._existError) Then
            Xb.Util.Out(functionName & ": " & err.Message)
            Xb.Media.Vlc.Lib.libvlc_clearerr()
            Throw err
        End If

    End Sub

    Protected _errorMessage As String
    Protected _existError As Boolean

    ''' <summary>
    ''' コンストラクタ
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub New()
        MyBase.New()

        Me._existError = False
        Dim err As IntPtr = Xb.Media.Vlc.[Lib].libvlc_errmsg()

        If (Not err = IntPtr.Zero) Then
            Me._existError = True

            Dim errMsg As String = Xb.Type.Marshaler.Utf8Marshaler.GetString(err)

            If (Not errMsg Is Nothing) Then
                _errorMessage = errMsg
            Else
                _errorMessage = "Any VLC Exception"
            End If
        Else
            _errorMessage = "Error Not Found"
        End If

    End Sub


    ''' <summary>
    ''' エラーメッセージ
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides ReadOnly Property Message() As String
        Get
            Return _errorMessage
        End Get
    End Property
            
End Class

