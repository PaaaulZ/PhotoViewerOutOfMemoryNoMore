﻿Imports System.Security.Cryptography
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

        ' --- Windows 10 Version 22H2 Build 19045.2486 ---

        ' x86 
        VALID_CRC.Add("1160360F25D5D1E263C7EADF03240B251B1DCB4263CB2AFCF431619A08346D0E", &HC88AD)
        ' x64
        VALID_CRC.Add("994476041A9AF3E546706B016A8890B7830FD94FF9C766DD18FF36530E456987", &HA07DC)

        ' --- Windows 7 Enterprise Version 6.1.7601 Service Pack 1 Build 7601 ---

        ' x86
        VALID_CRC.Add("F732F0530A7FFD6C67FF774760174B8E01377DC1085942215870C85235F11C61", &HCAA9E)

        ' --- Windows 11 Pro 10.0.22000 build 22000 ---

        ' x64
        VALID_CRC.Add("5176E7C7AD8C8C62D4B78C3300067758DA380EF85CED7DF150B50BB4DD981A84", &HC4D17)
        ' x86
        VALID_CRC.Add("806BD2FF9DDBE08E1E230FE4CA048312CEB88ACF0EEEDF3597938014FA496D48", &HCCAA0)



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

    Private Sub btnPatchSigScan_Click(sender As Object, e As EventArgs) Handles btnPatchSigScan.Click

        Dim pathToFile As String = ""
        Dim pathToFileSig As String = ""

        If txtPathSigScanner.Text = "" Then
            MsgBox("Please insert path for SigScanner.exe or Browse. Usually under ""Program files\Windows PhotoViewer""")
            Environment.Exit(1)
        Else
            pathToFileSig = txtPathSigScanner.Text
        End If

        If txtPath.Text = "" Then
            MsgBox("Please insert path for ImagingEngine.dll or Browse. Usually under ""Program files\Windows PhotoViewer""")
            Environment.Exit(1)
        Else
            pathToFile = txtPath.Text
        End If

        Dim p As New Process()
        p.StartInfo.FileName = pathToFileSig
        p.StartInfo.Arguments = pathToFile + " " + pathToFile.Replace("ImagingEngine.dll", "ImagingEngine.dll.bak")
        p.Start()

    End Sub

    Private Sub btnBrowseSig_Click(sender As Object, e As EventArgs) Handles btnBrowseSig.Click

        Dim a As New OpenFileDialog() With {.Filter = "SigScanner executable|SigScanner.exe"}

        If a.ShowDialog() = DialogResult.OK Then
            txtPathSigScanner.Text = a.FileName
        End If

    End Sub
End Class


