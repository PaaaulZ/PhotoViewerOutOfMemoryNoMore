<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form1
    Inherits System.Windows.Forms.Form

    'Form esegue l'override del metodo Dispose per pulire l'elenco dei componenti.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Richiesto da Progettazione Windows Form
    Private components As System.ComponentModel.IContainer

    'NOTA: la procedura che segue è richiesta da Progettazione Windows Form
    'Può essere modificata in Progettazione Windows Form.  
    'Non modificarla mediante l'editor del codice.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.btnBrowse = New System.Windows.Forms.Button()
        Me.btnPatch = New System.Windows.Forms.Button()
        Me.txtPath = New System.Windows.Forms.TextBox()
        Me.lblPath = New System.Windows.Forms.Label()
        Me.btnPatchSigScan = New System.Windows.Forms.Button()
        Me.txtPathSigScanner = New System.Windows.Forms.TextBox()
        Me.lblPathSig = New System.Windows.Forms.Label()
        Me.btnBrowseSig = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'btnBrowse
        '
        Me.btnBrowse.Location = New System.Drawing.Point(531, 69)
        Me.btnBrowse.Name = "btnBrowse"
        Me.btnBrowse.Size = New System.Drawing.Size(55, 23)
        Me.btnBrowse.TabIndex = 0
        Me.btnBrowse.Text = "Browse"
        Me.btnBrowse.UseVisualStyleBackColor = True
        '
        'btnPatch
        '
        Me.btnPatch.Location = New System.Drawing.Point(495, 200)
        Me.btnPatch.Name = "btnPatch"
        Me.btnPatch.Size = New System.Drawing.Size(114, 69)
        Me.btnPatch.TabIndex = 0
        Me.btnPatch.Text = "Patch"
        Me.btnPatch.UseVisualStyleBackColor = True
        '
        'txtPath
        '
        Me.txtPath.Location = New System.Drawing.Point(149, 71)
        Me.txtPath.Name = "txtPath"
        Me.txtPath.Size = New System.Drawing.Size(376, 20)
        Me.txtPath.TabIndex = 1
        '
        'lblPath
        '
        Me.lblPath.AutoSize = True
        Me.lblPath.Location = New System.Drawing.Point(13, 74)
        Me.lblPath.Name = "lblPath"
        Me.lblPath.Size = New System.Drawing.Size(130, 13)
        Me.lblPath.TabIndex = 2
        Me.lblPath.Text = "Path to ImagingEngine.dll:"
        '
        'btnPatchSigScan
        '
        Me.btnPatchSigScan.Location = New System.Drawing.Point(495, 275)
        Me.btnPatchSigScan.Name = "btnPatchSigScan"
        Me.btnPatchSigScan.Size = New System.Drawing.Size(114, 69)
        Me.btnPatchSigScan.TabIndex = 3
        Me.btnPatchSigScan.Text = "Patch (sigscan)"
        Me.btnPatchSigScan.UseVisualStyleBackColor = True
        '
        'txtPathSigScanner
        '
        Me.txtPathSigScanner.Location = New System.Drawing.Point(149, 94)
        Me.txtPathSigScanner.Name = "txtPathSigScanner"
        Me.txtPathSigScanner.Size = New System.Drawing.Size(376, 20)
        Me.txtPathSigScanner.TabIndex = 1
        '
        'lblPathSig
        '
        Me.lblPathSig.AutoSize = True
        Me.lblPathSig.Location = New System.Drawing.Point(21, 97)
        Me.lblPathSig.Name = "lblPathSig"
        Me.lblPathSig.Size = New System.Drawing.Size(122, 13)
        Me.lblPathSig.TabIndex = 2
        Me.lblPathSig.Text = "Path to SigScanner.exe:"
        '
        'btnBrowseSig
        '
        Me.btnBrowseSig.Location = New System.Drawing.Point(531, 94)
        Me.btnBrowseSig.Name = "btnBrowseSig"
        Me.btnBrowseSig.Size = New System.Drawing.Size(55, 23)
        Me.btnBrowseSig.TabIndex = 0
        Me.btnBrowseSig.Text = "Browse"
        Me.btnBrowseSig.UseVisualStyleBackColor = True
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(621, 473)
        Me.Controls.Add(Me.btnPatchSigScan)
        Me.Controls.Add(Me.lblPathSig)
        Me.Controls.Add(Me.lblPath)
        Me.Controls.Add(Me.txtPathSigScanner)
        Me.Controls.Add(Me.txtPath)
        Me.Controls.Add(Me.btnPatch)
        Me.Controls.Add(Me.btnBrowseSig)
        Me.Controls.Add(Me.btnBrowse)
        Me.Name = "Form1"
        Me.Text = "Windows Photo Viewer | Patcher"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents btnBrowse As Button
    Friend WithEvents btnPatch As Button
    Friend WithEvents txtPath As TextBox
    Friend WithEvents lblPath As Label
    Friend WithEvents btnPatchSigScan As Button
    Friend WithEvents txtPathSigScanner As TextBox
    Friend WithEvents lblPathSig As Label
    Friend WithEvents btnBrowseSig As Button
End Class
