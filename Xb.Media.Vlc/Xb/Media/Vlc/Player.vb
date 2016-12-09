Option Strict On

Imports System.Threading

''' <summary>
''' プレイヤー管理クラス
''' </summary>
''' <remarks>
''' VLCプレイヤーオブジェクトは１メディアに対して１つ生成する想定。
''' 再生開始で１インスタンスが生成され、終了でDisposeされる。
''' </remarks>
Public Class Player
    Implements IDisposable

    ''' <summary>
    ''' 進捗位置イベント引数定義クラス
    ''' </summary>
    ''' <remarks></remarks>
    Public Class ProgressEventArgs
        Inherits EventArgs

        Private _percentage As Single
        Private _time As Long

        Public ReadOnly Property Percentage() As Single
            Get
                Return Me._percentage
            End Get
        End Property

        Public ReadOnly Property Time() As Long
            Get
                Return Me._time
            End Get
        End Property

        Public Sub New(ByVal percentage As Single, ByVal time As Long)
            Me._percentage = percentage
            Me._time = time
        End Sub
    End Class
    Public Delegate Sub ProgressEventHandler(ByVal sender As Object, ByVal e As ProgressEventArgs)
    Public Event Progressed As ProgressEventHandler

    Public Event Started As EventHandler
    Public Event Finished As EventHandler
    Public Event Paused As EventHandler
    Public Event Expand As EventHandler
    Public Event Contracted As EventHandler


    Private ReadOnly _handle As IntPtr
    Private _playing, _paused As Boolean
    Private _timer As System.Threading.Timer
    Private _timerDelegate As System.Threading.TimerCallback
    Private _isDisposing As Boolean
    Private _isDisposed As Boolean

#Region "プロパティ"

    ''' <summary>
    ''' 再生中か否か
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property IsPlaying() As Boolean
        Get
            Return _playing AndAlso Not _paused
        End Get
    End Property


    ''' <summary>
    ''' ポーズ中か否か
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property IsPaused() As Boolean
        Get
            Return _playing AndAlso _paused
        End Get
    End Property


    ''' <summary>
    ''' ボリューム
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property Volume() As Integer
        Get
            If (Me.IsDisposing()) Then Return 0
            Dim result As Integer = [Lib].libvlc_audio_get_volume(Me._handle)
            Xb.Media.Vlc.Exception.ThrowIfExist("Xb.Media.Vlc.Player.Volume_1")
            Return result
        End Get
        Set(ByVal value As Integer)
            If (Me.IsDisposing()) Then Return
            If (0 > value) Then value = 0
            If (value > 100) Then value = 100
            [Lib].libvlc_audio_set_volume(Me._handle, value)
            Xb.Media.Vlc.Exception.ThrowIfExist("Xb.Media.Vlc.Player.Volume_2")

            Try
                '初回ボリューム設定時に、必ず音声デバイス取得不能エラーが出てしまうらしい。
            Catch ex As Exception
                'ひとまず棚上げ
            End Try

        End Set
    End Property


    ''' <summary>
    ''' ミュート
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property Mute() As Boolean
        Get
            If (Me.IsDisposing()) Then Return False
            Dim result As Integer = [Lib].libvlc_audio_get_mute(Me._handle)
            Xb.Media.Vlc.Exception.ThrowIfExist("Xb.Media.Vlc.Player.Mute_1")
            Return (result = 1)
        End Get
        Set(ByVal value As Boolean)
            If (Me.IsDisposing()) Then Return
            [Lib].libvlc_audio_set_mute(Me._handle, CInt(IIf(value, 1, 0)))
            Xb.Media.Vlc.Exception.ThrowIfExist("Xb.Media.Vlc.Player.Mute_2")
        End Set
    End Property


    ''' <summary>
    ''' フルスクリーン設定
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks>
    ''' 動作しない。
    ''' http://www.videolan.org/developers/vlc/doc/doxygen/html/group__libvlc__video.html#ga26892692dcb079743c9c7e0df3308ea4
    ''' 「トップレベルウインドウ」にする必要があるらしい。
    ''' libvlc_media_player_set_xwindow
    ''' </remarks>
    Public Property FullScreen() As Boolean
        Get
            If (Me.IsDisposing()) Then Return False
            Dim result As Integer = [Lib].libvlc_get_fullscreen(Me._handle)
            Xb.Media.Vlc.Exception.ThrowIfExist("Xb.Media.Vlc.Player.FullScreen_1")
            Return (result = 1)
        End Get
        Set(ByVal value As Boolean)
            If (Me.IsDisposing()) Then Return
            [Lib].libvlc_set_fullscreen(Me._handle, value)
            Xb.Media.Vlc.Exception.ThrowIfExist("Xb.Media.Vlc.Player.FullScreen_2")

            If (value) Then
                Me.FireEvent("Expand", Nothing)
            Else
                Me.FireEvent("Contracted", Nothing)
            End If

        End Set
    End Property


    ''' <summary>
    ''' 再生位置
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property Percentage() As Single
        Get
            If (Me.IsDisposing()) Then Return 0
            Dim result As Single = [Lib].libvlc_media_player_get_position(Me._handle)
            Xb.Media.Vlc.Exception.ThrowIfExist("Xb.Media.Vlc.Player.Percentage_1")
            Return result
        End Get
        Set(ByVal value As Single)
            If (Me.IsDisposing()) Then Return
            If (0 > value) Then value = 0
            If (value > 1) Then value = 1
            [Lib].libvlc_media_player_set_position(Me._handle, value)
            Xb.Media.Vlc.Exception.ThrowIfExist("Xb.Media.Vlc.Player.Percentage_2")
            Me.FireEvent("Progressed", New Object() {value, Me.Timestamp})

            If (Me.Length > 0 And Me.Length <= Me.Timestamp) Then Me.Dispose()
        End Set
    End Property


    ''' <summary>
    ''' シーク可能か否か
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property IsSeekable() As Boolean
        Get
            If (Me.IsDisposing()) Then Return False
            Dim result As Integer = [Lib].libvlc_media_player_is_seekable(Me._handle)
            Xb.Media.Vlc.Exception.ThrowIfExist("Xb.Media.Vlc.Player.IsSeekable")
            Return (result = 1)
        End Get
    End Property


    ''' <summary>
    ''' タイムスタンプ
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property Timestamp() As Long
        Get
            If (Me.IsDisposing()) Then Return 0
            Dim result As Long = [Lib].libvlc_media_player_get_time(Me._handle)
            Xb.Media.Vlc.Exception.ThrowIfExist("Xb.Media.Vlc.Player.Timestamp_1")
            Return result
        End Get
        Set(ByVal value As Long)
            If (Me.IsDisposing()) Then Return
            If (0 > value) Then value = 0
            [Lib].libvlc_media_player_set_time(Me._handle, value)
            Xb.Media.Vlc.Exception.ThrowIfExist("Xb.Media.Vlc.Player.Timestamp_2")
            Me.FireEvent("Progressed", New Object() {Me.Percentage, value})

            If (Me.Length > 0 And Me.Length <= Me.Timestamp) Then Me.Dispose()
        End Set
    End Property


    ''' <summary>
    ''' メディア長
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property Length() As Long
        Get
            If (Me.IsDisposing()) Then Return 0
            Dim result As Long = [Lib].libvlc_media_player_get_length(Me._handle)
            Xb.Media.Vlc.Exception.ThrowIfExist("Xb.Media.Vlc.Player.Length")
            Return result
        End Get
    End Property

