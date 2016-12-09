Option Strict On

''' <summary>
''' ���f�B�A�Ǘ��N���X
''' </summary>
''' <remarks></remarks>
Public Class Media
    Implements IDisposable

    '�n���h��
    Friend Handle As IntPtr

    ''' <summary>
    ''' �R���X�g���N�^
    ''' </summary>
    ''' <param name="instance"></param>
    ''' <param name="fileName"></param>
    ''' <remarks></remarks>
    Public Sub New(ByVal instance As Core, ByVal fileName As String)

        Dim state As Integer

        If (Not IO.File.Exists(fileName)) Then
            Xb.Util.Out("�n���l�t�@�C���̑��݂��m�F�o���܂���F" & fileName)
            Throw New ArgumentException("�n���l�t�@�C���̑��݂��m�F�o���܂���F" & fileName)
        End If

        '�t�@�C�����烁�f�B�A�𐶐�����B
        Me.Handle = [Lib].libvlc_media_new_path(instance.Handle, fileName)


        '���f�B�A�n���h���擾�`�F�b�N
        If (Me.Handle = IntPtr.Zero) Then
            Xb.Media.Vlc.Exception.ThrowIfExist("Xb.Media.Vlc.Media.New_1")
            Throw New ArgumentException("���f�B�A�擾�Ɏ��s���܂����B")
        End If

        '���f�B�A�X�e�[�g�̌��� - ��������OK�ɂȂ�B
        'IDLE/CLOSE=0, OPENING=1, BUFFERING=2, PLAYING=3, PAUSED=4, 
        'STOPPING=5, ENDED=6, ERROR=7 
        state = [Lib].libvlc_media_get_state(Me.Handle)
        Xb.Media.Vlc.Exception.ThrowIfExist("Xb.Media.Vlc.Media.New_2")
        If (state = 7) Then
            Xb.Util.Out("Xb.Media.Vlc.Media.New: ���f�B�A���G���[��Ԃł��B")
            Throw New ArgumentException("���f�B�A���G���[��Ԃł��B")
        End If

    End Sub


    ''' <summary>
    ''' �R���X�g���N�^ - �����ς݃��f�B�A�̃n���h����ێ�����R���X�g���N�^
    ''' </summary>
    ''' <param name="handle"></param>
    ''' <remarks></remarks>
    Public Sub New(ByVal handle As IntPtr)
        Me.Handle = handle
    End Sub


    ' ''' <summary>
    ' ''' Track type.
    ' ''' </summary>
    'Public Enum libvlc_track_t As Integer
    '    ''' <summary>
    '    ''' Unknown track type.
    '    ''' </summary>
    '    libvlc_track_unknown = -1
    '    ''' <summary>
    '    ''' Track is audio.
    '    ''' </summary>
    '    libvlc_track_audio = 0
    '    ''' <summary>
    '    ''' Track is video.
    '    ''' </summary>
    '    libvlc_track_video = 1
    '    ''' <summary>
    '    ''' Track is text.
    '    ''' </summary>
    '    libvlc_track_text = 2
    'End Enum

    ' ''' <summary>
    ' ''' Track information.
    ' ''' </summary>
    '<Runtime.InteropServices.StructLayout(Runtime.InteropServices.LayoutKind.Explicit)> _
    'Public Class libvlc_media_track_info_t
    '    ' fourcc
    '    <Runtime.InteropServices.FieldOffset(0)> _
    '    Public i_codec As UInteger
    '    <Runtime.InteropServices.FieldOffset(4)> _
    '    Public i_id As Integer

    '    <Runtime.InteropServices.FieldOffset(8)> _
    '    Public i_type As libvlc_track_t

    '    ' video codec info
    '    <Runtime.InteropServices.FieldOffset(12)> _
    '    Public i_profile As Integer
    '    <Runtime.InteropServices.FieldOffset(16)> _
    '    Public i_level As Integer

    '    <Runtime.InteropServices.FieldOffset(20)> _
    '    Public i_channels As UInteger
    '    <Runtime.InteropServices.FieldOffset(24)> _
    '    Public i_rate As UInteger

    '    <Runtime.InteropServices.FieldOffset(20)> _
    '    Public i_height As UInteger
    '    <Runtime.InteropServices.FieldOffset(24)> _
    '    Public i_width As UInteger
    'End Class

    'Public Shared Function libvlc_media_get_tracks_info(media As IntPtr) As libvlc_media_track_info_t()

    '    Dim size As Integer = Runtime.InteropServices.Marshal.SizeOf(GetType(libvlc_media_track_info_t))
    '    Dim pointerSize As Integer = IntPtr.Size
    '    Dim pointer As IntPtr = Runtime.InteropServices.Marshal.AllocHGlobal(pointerSize)

    '    For i As Integer = 0 To pointerSize - 1
    '        Runtime.InteropServices.Marshal.WriteByte(New IntPtr(pointer.ToInt64() + i), &H0)
    '    Next

    '    Try
    '        Dim count As Integer = libvlc_media_get_tracks_info(media, pointer)

    '        If count > 0 Then
    '            Dim list As New List(Of libvlc_media_track_info_t)(count)
    '            Dim arrPtr As IntPtr = Runtime.InteropServices.Marshal.ReadIntPtr(pointer)

    '            If arrPtr <> IntPtr.Zero Then
    '                Try
    '                    For i As Integer = 0 To count - 1
    '                        Dim [structure] As New libvlc_media_track_info_t()
    '                        Runtime.InteropServices.Marshal.PtrToStructure(arrPtr, [structure])
    '                        list.Add([structure])

    '                        arrPtr = New IntPtr(arrPtr.ToInt64() + size)
    '                    Next
    '                    Return list.ToArray()
    '                Finally
    '                    ' TODO: libvlc tells to free memory. but how?
    '                    'LibVlcInterop.libvlc_free(arrPtr);
    '                    Try
    '                    Catch exc As Exception
    '                        Throw New ApplicationException("Can't free memory allocated inside libvlc.")
    '                    End Try
    '                End Try
    '            End If
    '            '
    '        End If
    '    Catch exc As Exception
    '        Throw New ApplicationException("Can't free memory allocated inside libvlc.")
    '    Finally
    '        Runtime.InteropServices.Marshal.FreeHGlobal(pointer)
    '    End Try
    '    Return New libvlc_media_track_info_t() {}
    'End Function


    Private _disposedValue As Boolean = False        ' �d������Ăяo�������o����ɂ�

    ' IDisposable
    Protected Overridable Sub Dispose(ByVal disposing As Boolean)
        If Not Me._disposedValue Then
            If disposing Then
            End If

            'VLC���f�B�A�C���X�^���X��j������B
            [Lib].libvlc_media_release(Me.Handle)
        End If
        Me._disposedValue = True
    End Sub

#Region " IDisposable Support "
    ' ���̃R�[�h�́A�j���\�ȃp�^�[���𐳂��������ł���悤�� Visual Basic �ɂ���Ēǉ�����܂����B
    Public Sub Dispose() Implements IDisposable.Dispose
        ' ���̃R�[�h��ύX���Ȃ��ł��������B�N���[���A�b�v �R�[�h����� Dispose(ByVal disposing As Boolean) �ɋL�q���܂��B
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub
#End Region

End Class

'Public Function GetInfo() As libvlc_media_track_t
'    Dim trackInfo As libvlc_media_track_t = New libvlc_media_track_t(), _
'        result As Integer
'    result = Xb.Media.Vlc.Lib.libvlc_media_tracks_get(Me.Handle, trackInfo)
'    If (result = 0) Then
'        '���s�B
'        Xb.App.Out("FAIL to Get TrackInfo.")
'    End If
'    Return trackInfo
'End Function

