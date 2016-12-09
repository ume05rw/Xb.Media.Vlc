Option Strict On

Imports System.Runtime.InteropServices

''' <summary>
''' VLC���C�u�������b�p
''' </summary>
''' <remarks>
''' http://www.videolan.org/developers/vlc/doc/doxygen/html/group__libvlc.html
''' 
''' ���ӁF
''' .NetFW2.0�ł͕s�v������"CallingConvention"�����̎w�肪�A.NetFW4.0�ŕK�v�ɂȂ������̂�����B
''' ��.NetFW3.5�����T���v���ł͌����Ȃ������B
'''  Cdecl   : �����^�߂�l���ϒ�
'''  StdCall : (���w�莞�̃f�t�H���g) �Œ蒷
''' 
''' �n���h���n�����\�b�h�ɂ͑S�ĕK�v�ɂȂ����炵���B
''' �n���h���w�莞�̓n���h��ID���l�łȂ��A�n���h���Ŏw�肳�ꂽ�\���̂�n���Ă���̂��H
''' </remarks>
Public NotInheritable Class [Lib]


    '#######################################################################################
    ''' <summary>
    ''' VLC�I�u�W�F�N�g�𐶐�����B
    ''' </summary>
    ''' <param name="argLength"></param>
    ''' <param name="args"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' CallingConvention:=CallingConvention.Cdecl 
    ''' �@->�������z��ȂǁA�ϒl�̂Ƃ��Ɏg�p����錾�B
    ''' </remarks>
    <DllImport("libvlc", CallingConvention:=CallingConvention.Cdecl)> _
    <System.Security.SuppressUnmanagedCodeSecurity()> _
    Public Shared Function libvlc_new(ByVal argLength As Integer, _
                                    <MarshalAs(UnmanagedType.LPArray, ArraySubType:=UnmanagedType.LPStr)> ByVal args As String()) As IntPtr
    End Function

    ''' <summary>
    ''' VLC�I�u�W�F�N�g���������B
    ''' </summary>
    ''' <param name="instanceHandle"></param>
    ''' <remarks></remarks>
    <DllImport("libvlc", CallingConvention:=CallingConvention.Cdecl)> _
    <System.Security.SuppressUnmanagedCodeSecurity()> _
    Public Shared Sub libvlc_release(ByVal instanceHandle As IntPtr)
    End Sub

    ''' <summary>
    ''' VLC�I�u�W�F�N�g�̃o�[�W�������擾����B
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DllImport("libvlc")> _
    <System.Security.SuppressUnmanagedCodeSecurity()> _
    Public Shared Function libvlc_get_version() As String
    End Function


    '#######################################################################################
    ''' <summary>
    ''' ���f�B�A�I�u�W�F�N�g���AURL���Q�Ƃ��Đ�������B
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
    ''' ���f�B�A�I�u�W�F�N�g���A���[�J���p�X���烁�f�B�A�t�@�C�����Q�Ƃ��Đ�������B
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
    ''' ���f�B�A�I�u�W�F�N�g�̏�Ԃ��擾����B
    ''' </summary>
    ''' <param name="mediaHandle"></param>
    ''' <remarks></remarks>
    <DllImport("libvlc", CallingConvention:=CallingConvention.Cdecl)> _
    <System.Security.SuppressUnmanagedCodeSecurity()> _
    Public Shared Function libvlc_media_get_state(ByVal mediaHandle As IntPtr) As Integer
    End Function

    ''' <summary>
    ''' ���^�f�[�^�ƃg���b�N����ǂݍ��ށB
    ''' </summary>
    ''' <param name="mediaHandle"></param>
    ''' <remarks></remarks>
    <DllImport("libvlc", CallingConvention:=CallingConvention.Cdecl)> _
    <System.Security.SuppressUnmanagedCodeSecurity()> _
    Public Shared Sub libvlc_media_parse(ByVal mediaHandle As IntPtr)
    End Sub

    ''' <summary>
    ''' �f�����[�V�������擾����B
    ''' </summary>
    ''' <param name="mediaHandle"></param>
    ''' <remarks></remarks>
    <DllImport("libvlc", CallingConvention:=CallingConvention.Cdecl)> _
    <System.Security.SuppressUnmanagedCodeSecurity()> _
    Public Shared Function libvlc_media_get_duration(ByVal mediaHandle As IntPtr) As Long
    End Function



    ''' <summary>
    ''' ���f�B�A�I�u�W�F�N�g���������B
    ''' </summary>
    ''' <param name="mediaHandle"></param>
    ''' <remarks></remarks>
    <DllImport("libvlc", CallingConvention:=CallingConvention.Cdecl)> _
    <System.Security.SuppressUnmanagedCodeSecurity()> _
    Public Shared Sub libvlc_media_release(ByVal mediaHandle As IntPtr)
    End Sub

    '#######################################################################################
    ''' <summary>
    ''' �v���C���[�I�u�W�F�N�g�𐶐�����B
    ''' </summary>
    ''' <param name="mediaHandle"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DllImport("libvlc", CallingConvention:=CallingConvention.Cdecl)> _
    <System.Security.SuppressUnmanagedCodeSecurity()> _
    Public Shared Function libvlc_media_player_new_from_media(ByVal mediaHandle As IntPtr) As IntPtr
    End Function

    ''' <summary>
    ''' �v���C���[�I�u�W�F�N�g���������B
    ''' </summary>
    ''' <param name="playerHandle"></param>
    ''' <remarks></remarks>
    <DllImport("libvlc", CallingConvention:=CallingConvention.Cdecl)> _
    <System.Security.SuppressUnmanagedCodeSecurity()> _
    Public Shared Sub libvlc_media_player_release(ByVal playerHandle As IntPtr)
    End Sub

    ''' <summary>
    ''' �v���C���[�̕`���I�u�W�F�N�g���Z�b�g����B
    ''' </summary>
    ''' <param name="playerHandle"></param>
    ''' <param name="viewControlHandle"></param>
    ''' <remarks></remarks>
    <DllImport("libvlc", CallingConvention:=CallingConvention.Cdecl)> _
    <System.Security.SuppressUnmanagedCodeSecurity()> _
    Public Shared Sub libvlc_media_player_set_hwnd(ByVal playerHandle As IntPtr, ByVal viewControlHandle As IntPtr)
    End Sub

    ''' <summary>
    ''' �v���C���[���烁�f�B�A�I�u�W�F�N�g���擾����B
    ''' </summary>
    ''' <param name="playerHandle"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DllImport("libvlc", CallingConvention:=CallingConvention.Cdecl)> _
    <System.Security.SuppressUnmanagedCodeSecurity()> _
    Public Shared Function libvlc_media_player_get_media(ByVal playerHandle As IntPtr) As IntPtr
    End Function

    ''' <summary>
    ''' �v���C���[�Ƀ��f�B�A���Z�b�g����B
    ''' </summary>
    ''' <param name="playerHandle"></param>
    ''' <param name="mediaHandle"></param>
    ''' <remarks></remarks>
    <DllImport("libvlc", CallingConvention:=CallingConvention.Cdecl)> _
    <System.Security.SuppressUnmanagedCodeSecurity()> _
    Public Shared Sub libvlc_media_player_set_media(ByVal playerHandle As IntPtr, ByVal mediaHandle As IntPtr)
    End Sub

    ''' <summary>
    ''' �Đ����\���ۂ����ׂ�B
    ''' </summary>
    ''' <param name="playerHandle"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DllImport("libvlc", CallingConvention:=CallingConvention.Cdecl)> _
    <System.Security.SuppressUnmanagedCodeSecurity()> _
    Public Shared Function libvlc_media_player_will_play(ByVal playerHandle As IntPtr) As Integer
    End Function

    ''' <summary>
    ''' �v���C���[���J�n����B
    ''' </summary>
    ''' <param name="playerHandle"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DllImport("libvlc", CallingConvention:=CallingConvention.Cdecl)> _
    <System.Security.SuppressUnmanagedCodeSecurity()> _
    Public Shared Function libvlc_media_player_play(ByVal playerHandle As IntPtr) As Integer
    End Function

    ''' <summary>
    ''' �v���C���[�𒆒f����B
    ''' </summary>
    ''' <param name="playerHandle"></param>
    ''' <remarks></remarks>
    <DllImport("libvlc", CallingConvention:=CallingConvention.Cdecl)> _
    <System.Security.SuppressUnmanagedCodeSecurity()> _
    Public Shared Sub libvlc_media_player_pause(ByVal playerHandle As IntPtr)
    End Sub

    ''' <summary>
    ''' �v���C���[���~����B
    ''' </summary>
    ''' <param name="playerHandle"></param>
    ''' <remarks></remarks>
    <DllImport("libvlc", CallingConvention:=CallingConvention.Cdecl)> _
    <System.Security.SuppressUnmanagedCodeSecurity()> _
    Public Shared Sub libvlc_media_player_stop(ByVal playerHandle As IntPtr)
    End Sub

    ''' <summary>
    ''' �v���C���[�̓����Ԃ��擾����B
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
    ''' �v���C���[���Đ������ۂ��B
    ''' </summary>
    ''' <param name="playerHandle"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' 1 = �Đ����A 0 = ���̑�
    ''' </remarks>
    <DllImport("libvlc", CallingConvention:=CallingConvention.Cdecl)> _
    <System.Security.SuppressUnmanagedCodeSecurity()> _
    Public Shared Function libvlc_media_player_is_playing(ByVal playerHandle As IntPtr) As Integer
    End Function

    ''' <summary>
    ''' �{�����[����ݒ�l���擾����B
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
    ''' �{�����[����ݒ肷��B
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
    ''' �~���[�g�ݒ�l���擾����B
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
    ''' �~���[�g�ݒ���s���B
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
    ''' �t���X�N���[���ݒ�l���擾����B
    ''' </summary>
    ''' <param name="playerHandle"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DllImport("libvlc", CallingConvention:=CallingConvention.Cdecl)> _
    <System.Security.SuppressUnmanagedCodeSecurity()> _
    Public Shared Function libvlc_get_fullscreen(ByVal playerHandle As IntPtr) As Integer
    End Function

    ''' <summary>
    ''' �t���X�N���[���\���ݒ���s���B
    ''' </summary>
    ''' <param name="playerHandle"></param>
    ''' <param name="fullscreenValue"></param>
    ''' <remarks></remarks>
    <DllImport("libvlc", CallingConvention:=CallingConvention.Cdecl)> _
    <System.Security.SuppressUnmanagedCodeSecurity()> _
    Public Shared Sub libvlc_set_fullscreen(ByVal playerHandle As IntPtr, ByVal fullscreenValue As Boolean)
    End Sub

    ''' <summary>
    ''' �t���X�N���[���\����؂�ς���B
    ''' </summary>
    ''' <param name="playerHandle"></param>
    ''' <remarks></remarks>
    <DllImport("libvlc", CallingConvention:=CallingConvention.Cdecl)> _
    <System.Security.SuppressUnmanagedCodeSecurity()> _
    Public Shared Sub libvlc_toggle_fullscreen(ByVal playerHandle As IntPtr)
    End Sub

    ''' <summary>
    ''' �V�[�N�\���ۂ����擾����B
    ''' </summary>
    ''' <param name="playerHandle"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DllImport("libvlc", CallingConvention:=CallingConvention.Cdecl)> _
    <System.Security.SuppressUnmanagedCodeSecurity()> _
    Public Shared Function libvlc_media_player_is_seekable(ByVal playerHandle As IntPtr) As Integer
    End Function

    ''' <summary>
    ''' �Đ��ʒu���擾����B
    ''' </summary>
    ''' <param name="playerHandle"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' �ʒu���p�[�Z���e�[�W(0.0 �` 1.0)�Ŏ����B
    ''' </remarks>
    <DllImport("libvlc", CallingConvention:=CallingConvention.Cdecl)> _
    <System.Security.SuppressUnmanagedCodeSecurity()> _
    Public Shared Function libvlc_media_player_get_position(ByVal playerHandle As IntPtr) As Single
    End Function

    ''' <summary>
    ''' �Đ��ʒu���Z�b�g����B
    ''' </summary>
    ''' <param name="playerHandle"></param>
    ''' <param name="position"></param>
    ''' <remarks>
    ''' �ʒu���p�[�Z���e�[�W(0.0 �` 1.0)�ŃZ�b�g����B
    ''' </remarks>
    <DllImport("libvlc", CallingConvention:=CallingConvention.Cdecl)> _
    <System.Security.SuppressUnmanagedCodeSecurity()> _
    Public Shared Sub libvlc_media_player_set_position(ByVal playerHandle As IntPtr, ByVal position As Single)
    End Sub

    ''' <summary>
    ''' �J�����g���f�B�A�̒������擾����B
    ''' </summary>
    ''' <param name="playerHandle"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DllImport("libvlc", CallingConvention:=CallingConvention.Cdecl)> _
    <System.Security.SuppressUnmanagedCodeSecurity()> _
    Public Shared Function libvlc_media_player_get_length(ByVal playerHandle As IntPtr) As Long
    End Function

    ''' <summary>
    ''' �^�C���X�^���v���擾����B
    ''' </summary>
    ''' <param name="playerHandle"></param>
    ''' <returns></returns>
    ''' <remarks>�P�ʁFMSec</remarks>
    <DllImport("libvlc", CallingConvention:=CallingConvention.Cdecl)> _
    <System.Security.SuppressUnmanagedCodeSecurity()> _
    Public Shared Function libvlc_media_player_get_time(ByVal playerHandle As IntPtr) As Long
    End Function

    ''' <summary>
    ''' �^�C���X�^���v���Z�b�g����B
    ''' </summary>
    ''' <param name="playerHandle"></param>
    ''' <param name="time"></param>
    ''' <remarks>�P�ʁFMSec</remarks>
    <DllImport("libvlc", CallingConvention:=CallingConvention.Cdecl)> _
    <System.Security.SuppressUnmanagedCodeSecurity()> _
    Public Shared Sub libvlc_media_player_set_time(ByVal playerHandle As IntPtr, ByVal time As Long)
    End Sub


    '#######################################################################################
    ''' <summary>
    ''' �X�g���[���T�[�o������������B
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
    ''' �T�[�o���J�n����B
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
    ''' �L�����^�������ݒ���s���B
    ''' </summary>
    ''' <param name="instanceHandle"></param>
    ''' <param name="name"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' enabled = ?���l��`��T���B
    ''' </remarks>
    <DllImport("libvlc", CallingConvention:=CallingConvention.Cdecl)> _
    <System.Security.SuppressUnmanagedCodeSecurity()> _
    Public Shared Function libvlc_vlm_set_enabled(ByVal instanceHandle As IntPtr, ByVal name As String, ByVal enabled As Integer) As Integer
    End Function
            
    ''' <summary>
    ''' ���f�B�A�̍Đ��ʒu��ݒ肷��B
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
    ''' ���f�B�A���JSON���擾����B
    ''' </summary>
    ''' <param name="instanceHandle"></param>
    ''' <param name="name"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' �������̓A�v���P�[�V������̔����ASCII�����񂵂��g�p���Ȃ��̂Ń}�[�V�������O�͕s�v�����A
    ''' �n���l�^�߂�l���ꂼ����J�X�^���}�[�V�������O����L�q�T���v���Ƃ��Ďc���Ă����B
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
    ''' �T�[�o���~����B
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
    ''' �T�[�o���\�[�X���������B
    ''' </summary>
    ''' <param name="instanceHandle"></param>
    ''' <remarks></remarks>
    <DllImport("libvlc", CallingConvention:=CallingConvention.Cdecl)> _
    <System.Security.SuppressUnmanagedCodeSecurity()> _
    Public Shared Sub libvlc_vlm_release(ByVal instanceHandle As IntPtr)
    End Sub


    '#######################################################################################
    ''' <summary>
    ''' �ێ����̃G���[���b�Z�[�W���擾����B
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DllImport("libvlc")> _
    <System.Security.SuppressUnmanagedCodeSecurity()> _
    Public Shared Function libvlc_errmsg() As IntPtr
    End Function

    ''' <summary>
    ''' �G���[�L���b�V�����폜����B
    ''' </summary>
    ''' <remarks></remarks>
    <DllImport("libvlc")> _
    <System.Security.SuppressUnmanagedCodeSecurity()> _
    Public Shared Sub libvlc_clearerr()
    End Sub

    '''' <summary>
    '''' ���f�B�A���\���̂��擾����B
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

    '������ێ��p�\���̒�`
    'Ver2.2�ł̒�`�Ɋ�Â����L�q�����AVer1.2�ł͓��삵�Ȃ������B
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
