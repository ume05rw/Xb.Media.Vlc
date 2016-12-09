Option Strict On

''' <summary>
''' ストリーミングサービス管理クラス
''' </summary>
''' <remarks></remarks>
Public Class Service
    Implements IDisposable

    Private _coreHandle As IntPtr
    Private _mediaList As Dictionary(Of String, String)

    ''' <summary>
    ''' コンストラクタ
    ''' </summary>
    ''' <param name="core"></param>
    ''' <remarks></remarks>
    Public Sub New(ByVal core As Xb.Media.Vlc.Core)

        If (AppDomain.CurrentDomain.FriendlyName <> "DefaultDomain") Then
            If (core Is Nothing) Then
                Xb.Util.Out("VLC-Coreインスタンスが認識できません。")
                Throw New ArgumentException("VLC-Coreインスタンスが認識できません。")
            End If

            Me._coreHandle = core.Handle
        End If

        Me._mediaList = New Dictionary(Of String, String)()
                
    End Sub


    ''' <summary>
    ''' ストリーミングサーバを開始する。
    ''' </summary>
    ''' <param name="fileName"></param>
    ''' <param name="port"></param>
    ''' <remarks></remarks>
    Public Sub Open(ByVal fileName As String, _
                    ByVal port As Integer)

        Xb.Util.Out("Xb.Media.Vlc.Service.Open")

        If (fileName Is Nothing) Then
            Xb.Util.Out("ファイル名が認識できません。")
            Throw New ArgumentException("ファイル名が認識できません。")
        End If

        If (Not IO.File.Exists(fileName)) Then
            Xb.Util.Out("指定ファイルの存在が確認出来ません。")
            Throw New ArgumentException("指定ファイルの存在が確認出来ません。")
        End If

        Dim mediaName, mrl, sout As String, _
            res As Integer

        mediaName = Xb.Base.GetRandomString(20, Xb.Base.RandomStringType.NumAlpha)
        Me._mediaList.Add(fileName, mediaName)

        '
        'オーディオコーデックが"mp4a"では動作しない。
        'また、vlc.exeの画面でストリーミングしたときのオプションはNG。
        '
        mrl = "file:///" & fileName.Replace("\", "/")
        sout = "#transcode{" _
                & "vcodec=h264,vb=800,width=640,height=480,scale=1," _
                & "acodec=mpga,ab=128,channels=2,samplerate=44100" _
                & "}:" _
                & "rtp{sdp=rtsp://:" & port.ToString() & "/}"

        'Xb.App.Out("sout: " & vbCrLf & sout)

        res = Xb.Media.Vlc.Lib.libvlc_vlm_add_broadcast(Me._coreHandle, _
                                                        mediaName, _
                                                        mrl, _
                                                        sout, _
                                                        0, Nothing, 1, 0)

        Xb.Media.Vlc.Exception.ThrowIfExist("Xb.Media.Vlc.Service.StartServer_1")
        If (res <> 0) Then
            Xb.Util.Out("FAIL: libvlc_vlm_add_broadcast")
        End If

        res = Xb.Media.Vlc.Lib.libvlc_vlm_play_media(Me._coreHandle, _
                                                    mediaName)

        Xb.Media.Vlc.Exception.ThrowIfExist("Xb.Media.Vlc.Service.StartServer_2")
        If (res <> 0) Then
            Xb.Util.Out("FAIL: libvlc_vlm_play_media")
        End If

    End Sub


    ''' <summary>
    ''' メディア名からファイル名を取得する。
    ''' </summary>
    ''' <param name="mediaName"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetFileName(ByVal mediaName As String) As String

        For Each fileName As String In Me._mediaList.Keys
            If (Me._mediaList.Item(fileName) = mediaName) Then Return fileName
        Next

        Return Nothing

    End Function


    ''' <summary>
    ''' 有効／無効を設定する。
    ''' </summary>
    ''' <param name="fileName"></param>
    ''' <param name="enable"></param>
    ''' <remarks></remarks>
    Public Sub SetEnable(ByVal fileName As String, ByVal enable As Boolean)

        Xb.Util.Out("Xb.Media.Vlc.Service.SetEnable")

        Dim result As Integer, _
            mediaName As String

        If (Not Me._mediaList.ContainsKey(fileName)) Then
            Xb.Util.Out("サービス中のファイルではありません：" & fileName)
            Throw New ArgumentException("サービス中のファイルではありません：" & fileName)
        End If

        mediaName = Me._mediaList.Item(fileName)

        result = Xb.Media.Vlc.Lib.libvlc_vlm_set_enabled(Me._coreHandle, _
                                                            mediaName, _
                                                            CInt(IIf(enable, 1, 0)))
        Xb.Media.Vlc.Exception.ThrowIfExist("Xb.Media.Vlc.Service.SetEnable")

    End Sub


    '''' <summary>
    '''' サービス中メディアの情報を取得する。
    '''' </summary>
    '''' <param name="fileName"></param>
    '''' <returns></returns>
    '''' <remarks></remarks>
    'Public Function GetInfo(ByVal fileName As String) As Dictionary(Of String, Object)
    '    Xb.Util.Out("Xb.Media.Vlc.Service.SetEnable")

    '    Dim mediaName, infoJson As String, _
    '        result As Dictionary(Of String, Object)

    '    If (Not Me._mediaList.ContainsKey(fileName)) Then
    '        Xb.Util.Out("サービス中のファイルではありません：" & fileName)
    '        Throw New ArgumentException("サービス中のファイルではありません：" & fileName)
    '    End If

    '    mediaName = Me._mediaList.Item(fileName)

    '    infoJson = Xb.Media.Vlc.Lib.libvlc_vlm_show_media(Me._coreHandle, mediaName)
    '    Xb.Media.Vlc.Exception.ThrowIfExist("Xb.Media.Vlc.Service.SetEnable")

    '    result = Xb.Type.Json.Parse(infoJson)

    '    Return result

    'End Function


    ''' <summary>
    ''' ストリーミングサーバを停止する。
    ''' </summary>
    ''' <param name="fileName"></param>
    ''' <remarks></remarks>
    Public Sub Close(ByVal fileName As String)

        Xb.Util.Out("Xb.Media.Vlc.Service.Close")

        Dim result As Integer, _
            mediaName As String

        If (Not Me._mediaList.ContainsKey(fileName)) Then
            Xb.Util.Out("サービス中のファイルではありません：" & fileName)
            Throw New ArgumentException("サービス中のファイルではありません：" & fileName)
        End If

        mediaName = Me._mediaList.Item(fileName)
                
        result = Xb.Media.Vlc.Lib.libvlc_vlm_stop_media(Me._coreHandle, mediaName)
        Xb.Media.Vlc.Exception.ThrowIfExist("Xb.Media.Vlc.Service.Close_1")

        Xb.Media.Vlc.Lib.libvlc_vlm_release(Me._coreHandle)
        Xb.Media.Vlc.Exception.ThrowIfExist("Xb.Media.Vlc.Service.Close_2")

        Me._mediaList.Remove(fileName)

    End Sub


    ''' <summary>
    ''' ストリーミング処理を全て停止する。
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub CloseAll()

        Xb.Util.Out("Xb.Media.Vlc.Service.CloseAll")

        Dim result As Integer

        For Each mediaName As String In Me._mediaList.Values

            result = Xb.Media.Vlc.Lib.libvlc_vlm_stop_media(Me._coreHandle, mediaName)
            Xb.Media.Vlc.Exception.ThrowIfExist("Xb.Media.Vlc.Service.CloseAll_1")

            Xb.Media.Vlc.Lib.libvlc_vlm_release(Me._coreHandle)
            Xb.Media.Vlc.Exception.ThrowIfExist("Xb.Media.Vlc.Service.CloseAll_2")
        Next
        Me._mediaList = New Dictionary(Of String, String)()

    End Sub


