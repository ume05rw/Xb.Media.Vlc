Option Strict On

''' <summary>
''' �C���X�^���X�Ǘ��N���X
''' </summary>
''' <remarks></remarks>
Public Class Core
    Implements IDisposable


    '�n���h��
    Friend Handle As IntPtr


    ''' <summary>
    ''' �R���X�g���N�^
    ''' </summary>
    ''' <param name="args"></param>
    ''' <remarks></remarks>
    Public Sub New(ByVal args As String())

        'LibVLC 2.x�n�̎��s�O�`�F�b�N
        '2.x�n�ł́A�ȉ��̏������K�v�B
        '  dll�Ɠ����f�B���N�g���Ƀv���O�C���t�H���_��������΂Ȃ�Ȃ��B
        '  �v���O�C���t�H���_���A���ϐ�"VLC_PLUGIN_PATH"�ɃZ�b�g���Ă����˂΂Ȃ�Ȃ��B

        'libvlc.dll�̑��݃`�F�b�N
        Dim appPath As String = Xb.Base.AppPath
        If (Not IO.File.Exists(Xb.Base.AppPath & "\libvlc.dll")) Then
            Xb.Util.Out("LibVLC���C�u���������o�o���܂���B")
            Throw New ApplicationException("LibVLC���C�u���������o�o���܂���B")
        End If

        '�v���O�C���t�H���_�̑��݃`�F�b�N
        If (Not IO.Directory.Exists(Xb.Base.AppPath & "\plugins")) Then
            Xb.Util.Out("LibVLC�p�v���O�C�������o�o���܂���B")
            Throw New ApplicationException("LibVLC�p�v���O�C�������o�o���܂���B")
        End If

        Environment.SetEnvironmentVariable("VLC_PLUGIN_PATH", Xb.Base.AppPath & "\plugins")

        'VLC�C���X�^���X���擾����B
        If (args.Length > 0) Then
            Me.Handle = Xb.Media.Vlc.Lib.libvlc_new(args.Length, args)
        Else
            Me.Handle = Xb.Media.Vlc.Lib.libvlc_new(0, Nothing)
        End If


        If (Me.Handle = IntPtr.Zero) Then
            Xb.Media.Vlc.Exception.ThrowIfExist("Xb.Media.Vlc.Core.New")
        End If

        'LibVLC ver 1.x�n�ł̃C���X�^���X�擾
        'Dim args() As String = New String() { _
        '    "-I", _
        '    "dummy", _
        '    "--ignore-config", _
        '    "--plugin-path=C:\Users\ikaruga\Documents\Visual Studio 2005\Projects\MoviePlayer\Xb\Lib\Vlc\plugins" _
        '}
        'Me.Handle = Xb.Media.Vlc.Lib.libvlc_new(args.Length, args)

        'LibVLC ver 0.98�ł̃C���X�^���X�擾
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
    ''' VLC�I�u�W�F�N�g�̃o�[�W�������擾����B
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetVersion() As String

        Dim result As String = Xb.Media.Vlc.Lib.libvlc_get_version()
        Xb.Media.Vlc.Exception.ThrowIfExist("Xb.Media.Vlc.Core.GetVersion")
        Return result

    End Function


    Private _disposedValue As Boolean = False        ' �d������Ăяo�������o����ɂ�

    ' IDisposable
    Protected Overridable Sub Dispose(ByVal disposing As Boolean)
        If Not Me._disposedValue Then
            If disposing Then
            End If

            'VLC�C���X�^���X��j������B
            Xb.Media.Vlc.Lib.libvlc_release(Me.Handle)
            Xb.Media.Vlc.Exception.ThrowIfExist("Xb.Media.Vlc.Core.Dispose")

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

