Option Strict On

''' <summary>
''' インスタンス管理クラス
''' </summary>
''' <remarks></remarks>
Public Class Core
    Implements IDisposable


    'ハンドル
    Friend Handle As IntPtr


    ''' <summary>
    ''' コンストラクタ
    ''' </summary>
    ''' <param name="args"></param>
    ''' <remarks></remarks>
    Public Sub New(ByVal args As String())

        'LibVLC 2.x系の実行前チェック
        '2.x系では、以下の条件が必要。
        '  dllと同じディレクトリにプラグインフォルダが無ければならない。
        '  プラグインフォルダを、環境変数"VLC_PLUGIN_PATH"にセットしておかねばならない。

        'libvlc.dllの存在チェック
        Dim appPath As String = Xb.Base.AppPath
        If (Not IO.File.Exists(Xb.Base.AppPath & "\libvlc.dll")) Then
            Xb.Util.Out("LibVLCライブラリが検出出来ません。")
            Throw New ApplicationException("LibVLCライブラリが検出出来ません。")
        End If

        'プラグインフォルダの存在チェック
        If (Not IO.Directory.Exists(Xb.Base.AppPath & "\plugins")) Then
            Xb.Util.Out("LibVLC用プラグインが検出出来ません。")
            Throw New ApplicationException("LibVLC用プラグインが検出出来ません。")
        End If

        Environment.SetEnvironmentVariable("VLC_PLUGIN_PATH", Xb.Base.AppPath & "\plugins")

        'VLCインスタンスを取得する。
        If (args.Length > 0) Then
            Me.Handle = Xb.Media.Vlc.Lib.libvlc_new(args.Length, args)
        Else
            Me.Handle = Xb.Media.Vlc.Lib.libvlc_new(0, Nothing)
        End If


        If (Me.Handle = IntPtr.Zero) Then
            Xb.Media.Vlc.Exception.ThrowIfExist("Xb.Media.Vlc.Core.New")
        End If

        'LibVLC ver 1.x系でのインスタンス取得
        'Dim args() As String = New String() { _
        '    "-I", _
        '    "dummy", _
        '    "--ignore-config", _
        '    "--plugin-path=C:\Users\ikaruga\Documents\Visual Studio 2005\Projects\MoviePlayer\Xb\Lib\Vlc\plugins" _
        '}
        'Me.Handle = Xb.Media.Vlc.Lib.libvlc_new(args.Length, args)

        'LibVLC ver 0.98でのインスタンス取得
        'Dim args() As String = New String() { _
        '    "-I", _
        '    "dummy", _
        '    "--ignore-config", _
        '    "--plugin-path=C:\Users\ikaruga\Documents\Visual Studio 2005\Projects\MoviePlayer\Xb\Lib\Vlc\plugins", _
        '    "--vout-filter=deinterlace", _
        '    "--deinterlace-mode=blend" _
        '}
        'Me.Handle = Xb.Media.Vlc.Lib.libvlc_new(args.Length, args)

    End Sub


    ''' <summary>
    ''' VLCオブジェクトのバージョンを取得する。
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetVersion() As String

        Dim result As String = Xb.Media.Vlc.Lib.libvlc_get_version()
        Xb.Media.Vlc.Exception.ThrowIfExist("Xb.Media.Vlc.Core.GetVersion")
        Return result

    End Function


    Private _disposedValue As Boolean = False        ' 重複する呼び出しを検出するには

    ' IDisposable
    Protected Overridable Sub Dispose(ByVal disposing As Boolean)
        If Not Me._disposedValue Then
            If disposing Then
            End If

            'VLCインスタンスを破棄する。
            Xb.Media.Vlc.Lib.libvlc_release(Me.Handle)
            Xb.Media.Vlc.Exception.ThrowIfExist("Xb.Media.Vlc.Core.Dispose")

        End If
        Me._disposedValue = True
    End Sub

#Region " IDisposable Support "
    ' このコードは、破棄可能なパターンを正しく実装できるように Visual Basic によって追加されました。
    Public Sub Dispose() Implements IDisposable.Dispose
        ' このコードを変更しないでください。クリーンアップ コードを上の Dispose(ByVal disposing As Boolean) に記述します。
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub
#End Region

End Class

