Option Strict Off
Option Explicit Off
Imports System
Imports EnvDTE
Imports EnvDTE80
Imports System.Diagnostics

Public Module RecordingModule
    Sub ReplaceOMIds()
        DTE.ExecuteCommand("Edit.FindinFiles")
        DTE.Windows.Item("{CF2DDC32-8CAD-11D2-9302-005345000000}").Activate() 'Find and Replace
        DTE.ExecuteCommand("Edit.SwitchtoReplaceInFiles")
        DTE.Find.FilesOfType = "*.xml"
        DTE.Find.Action = vsFindAction.vsFindActionReplaceAll
        DTE.Windows.Item(Constants.vsWindowKindSolutionExplorer).Activate()
        DTE.Find.FindWhat = "<id "
        DTE.Find.ReplaceWith = "<id access=""nosetter.pascalcase-m-underscore""  "
        DTE.Find.Target = vsFindTarget.vsFindTargetFiles
        DTE.Find.MatchCase = False
        DTE.Find.MatchWholeWord = False
        DTE.Find.MatchInHiddenText = True
        DTE.Find.PatternSyntax = vsFindPatternSyntax.vsFindPatternSyntaxLiteral
        DTE.Find.SearchPath = "Entire Solution"
        DTE.Find.SearchSubfolders = True
        DTE.Find.KeepModifiedDocumentsOpen = False
        DTE.Find.ResultsLocation = vsFindResultsLocation.vsFindResults1
        DTE.Find.Action = vsFindAction.vsFindActionReplaceAll
        If (DTE.Find.Execute() = vsFindResult.vsFindResultNotFound) Then
            Throw New System.Exception("vsFindResultNotFound")
        End If
        DTE.Windows.Item("{CF2DDC32-8CAD-11D2-9302-005345000000}").Close()
    End Sub
    Sub ReplaceOMByte()
        DTE.ExecuteCommand("Edit.FindinFiles")
        DTE.Windows.Item("{CF2DDC32-8CAD-11D2-9302-005345000000}").Activate() 'Find and Replace
        DTE.ExecuteCommand("Edit.SwitchtoReplaceInFiles")
        DTE.Find.FilesOfType = "*.hbm.xml"
        DTE.Find.Action = vsFindAction.vsFindActionReplaceAll
        DTE.Windows.Item(Constants.vsWindowKindSolutionExplorer).Activate()
        DTE.Find.FindWhat = "Byte()"
        DTE.Find.ReplaceWith = "BinaryBlob"
        DTE.Find.Target = vsFindTarget.vsFindTargetFiles
        DTE.Find.MatchCase = False
        DTE.Find.MatchWholeWord = False
        DTE.Find.MatchInHiddenText = True
        DTE.Find.PatternSyntax = vsFindPatternSyntax.vsFindPatternSyntaxLiteral
        DTE.Find.SearchPath = "Entire Solution"
        DTE.Find.SearchSubfolders = True
        DTE.Find.KeepModifiedDocumentsOpen = False
        DTE.Find.ResultsLocation = vsFindResultsLocation.vsFindResults1
        DTE.Find.Action = vsFindAction.vsFindActionReplaceAll
        If (DTE.Find.Execute() = vsFindResult.vsFindResultNotFound) Then
            Throw New System.Exception("vsFindResultNotFound")
        End If
        DTE.Windows.Item("{CF2DDC32-8CAD-11D2-9302-005345000000}").Close()
    End Sub
    Sub ReplaceOMLazy()
        DTE.ExecuteCommand("Edit.FindinFiles")
        DTE.Windows.Item("{CF2DDC32-8CAD-11D2-9302-005345000000}").Activate() 'Find and Replace
        DTE.ExecuteCommand("Edit.SwitchtoReplaceInFiles")
        DTE.Find.FilesOfType = "*.hbm.xml"
        DTE.Find.FindWhat = "<bag "
        DTE.Find.ReplaceWith = "<bag lazy=""true"" "
        DTE.Find.Action = vsFindAction.vsFindActionReplaceAll
        DTE.Windows.Item(Constants.vsWindowKindSolutionExplorer).Activate()
        DTE.Find.Target = vsFindTarget.vsFindTargetFiles
        DTE.Find.MatchCase = False
        DTE.Find.MatchWholeWord = False
        DTE.Find.MatchInHiddenText = True
        DTE.Find.PatternSyntax = vsFindPatternSyntax.vsFindPatternSyntaxLiteral
        DTE.Find.SearchPath = "Entire Solution"
        DTE.Find.SearchSubfolders = True
        DTE.Find.KeepModifiedDocumentsOpen = False
        DTE.Find.ResultsLocation = vsFindResultsLocation.vsFindResults1
        DTE.Find.Action = vsFindAction.vsFindActionReplaceAll
        If (DTE.Find.Execute() = vsFindResult.vsFindResultNotFound) Then
            Throw New System.Exception("vsFindResultNotFound")
        End If
        DTE.Windows.Item("{CF2DDC32-8CAD-11D2-9302-005345000000}").Close()
    End Sub
    Sub ReplaceOM()
        ReplaceOMLazy()
        ReplaceOMIds()
        ReplaceOMByte()
    End Sub
End Module



