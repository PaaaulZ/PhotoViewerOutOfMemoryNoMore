Imports System.Security.Cryptography
Imports System.Text
Imports System.IO

Public Class Form1

    Private VALID_CRC As New Dictionary(Of String, Integer)

    Private Sub CreateBackup(ByVal path As String)

        Dim pathOnly As String = System.IO.Path.GetDirectoryName(path)

        File.Delete(pathOnly + "\ImagingEngine.dll.bak")
        File.Copy(path, pathOnly + "\ImagingEngine.dll.bak")

    End Sub

    Private Sub PatchFile(ByVal path As String, ByVal offset As Integer)

        CreateBackup(path)

        Dim sr As FileStream = New FileStream(path, FileMode.Open, FileAccess.ReadWrite)
        sr.Seek(offset, SeekOrigin.Begin)

        Dim currentByte As Byte = sr.ReadByte()
        If currentByte = &H75 Then
            sr.Seek(offset, SeekOrigin.Begin)
            sr.WriteByte(&HEB)
        Else
            MsgBox("Invalid byte at offset, unsupported dll version?")
            Environment.Exit(1)
        End If
        sr.Close()

    End Sub

    ' --- INIT ---

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Init dictionaries

        ' --- Windows 10 Pro Version 1903 Build 18362.356 ---

        ' x86 
        VALID_CRC.Add("3F43D403517A4889856CFFAC7AB579545E9FFE0F022F3BF0262890EC4EC269EF", &HC93DE)
        ' x64
        VALID_CRC.Add("A532AE68CCC53DFDC190BB447AE9175A86C072DD99A374151F1D23EDABFCC1A2", &H9E56D)

        ' --- Windows 7 Enterprise Version 6.1.7601 Service Pack 1 Build 7601 ---

        ' x86
        VALID_CRC.Add("F732F0530A7FFD6C67FF774760174B8E01377DC1085942215870C85235F11C61", &HCAA9E)



    End Sub

    ' --- UTILS ---
    Private Function BytesToHexString(ByVal bytes_Input As Byte()) As String

        ' https://social.msdn.microsoft.com/Forums/vstudio/en-US/fa53ce74-fd53-4d2a-bc05-619Fb9d32481/convert-Byte-array-To-hex-String?forum=vbgeneral

        Dim strTemp As New StringBuilder(bytes_Input.Length * 2)

        For Each b As Byte In bytes_Input

            ' HACK: My calculated hash has zeros in front of single digits
            Dim hexTMP As String = Conversion.Hex(b)
            If hexTMP.Length = 1 Then
                hexTMP = "0" + hexTMP
            End If

            strTemp.Append(hexTMP)
        Next
        Return strTemp.ToString()

    End Function

    Private Function GetOffset(ByVal path As String)

        Dim sha256 As SHA256 = SHA256Managed.Create()
        Dim fileStream As Stream = New StreamReader(path).BaseStream
        Dim hash As Byte() = sha256.ComputeHash(fileStream)
        Dim hexString As String = BytesToHexString(hash)

        fileStream.Close()

        If VALID_CRC.Keys.Contains(hexString) Then
            Return VALID_CRC(hexString)
        Else
            Return Nothing
        End If

    End Function

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

        Dim offset As Integer = GetOffset(pathToFile)
        If IsNothing(offset) Then
            If MessageBox.Show("Unknown ImagingEngine.dll version, do you want to try anyway?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.No Then
                Environment.Exit(1)
            Else
                PatchFile(pathToFile, offset)
            End If
        Else
            PatchFile(pathToFile, offset)
        End If

        MsgBox("Done.")

    End Sub
End Class


