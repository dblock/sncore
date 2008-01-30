move Janrain.OpenId.dll Janrain.OpenId.weak.dll
"C:\Program Files\Microsoft\ILMerge\ILMerge.exe" Janrain.OpenId.weak.dll /keyfile:..\..\key.snk /out:Janrain.OpenId.dll
del Janrain.OpenId.weak.dll

