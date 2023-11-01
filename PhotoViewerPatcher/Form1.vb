Imports System.IO

Public Class Form1

    Private Const PATTERN As String = "85 C0 75 ? ? 0D 03 15 86"

    Private Sub CreateBackup(ByVal path As String)
        File.Copy(path, $"{System.IO.Path.GetDirectoryName(path)}\ImagingEngine.dll.bak", True)
    End Sub

    Private Sub TakeOwnership(ByVal path As String)
        RunCommand("takeown.exe", $"/R /F ""{System.IO.Path.GetDirectoryName(path)}""")
        RunCommand("icacls.exe", $"""{path}"" /grant Administrators:F")
    End Sub

    Private Sub RunCommand(ByVal command As String, ByVal arguments As String)
        Dim p As Process = Process.Start(command, arguments)
        While True
            ' HACK: race condition fix.
            If p.HasExited Then
                Return
            End If
        End While
    End Sub

    Private Sub PatchFile(ByVal path As String, ByVal offset As Integer)

        TakeOwnership(path)
        Try
            CreateBackup(path)
            Using fs As New FileStream(path, FileMode.Open, FileAccess.ReadWrite)
                fs.Seek(offset, SeekOrigin.Begin)

                If fs.ReadByte() = &H75 Then
                    fs.Seek(offset, SeekOrigin.Begin)
                    fs.WriteByte(&HEB)
                Else
                    MsgBox("Invalid byte at offset, unsupported dll version?")
                    Environment.Exit(1)
                End If
            End Using
        Catch ex As System.UnauthorizedAccessException
            MsgBox("Access denied, run as administrator or manually take ownership of folder and files")
        End Try


    End Sub

    ' --- EVENTS ---

    Private Sub btnBrowse_Click(sender As Object, e As EventArgs) Handles btnBrowse.Click

        Dim a As New OpenFileDialog() With {.Filter = "DLL to patch|ImagingEngine.dll"}

        If a.ShowDialog() = DialogResult.OK Then
            txtPath.Text = a.FileName
        End If

    End Sub

    Private Sub btnPatch_Click(sender As Object, e As EventArgs) Handles btnPatch.Click

        Dim pathToFile As String = ""

        If txtPath.Text = "" Then
            MsgBox("Please insert path for ImagingEngine.dll or Browse. Usually under ""Program files\Windows PhotoViewer""")
            Environment.Exit(1)
        Else
            pathToFile = txtPath.Text
        End If

        Dim j As Integer, firstFound As Integer = 0
        Dim patternSplit As String() = PATTERN.Split(" ")

        Dim fileBytes As Byte() = File.ReadAllBytes(pathToFile)
        For i = 0 To fileBytes.Count - 1 Step 1

            If patternSplit(j) = "?" Then
                j += 1
                If firstFound = 0 Then
                    firstFound = i
                End If
                Continue For
            End If

            If fileBytes(i) = CByte($"&H{patternSplit(j)}") Then
                j += 1
                If firstFound = 0 Then
                    firstFound = i
                End If
            Else
                firstFound = 0
                j = 0
            End If

            If j >= patternSplit.Count Then
                Exit For
            End If
        Next

        If firstFound > 0 Then
            PatchFile(pathToFile, (firstFound + &H2))
            ' HACK: We lose focus after running commands with Process.Start()
            Me.Focus()
            MsgBox("Done")
        Else
            MsgBox("Could not find byte to patch. Already patched?")
            Return
        End If

    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If Not My.User.IsInRole(ApplicationServices.BuiltInRole.Administrator) Then
            MsgBox("You are not running as Administrator, expect some errors")
        End If
    End Sub
End Class


