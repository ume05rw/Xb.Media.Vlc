Option Strict On

Imports System.Runtime.InteropServices

''' <summary>
''' VLCライブラリラッパ
''' </summary>
''' <remarks>
''' http://www.videolan.org/developers/vlc/doc/doxygen/html/group__libvlc.html
''' 
''' 注意：
''' .NetFW2.0では不要だった"CallingConvention"属性の指定が、.NetFW4.0で必要になったものがある。
''' ※.NetFW3.5実装サンプルでは見られなかった。
'''  Cdecl   : 引数／戻り値が可変長
'''  StdCall : (未指定時のデフォルト) 固定長
''' 
''' ハンドル渡しメソッドには全て必要になったらしい。
''' ハンドル指定時はハンドルID数値でなく、ハンドルで指定された構造体を渡しているのか？
''' </remarks>
Public NotInheritable Class [Lib]


    '#######################################################################################
    ''' <summary>
    ''' VLCオブジェクトを生成する。
    ''' </summary>
    ''' <param name="argLength"></param>
    ''' <param name="args"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' CallingConvention:=CallingConvention.Cdecl 
    ''' 　->引数が配列など、可変値のときに使用する宣言。
    ''' </remarks>
    <DllImport("libvlc", CallingConvention:=CallingConvention.Cdecl)> _
    <System.Security.SuppressUnmanagedCodeSecurity()> _
    Public Shared Function libvlc_new(ByVal argLength As Integer, _
                                    <MarshalAs(UnmanagedType.LPArray, ArraySubType:=UnmanagedType.LPStr)> ByVal args As String()) As IntPtr
    End Function

    ''' <summary>
    ''' VLCオブジェクトを解放する。
    ''' </summary>
    ''' <param name="instanceHandle"></param>
    ''' <remarks></remarks>
    <DllImport("libvlc", CallingConvention:=CallingConvention.Cdecl)> _
    <System.Security.SuppressUnmanagedCodeSecurity()> _
    Public Shared Sub libvlc_release(ByVal instanceHandle As IntPtr)
    End Sub

    ''' <summary>
    ''' VLCオブジェクトのバージョンを取得する。
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DllImport("libvlc")> _
    <System.Security.SuppressUnmanagedCodeSecurity()> _
    Public Shared Function libvlc_get_version() As String
    End Function


    '#######################################################################################
    ''' <summary>
    ''' メディアオブジェクトを、URLを参照して生成する。
    ''' </summary>
    ''' <param name="instanceHandle"></param>
    ''' <param name="fileName"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DllImport("libvlc", CallingConvention:=CallingConvention.Cdecl)> _
    <System.Security.SuppressUnmanagedCodeSecurity()> _
    Public Shared Function libvlc_media_new_location(ByVal instanceHandle As IntPtr, _
                                                    <MarshalAs(UnmanagedType.CustomMarshaler, _
                                                        MarshalTypeRef:=GetType(Xb.Type.Marshaler.Utf8Marshaler))> _
                                                        ByVal fileName As String _
                                                    ) As IntPtr
    End Function


    ''' <summary>
    ''' メディアオブジェクトを、ローカルパスからメディアファイルを参照して生成する。
    ''' </summary>
    ''' <param name="instanceHandle"></param>
    ''' <param name="url"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DllImport("libvlc", CallingConvention:=CallingConvention.Cdecl)> _
    <System.Security.SuppressUnmanagedCodeSecurity()> _
    Public Shared Function libvlc_media_new_path(ByVal instanceHandle As IntPtr, _
                                                <MarshalAs(UnmanagedType.CustomMarshaler, _
                                                    MarshalTypeRef:=GetType(Xb.Type.Marshaler.Utf8Marshaler))> _
                                                    ByVal url As String _
                                                ) As IntPtr
    End Function


    ''' <summary>
    ''' メディアオブジェクトの状態を取得する。
    ''' </summary>
    ''' <param name="mediaHandle"></param>
    ''' <remarks></remarks>
    <DllImport("libvlc", CallingConvention:=CallingConvention.Cdecl)> _
    <System.Security.SuppressUnmanagedCodeSecurity()> _
    Public Shared Function libvlc_media_get_state(ByVal mediaHandle As IntPtr) As Integer
    End Function

    ''' <summary>
    ''' メタデータとトラック情報を読み込む。
    ''' </summary>
    ''' <param name="mediaHandle"></param>
    ''' <remarks></remarks>
    <DllImport("libvlc", CallingConvention:=CallingConvention.Cdecl)> _
    <System.Security.SuppressUnmanagedCodeSecurity()> _
    Public Shared Sub libvlc_media_parse(ByVal mediaHandle As IntPtr)
    End Sub

    ''' <summary>
    ''' デュレーションを取得する。
    ''' </summary>
    ''' <param name="mediaHandle"></param>
    ''' <remarks></remarks>
    <DllImport("libvlc", CallingConvention:=CallingConvention.Cdecl)> _
    <System.Security.SuppressUnmanagedCodeSecurity()> _
    Public Shared Function libvlc_media_get_duration(ByVal mediaHandle As IntPtr) As Long
    End Function



    ''' <summary>
    ''' メディアオブジェクトを解放する。
    ''' </summary>
    ''' <param name="mediaHandle"></param>
    ''' <remarks></remarks>
    <DllImport("libvlc", CallingConvention:=CallingConvention.Cdecl)> _
    <System.Security.SuppressUnmanagedCodeSecurity()> _
    Public Shared Sub libvlc_media_release(ByVal mediaHandle As IntPtr)
    End Sub

    '#######################################################################################
    ''' <summary>
    ''' プレイヤーオブジェクトを生成する。
    ''' </summary>
    ''' <param name="mediaHandle"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DllImport("libvlc", CallingConvention:=CallingConvention.Cdecl)> _
    <System.Security.SuppressUnmanagedCodeSecurity()> _
    Public Shared Function libvlc_media_player_new_from_media(ByVal mediaHandle As IntPtr) As IntPtr
    End Function

    ''' <summary>
    ''' プレイヤーオブジェクトを解放する。
    ''' </summary>
    ''' <param name="playerHandle"></param>
    ''' <remarks></remarks>
    <DllImport("libvlc", CallingConvention:=CallingConvention.Cdecl)> _
    <System.Security.SuppressUnmanagedCodeSecurity()> _
    Public Shared Sub libvlc_media_player_release(ByVal playerHandle As IntPtr)
    End Sub

    ''' <summary>
    ''' プレイヤーの描画先オブジェクトをセットする。
    ''' </summary>
    ''' <param name="playerHandle"></param>
    ''' <param name="viewControlHandle"></param>
    ''' <remarks></remarks>
    <DllImport("libvlc", CallingConvention:=CallingConvention.Cdecl)> _
    <System.Security.SuppressUnmanagedCodeSecurity()> _
    Public Shared Sub libvlc_media_player_set_hwnd(ByVal playerHandle As IntPtr, ByVal viewControlHandle As IntPtr)
    End Sub

    ''' <summary>
    ''' プレイヤーからメディアオブジェクトを取得する。
    ''' </summary>
    ''' <param name="playerHandle"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DllImport("libvlc", CallingConvention:=CallingConvention.Cdecl)> _
    <System.Security.SuppressUnmanagedCodeSecurity()> _
    Public Shared Function libvlc_media_player_get_media(ByVal playerHandle As IntPtr) As IntPtr
    End Function

    ''' <summary>
    ''' プレイヤーにメディアをセットする。
    ''' </summary>
    ''' <param name="playerHandle"></param>
    ''' <param name="mediaHandle"></param>
    ''' <remarks></remarks>
    <DllImport("libvlc", CallingConvention:=CallingConvention.Cdecl)> _
    <System.Security.SuppressUnmanagedCodeSecurity()> _
    Public Shared Sub libvlc_media_player_set_media(ByVal playerHandle As IntPtr, ByVal mediaHandle As IntPtr)
    End Sub

    ''' <summary>
    ''' 再生が可能か否か調べる。
    ''' </summary>
    ''' <param name="playerHandle"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DllImport("libvlc", CallingConvention:=CallingConvention.Cdecl)> _
    <System.Security.SuppressUnmanagedCodeSecurity()> _
    Public Shared Function libvlc_media_player_will_play(ByVal playerHandle As IntPtr) As Integer
    End Function

    ''' <summary>
    ''' プレイヤーを開始する。
    ''' </summary>
    ''' <param name="playerHandle"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DllImport("libvlc", CallingConvention:=CallingConvention.Cdecl)> _
    <System.Security.SuppressUnmanagedCodeSecurity()> _
    Public Shared Function libvlc_media_player_play(ByVal playerHandle As IntPtr) As Integer
    End Function

    ''' <summary>
    ''' プレイヤーを中断する。
    ''' </summary>
    ''' <param name="playerHandle"></param>
    ''' <remarks></remarks>
    <DllImport("libvlc", CallingConvention:=CallingConvention.Cdecl)> _
    <System.Security.SuppressUnmanagedCodeSecurity()> _
    Public Shared Sub libvlc_media_player_pause(ByVal playerHandle As IntPtr)
    End Sub

    ''' <summary>
    ''' プレイヤーを停止する。
    ''' </summary>
    ''' <param name="playerHandle"></param>
    ''' <remarks></remarks>
    <DllImport("libvlc", CallingConvention:=CallingConvention.Cdecl)> _
    <System.Security.SuppressUnmanagedCodeSecurity()> _
    Public Shared Sub libvlc_media_player_stop(ByVal playerHandle As IntPtr)
    End Sub

    ''' <summary>
    ''' プレイヤーの動作状態を取得する。
    ''' </summary>
    ''' <param name="playerHandle"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' IDLE/CLOSE=0, 
    ''' OPENING=1, 
    ''' BUFFERING=2, 
    ''' PLAYING=3, 
    ''' PAUSED=4, 
    ''' STOPPING=5, 
    ''' ENDED=6, 
    ''' ERROR=7 
    ''' </remarks>
    <DllImport("libvlc", CallingConvention:=CallingConvention.Cdecl)> _
    <System.Security.SuppressUnmanagedCodeSecurity()> _
    Public Shared Function libvlc_media_player_get_state(ByVal playerHandle As IntPtr) As Integer
    End Function

    ''' <summary>
    ''' プレイヤーが再生中か否か。
    ''' </summary>
    ''' <param name="playerHandle"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' 1 = 再生中、 0 = その他
    ''' </remarks>
    <DllImport("libvlc", CallingConvention:=CallingConvention.Cdecl)> _
    <System.Security.SuppressUnmanagedCodeSecurity()> _
    Public Shared Function libvlc_media_player_is_playing(ByVal playerHandle As IntPtr) As Integer
    End Function

    ''' <summary>
    ''' ボリュームを設定値を取得する。
    ''' </summary>
    ''' <param name="playerHandle"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' (0 = mute, 100 = nominal / 0dB) 
    ''' </remarks>
    <DllImport("libvlc", CallingConvention:=CallingConvention.Cdecl)> _
    <System.Security.SuppressUnmanagedCodeSecurity()> _
    Public Shared Function libvlc_audio_get_volume(ByVal playerHandle As IntPtr) As Integer
    End Function

    ''' <summary>
    ''' ボリュームを設定する。
    ''' </summary>
    ''' <param name="playerHandle"></param>
    ''' <param name="volume"></param>
    ''' <returns></returns>
    ''' <remarks>
    '''  (0 = mute, 100 = nominal / 0dB) 
    ''' </remarks>
    <DllImport("libvlc", CallingConvention:=CallingConvention.Cdecl)> _
    <System.Security.SuppressUnmanagedCodeSecurity()> _
    Public Shared Function libvlc_audio_set_volume(ByVal playerHandle As IntPtr, ByVal volume As Integer) As Integer
    End Function

    ''' <summary>
    ''' ミュート設定値を取得する。
    ''' </summary>
    ''' <param name="playerHandle"></param>
    ''' <returns></returns>
    ''' <remarks>
    '''  status is true then mute, otherwise unmute 
    ''' </remarks>
    <DllImport("libvlc", CallingConvention:=CallingConvention.Cdecl)> _
    <System.Security.SuppressUnmanagedCodeSecurity()> _
    Public Shared Function libvlc_audio_get_mute(ByVal playerHandle As IntPtr) As Integer
    End Function

    ''' <summary>
    ''' ミュート設定を行う。
    ''' </summary>
    ''' <param name="playerHandle"></param>
    ''' <param name="status"></param>
    ''' <remarks>
    ''' status is true then mute, otherwise unmute 
    ''' </remarks>
    <DllImport("libvlc", CallingConvention:=CallingConvention.Cdecl)> _
    <System.Security.SuppressUnmanagedCodeSecurity()> _
    Public Shared Sub libvlc_audio_set_mute(ByVal playerHandle As IntPtr, ByVal status As Integer)
    End Sub

    ''' <summary>
    ''' フルスクリーン設定値を取得する。
    ''' </summary>
    ''' <param name="playerHandle"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DllImport("libvlc", CallingConvention:=CallingConvention.Cdecl)> _
    <System.Security.SuppressUnmanagedCodeSecurity()> _
    Public Shared Function libvlc_get_fullscreen(ByVal playerHandle As IntPtr) As Integer
    End Function

    ''' <summary>
    ''' フルスクリーン表示設定を行う。
    ''' </summary>
    ''' <param name="playerHandle"></param>
    ''' <param name="fullscreenValue"></param>
    ''' <remarks></remarks>
    <DllImport("libvlc", CallingConvention:=CallingConvention.Cdecl)> _
    <System.Security.SuppressUnmanagedCodeSecurity()> _
    Public Shared Sub libvlc_set_fullscreen(ByVal playerHandle As IntPtr, ByVal fullscreenValue As Boolean)
    End Sub

    ''' <summary>
    ''' フルスクリーン表示を切り変える。
    ''' </summary>
    ''' <param name="playerHandle"></param>
    ''' <remarks></remarks>
    <DllImport("libvlc", CallingConvention:=CallingConvention.Cdecl)> _
    <System.Security.SuppressUnmanagedCodeSecurity()> _
    Public Shared Sub libvlc_toggle_fullscreen(ByVal playerHandle As IntPtr)
    End Sub

    ''' <summary>
    ''' シーク可能か否かを取得する。
    ''' </summary>
    ''' <param name="playerHandle"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DllImport("libvlc", CallingConvention:=CallingConvention.Cdecl)> _
    <System.Security.SuppressUnmanagedCodeSecurity()> _
    Public Shared Function libvlc_media_player_is_seekable(ByVal playerHandle As IntPtr) As Integer
    End Function

    ''' <summary>
    ''' 再生位置を取得する。
    ''' </summary>
    ''' <param name="playerHandle"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' 位置をパーセンテージ(0.0 ～ 1.0)で示す。
    ''' </remarks>
    <DllImport("libvlc", CallingConvention:=CallingConvention.Cdecl)> _
    <System.Security.SuppressUnmanagedCodeSecurity()> _
    Public Shared Function libvlc_media_player_get_position(ByVal playerHandle As IntPtr) As Single
    End Function

    ''' <summary>
    ''' 再生位置をセットする。
    ''' </summary>
    ''' <param name="playerHandle"></param>
    ''' <param name="position"></param>
    ''' <remarks>
    ''' 位置をパーセンテージ(0.0 ～ 1.0)でセットする。
    ''' </remarks>
    <DllImport("libvlc", CallingConvention:=CallingConvention.Cdecl)> _
    <System.Security.SuppressUnmanagedCodeSecurity()> _
    Public Shared Sub libvlc_media_player_set_position(ByVal playerHandle As IntPtr, ByVal position As Single)
    End Sub

    ''' <summary>
    ''' カレントメディアの長さを取得する。
    ''' </summary>
    ''' <param name="playerHandle"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DllImport("libvlc", CallingConvention:=CallingConvention.Cdecl)> _
    <System.Security.SuppressUnmanagedCodeSecurity()> _
    Public Shared Function libvlc_media_player_get_length(ByVal playerHandle As IntPtr) As Long
    End Function

    ''' <summary>
    ''' タイムスタンプを取得する。
    ''' </summary>
    ''' <param name="playerHandle"></param>
    ''' <returns></returns>
    ''' <remarks>単位：MSec</remarks>
    <DllImport("libvlc", CallingConvention:=CallingConvention.Cdecl)> _
    <System.Security.SuppressUnmanagedCodeSecurity()> _
    Public Shared Function libvlc_media_player_get_time(ByVal playerHandle As IntPtr) As Long
    End Function

    ''' <summary>
    ''' タイムスタンプをセットする。
    ''' </summary>
    ''' <param name="playerHandle"></param>
    ''' <param name="time"></param>
    ''' <remarks>単位：MSec</remarks>
    <DllImport("libvlc", CallingConvention:=CallingConvention.Cdecl)> _
    <System.Security.SuppressUnmanagedCodeSecurity()> _
    Public Shared Sub libvlc_media_player_set_time(ByVal playerHandle As IntPtr, ByVal time As Long)
    End Sub


    '#######################################################################################
    ''' <summary>
    ''' ストリームサーバを初期化する。
    ''' </summary>
    ''' <param name="instanceHandle"></param>
    ''' <param name="mediaName"></param>
    ''' <param name="acceptUrl"></param>
    ''' <param name="outputParam"></param>
    ''' <param name="additionalOptNum"></param>
    ''' <param name="additionalOptVal"></param>
    ''' <param name="isNewBroadcast"></param>
    ''' <param name="isLoop"></param>
    ''' <remarks></remarks>
    <DllImport("libvlc", CallingConvention:=CallingConvention.Cdecl)> _
    <System.Security.SuppressUnmanagedCodeSecurity()> _
    Public Shared Function libvlc_vlm_add_broadcast(ByVal instanceHandle As IntPtr, _
                                                    ByVal mediaName As String, _
                                                    <MarshalAs(UnmanagedType.CustomMarshaler, _
                                                        MarshalTypeRef:=GetType(Xb.Type.Marshaler.Utf8Marshaler))> _
                                                        ByVal acceptUrl As String, _
                                                    ByVal outputParam As String, _
                                                    ByVal additionalOptNum As Integer, _
                                                    ByVal additionalOptVal As String, _
                                                    ByVal isNewBroadcast As Integer, _
                                                    ByVal isLoop As Integer) As Integer
    End Function

    ''' <summary>
    ''' サーバを開始する。
    ''' </summary>
    ''' <param name="instanceHandle"></param>
    ''' <param name="name"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DllImport("libvlc", CallingConvention:=CallingConvention.Cdecl)> _
    <System.Security.SuppressUnmanagedCodeSecurity()> _
    Public Shared Function libvlc_vlm_play_media(ByVal instanceHandle As IntPtr, ByVal name As String) As Integer
    End Function

    ''' <summary>
    ''' 有効化／無効化設定を行う。
    ''' </summary>
    ''' <param name="instanceHandle"></param>
    ''' <param name="name"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' enabled = ?数値定義を探す。
    ''' </remarks>
    <DllImport("libvlc", CallingConvention:=CallingConvention.Cdecl)> _
    <System.Security.SuppressUnmanagedCodeSecurity()> _
    Public Shared Function libvlc_vlm_set_enabled(ByVal instanceHandle As IntPtr, ByVal name As String, ByVal enabled As Integer) As Integer
    End Function
            
    ''' <summary>
    ''' メディアの再生位置を設定する。
    ''' </summary>
    ''' <param name="instanceHandle"></param>
    ''' <param name="name"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' </remarks>
    <DllImport("libvlc", CallingConvention:=CallingConvention.Cdecl)> _
    <System.Security.SuppressUnmanagedCodeSecurity()> _
    Public Shared Function libvlc_vlm_seek_media(ByVal instanceHandle As IntPtr, ByVal name As String, ByVal percentage As Single) As Integer
    End Function


    ''' <summary>
    ''' メディア情報JSONを取得する。
    ''' </summary>
    ''' <param name="instanceHandle"></param>
    ''' <param name="name"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' 第二引数はアプリケーション上の縛りでASCII文字列しか使用しないのでマーシャリングは不要だが、
    ''' 渡し値／戻り値それぞれをカスタムマーシャリングする記述サンプルとして残しておく。
    ''' </remarks>
    <DllImport("libvlc", CallingConvention:=CallingConvention.Cdecl)> _
    <System.Security.SuppressUnmanagedCodeSecurity()> _
    Public Shared Function libvlc_vlm_show_media(ByVal instanceHandle As IntPtr, _
                                                <MarshalAs(UnmanagedType.CustomMarshaler, _
                                                    MarshalTypeRef:=GetType(Xb.Type.Marshaler.Utf8Marshaler))> _
                                                    ByVal name As String _
                                                    ) As <MarshalAs(UnmanagedType.CustomMarshaler, _
                                                    MarshalTypeRef:=GetType(Xb.Type.Marshaler.Utf8Marshaler))> String
    End Function

    ''' <summary>
    ''' サーバを停止する。
    ''' </summary>
    ''' <param name="instanceHandle"></param>
    ''' <param name="name"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DllImport("libvlc", CallingConvention:=CallingConvention.Cdecl)> _
    <System.Security.SuppressUnmanagedCodeSecurity()> _
    Public Shared Function libvlc_vlm_stop_media(ByVal instanceHandle As IntPtr, ByVal name As String) As Integer
    End Function

    ''' <summary>
    ''' サーバリソースを解放する。
    ''' </summary>
    ''' <param name="instanceHandle"></param>
    ''' <remarks></remarks>
    <DllImport("libvlc", CallingConvention:=CallingConvention.Cdecl)> _
    <System.Security.SuppressUnmanagedCodeSecurity()> _
    Public Shared Sub libvlc_vlm_release(ByVal instanceHandle As IntPtr)
    End Sub


    '#######################################################################################
    ''' <summary>
    ''' 保持中のエラーメッセージを取得する。
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DllImport("libvlc")> _
    <System.Security.SuppressUnmanagedCodeSecurity()> _
    Public Shared Function libvlc_errmsg() As IntPtr
    End Function

    ''' <summary>
    ''' エラーキャッシュを削除する。
    ''' </summary>
    ''' <remarks></remarks>
    <DllImport("libvlc")> _
    <System.Security.SuppressUnmanagedCodeSecurity()> _
    Public Shared Sub libvlc_clearerr()
    End Sub

    '''' <summary>
    '''' メディア情報構造体を取得する。
    '''' </summary>
    '''' <param name="instanceHandle"></param>
    '''' <param name="tracks"></param>
    '''' <returns></returns>
    '''' <remarks></remarks>
    '<DllImport("libvlc")> _
    '<System.Security.SuppressUnmanagedCodeSecurity()> _
    'Public Shared Function libvlc_media_tracks_get(ByVal instanceHandle As IntPtr, _
    '                                    ByRef tracks As libvlc_media_track_t) As Integer
    'End Function

    '動画情報保持用構造体定義
    'Ver2.2での定義に基づいた記述だが、Ver1.2では動作しなかった。
    '<StructLayout(LayoutKind.Explicit)> _
    'Public Structure libvlc_media_track_t
    '    <FieldOffset(0)> _
    '    Public i_codec As UInteger
    '    <FieldOffset(4)> _
    '    Public i_original_fourcc As UInteger
    '    <FieldOffset(8)> _
    '    Public i_id As Integer
    '    <FieldOffset(12)> _
    '    Public i_type As libvlc_track_type_t
    '    <FieldOffset(16)> _
    '    Public i_profile As Integer
    '    <FieldOffset(20)> _
    '    Public i_level As Integer
    '    <FieldOffset(24)> _
    '    Public audio As libvlc_audio_track_t
    '    <FieldOffset(24)> _
    '    Public video As libvlc_video_track_t
    '    <FieldOffset(24)> _
    '    Public subtitle As libvlc_subtitle_track_t
    '    <FieldOffset(48)> _
    '    Public i_bitrate As UInteger
    '    <FieldOffset(52)> _
    '    Public psz_language As IntPtr
    '    <FieldOffset(56)> _
    '    Public psz_description As IntPtr
    'End Structure

    '<StructLayout(LayoutKind.Sequential)> _
    'Public Structure libvlc_audio_track_t
    '    Public i_channels As UInteger
    '    Public i_rate As UInteger
    'End Structure

    '<StructLayout(LayoutKind.Sequential)> _
    'Public Structure libvlc_video_track_t
    '    Public i_height As UInteger
    '    Public i_width As UInteger
    '    Public i_sar_num As UInteger
    '    Public i_sar_den As UInteger
    '    Public i_frame_rate_num As UInteger
    '    Public i_frame_rate_den As UInteger
    'End Structure

    '<StructLayout(LayoutKind.Sequential)> _
    'Public Structure libvlc_subtitle_track_t
    '    Public psz_encoding As IntPtr
    'End Structure

    'Public Enum libvlc_track_type_t
    '    libvlc_track_unknown = -1
    '    libvlc_track_audio = 0
    '    libvlc_track_video = 1
    '    libvlc_track_text = 2
    'End Enum 

End Class