#Region "IDisposable Support"
    Private _disposedValue As Boolean ' 重複する呼び出しを検出するには

    ' IDisposable
    Protected Overridable Sub Dispose(disposing As Boolean)
        If Not Me._disposedValue Then
            If disposing Then
                Me.CloseAll()
                Me._mediaList = Nothing
            End If
        End If
        Me._disposedValue = True
    End Sub

    ' このコードは、破棄可能なパターンを正しく実装できるように Visual Basic によって追加されました。
    Public Sub Dispose() Implements IDisposable.Dispose
        ' このコードを変更しないでください。クリーンアップ コードを上の Dispose(ByVal disposing As Boolean) に記述します。
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub
#End Region

End Class


'以下、sout文字列の設定例サンプル
'"#transcode{vcodec=h264,vb=0,scale=0,acodec=mp4a,ab=128,channels=2,samplerate=44100}:rtp{sdp=rtsp://:5544/}" '<-この記述が最も多い。
'"#transcode{vcodec=h264,vb=800,scale=1,acodec=mpga,ab=128,channels=2,samplerate=44100}:http{mux=ts,dst=:7777/}"
'"#transcode{vcodec=h264,venc=x264,vb=0,scale=0,acodec=mpga,ab=128,channels=2, samplerate=44100}:http{mux=ffmpeg{mux=flv},dst=:7777/}"
'"#transcode{vcodec=mp4v,vb=800,scale=1,acodec=mp4a,ab=128,channels=2,samplerate=44100}:duplicate{dst=file{dst=D:\file.mp4},dst=display}"
'"#transcode{acodec=mp3,ab=128,channels=2,samplerate=44100}:http{dst=:8090/go.mp3}"
'"#transcode{vcodec=h264,vb=800,scale=1}:duplicate{dst=std{access=udp,mux=ts,dst=127.0.0.1:1234},dst=display}"
'"#transcode{acodec=vorb,channels=2,samplerate=44100}:std{access=file,mux=ogg}"
'"#transcode{acodec=s16l,channels=2}:std{access=file,mux=wav,dst=\\\\Server\Qmultimedia\Music\song_c38.wav}"
'"#transcode{acodec=flac}:std{mux=raw,dst=\\\\Server\Qmultimedia\Music\song_c38.flac}" 
'"#transcode{acodec=s16l,channels=2}:std{access=file,mux=wav,dst=%DestPrefix%_c%%i.wav}"