#End Region

    ''' <summary>
    ''' コンストラクタ
    ''' </summary>
    ''' <param name="media"></param>
    ''' <remarks></remarks>
    Public Sub New(ByVal media As Xb.Media.Vlc.Media, _
                    ByVal renderPanelHandle As IntPtr)

        'Xb.App.Out("Player.New")

        Me._isDisposing = False
        Me._isDisposed = False

        'Me._renderManager = New RenderManager(render)

        'VLCメディアプレイヤーインスタンスを取得する。
        Me._handle = Xb.Media.Vlc.Lib.libvlc_media_player_new_from_media(media.Handle)

        If (Me._handle = IntPtr.Zero) Then
            Xb.Media.Vlc.Exception.ThrowIfExist("Xb.Media.Vlc.Player.New_1")
        End If

        '描画先オブジェクトのハンドルをセットする。
        [Lib].libvlc_media_player_set_hwnd(Me._handle, renderPanelHandle)
        Xb.Media.Vlc.Exception.ThrowIfExist("Xb.Media.Vlc.Player.New_2")

    End Sub

    Public Function IsDisposing() As Boolean
        Return (Me._isDisposing Or Me._isDisposed)
    End Function

    Public Function IsDisposed() As Boolean
        Return (Me._isDisposed)
    End Function

    ''' <summary>
    ''' UIスレッドで渡し値イベントをレイズする。
    ''' </summary>
    ''' <param name="eventType"></param>
    ''' <param name="args"></param>
    ''' <remarks></remarks>
    Private Sub FireEvent(ByVal eventType As Object, ByVal args As Object)

        'Xb.App.Out("Player.RaiseEventOnUiThread")

        If (Me.IsDisposed()) Then Return

        Select Case eventType.ToString()
            Case "Progressed"
                Dim objAry() As Object
                Try
                    objAry = DirectCast(args, Object())
                    RaiseEvent Progressed(Me, New ProgressEventArgs(DirectCast(objAry(0), Single), _
                                                                    DirectCast(objAry(1), Long)))
                Catch ex As Exception
                    Xb.Util.Out("Xb.Media.Vlc.Player.RaiseEventOnUiThread: イベント用パラメータパースに失敗しました。")
                    Throw New ArgumentException("イベント用パラメータパースに失敗しました。")
                End Try

            Case "Started"
                RaiseEvent Started(Me, New EventArgs())
            Case "Finished"
                RaiseEvent Finished(Me, New EventArgs())
            Case "Paused"
                RaiseEvent Paused(Me, New EventArgs())
                'Case "Canceled"
                '    RaiseEvent Canceled(Me, New EventArgs())
            Case "Expand"
                RaiseEvent Expand(Me, New EventArgs())
            Case "Contracted"
                RaiseEvent Contracted(Me, New EventArgs())
            Case Else
                '何もしない。
        End Select
    End Sub


    ''' <summary>
    ''' ポーズ途中から再生する。
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub Play()

        Xb.Util.Out("Player.Play")

        If (Me.IsDisposing()) Then Return
        Dim result As Integer
        Try
            result = [Lib].libvlc_media_player_play(Me._handle)
            Xb.Media.Vlc.Exception.ThrowIfExist("Xb.Media.Vlc.Player.Play_1")
        Catch ex As Exception
            Xb.Util.Out("Xb.Media.Vlc.Player.Play: メディア再生に失敗しました: " & ex.Message)
            Throw New ApplicationException("メディア再生に失敗しました: " & ex.Message)
        End Try

        _playing = True
        _paused = False

        If (Not Me._timerDelegate Is Nothing) Then Me._timerDelegate = Nothing
        If (Not Me._timer Is Nothing) Then Me._timer.Dispose()
        Me._timerDelegate = New TimerCallback(AddressOf Tick)
        Me._timer = New Timer(Me._timerDelegate, Nothing, 0, 500)

        Xb.Base.SetTimeout(New Xb.Base.TimerDelegate(AddressOf ValidateStart))
    End Sub


    ''' <summary>
    ''' 開始したか否かを検証する。
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub ValidateStart(ParamArray args As Object())

        Xb.Util.Out("Player.ValidateStart")

        If ([Lib].libvlc_media_player_is_playing(Me._handle) = 1) Then
            Me.FireEvent("Started", Nothing)
        Else
            Xb.Base.SetTimeout(New Xb.Base.TimerDelegate(AddressOf ValidateStart))
        End If
    End Sub


    ''' <summary>
    ''' フルスクリーン表示を切り替える。
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub ChangeFullScreen()

        Xb.Util.Out("Player.ChangeFullScreen")

        Dim result As Integer
        If (Me.IsDisposing()) Then Return
        [Lib].libvlc_toggle_fullscreen(Me._handle)
        Xb.Media.Vlc.Exception.ThrowIfExist("Xb.Media.Vlc.Player.ChangeFullScreen_1")

        result = [Lib].libvlc_get_fullscreen(Me._handle)
        Xb.Media.Vlc.Exception.ThrowIfExist("Xb.Media.Vlc.Player.ChangeFullScreen_2")
        If (result = 1) Then
            Me.FireEvent("Expand", Nothing)
        Else
            Me.FireEvent("Contracted", Nothing)
        End If
    End Sub


    ''' <summary>
    ''' ポーズする。
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub Pause()

        Xb.Util.Out("Player.Pause")

        If (Me.IsDisposing()) Then Return
        [Lib].libvlc_media_player_pause(Me._handle)
        Xb.Media.Vlc.Exception.ThrowIfExist("Xb.Media.Vlc.Player.Pause")
        If (Me._playing) Then Me._paused = Me._paused Xor True

        Me.FireEvent("Paused", Nothing)

    End Sub


    ''' <summary>
    ''' タイマーによる再生位置ポーリング処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <remarks></remarks>
    Public Sub Tick(ByVal sender As Object)

        'Xb.App.Out("Player.Tick")

        If (Me.IsDisposing()) Then Return
        Me.FireEvent("Progressed", New Object() {Me.Percentage, Me.Timestamp})

        If (Me.Length > 0 And Me.Length <= Me.Timestamp) Then Me.Dispose()
    End Sub



    ''' <summary>
    ''' 非同期順次破棄２ - プレイヤーを停止
    ''' </summary>
    ''' <remarks>
    ''' Disposeメソッドから呼ばれる。
    ''' </remarks>
    Private Sub DisposeStep2(ParamArray args As Object())

        Xb.Util.Out("Player.DisposeStep2")

        Try
            Dim state As Integer = [Lib].libvlc_media_player_get_state(Me._handle)
            Xb.Media.Vlc.Exception.ThrowIfExist("Xb.Media.Vlc.Player.CloseStep2_1")

            If (1 <= state And state <= 5) Then
                [Lib].libvlc_media_player_pause(Me._handle)
                Xb.Media.Vlc.Exception.ThrowIfExist("Xb.Media.Vlc.Player.CloseStep2_2")

                [Lib].libvlc_media_player_stop(Me._handle)
                Xb.Media.Vlc.Exception.ThrowIfExist("Xb.Media.Vlc.Player.CloseStep3_2")
            End If
        Catch ex As Exception
            Xb.Util.Out("Player.DisposeStep2: Fail DisposeStep2 - " & ex.Message)
        End Try

        Xb.Base.SetTimeout(New Xb.Base.TimerDelegate(AddressOf DisposeStep3), 300)
    End Sub

    ''' <summary>
    ''' 非同期順次破棄３ - プレイヤーを解放
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub DisposeStep3(ParamArray args As Object())

        Xb.Util.Out("Player.DisposeStep3")

        Try
            [Lib].libvlc_media_player_release(Me._handle)
            Xb.Media.Vlc.Exception.ThrowIfExist("Xb.Media.Vlc.Player.CloseStep4")
        Catch ex As Exception
            Xb.Util.Out("Player.DisposeStep2: Fail1 DisposeStep3 - " & ex.Message)
        End Try

        Try
            Me.FireEvent("Finished", Nothing)
        Catch ex As Exception
            Xb.Util.Out("Player.DisposeStep2: Fail2 DisposeStep3 - " & ex.Message)
        End Try

        'Dispose()メソッド内の記述を移動。
        Me._disposedValue = True
        GC.SuppressFinalize(Me)

    End Sub


    Private _disposedValue As Boolean = False        ' 重複する呼び出しを検出するには

#Region " IDisposable Support "
    ' このコードは、破棄可能なパターンを正しく実装できるように Visual Basic によって追加されました。
    Public Sub Dispose() Implements IDisposable.Dispose
        ' このコードを変更しないでください。クリーンアップ コードを上の Dispose(ByVal disposing As Boolean) に記述します。

        Xb.Util.Out("Player.Dispose")

        '進捗イベントレイズ用タイマーを破棄
        Me._isDisposing = True
        If (Not Me._timerDelegate Is Nothing) Then Me._timerDelegate = Nothing
        If (Not Me._timer Is Nothing) Then Me._timer.Dispose()
        Me._timer = Nothing

        Xb.Base.SetTimeout(New Xb.Base.TimerDelegate(AddressOf DisposeStep2), 200)
    End Sub
#End Region

End Class