'"#transcode{vcodec=DIV3,vb=1024,scale=1,acodec=mpga,ab=192,channels=2}:duplicate{dst=std{access=http,mux=ts,dst=:8080}}"
' ↓
'outputs : http://mmdmmo.ddo.jp:8080
'Encapsulation(Method) : MPEG(TS)
'Transcoding(options)
'ビデオコーデック : DIV3 ビットレート : 1024kb/s スケール : 1
'オーディオコーデック : mpga ビットレート : 192kb/s チャンネル : 2
'情報：ストリーム０→I420 ストリーム１→araw

'"#transcode{vcodec=mp1v,vb=40,width=160,height=120,acodec=mpga,channels=1,ab=32}:std{access=http,mux=mpeg1,url=192.168.0.2:80}"
' ↓
' width, height = エンコード後の画面サイズ
' vb = 画像ビットレート(キロビット/秒)
' ab = 音声ビットレート(キロビット/秒) モノラル32, ステレオ64が最低値
' channels = モノラル: 1, ステレオ: 2

'"#transcode{vcodec=h264,vb=400,fps=15,scale=1,acodec=mp4a,ab=128,channels=2,samplerate=44100,scodec=dvbs}:rtp{dst=127.0.0.1,port=1234,mux=ts,sdp=file://c:\vlc.sdp}"
