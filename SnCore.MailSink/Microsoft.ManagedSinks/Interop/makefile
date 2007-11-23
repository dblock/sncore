currentdir = .
EXOUTDIR = $(currentdir)\obj

goal bldbin: $(EXOUTDIR)\Microsoft.Exchange.Transport.EventInterop.dll

$(EXOUTDIR)\Microsoft.Exchange.Transport.EventInterop.dll: Microsoft.Exchange.Transport.EventInterop.idl Microsoft.Exchange.Transport.EventInterop.diff Microsoft.Exchange.Transport.EventInterop.snk
	-md $(EXOUTDIR)
	midl Microsoft.Exchange.Transport.EventInterop.idl /robust /out $(EXOUTDIR)
	tlbimp $(EXOUTDIR)\Microsoft.Exchange.Transport.EventInterop.tlb /primary /out:$(EXOUTDIR)\Microsoft.Exchange.Transport.EventInterop.dll /namespace:Microsoft.Exchange.Transport.EventInterop /keyfile:Microsoft.Exchange.Transport.EventInterop.snk
	ildasm $(EXOUTDIR)\Microsoft.Exchange.Transport.EventInterop.dll /out=$(EXOUTDIR)\Microsoft.Exchange.Transport.EventInterop.il.orig
	patch $(EXOUTDIR)\Microsoft.Exchange.Transport.EventInterop.il.orig Microsoft.Exchange.Transport.EventInterop.diff -o $(EXOUTDIR)\Microsoft.Exchange.Transport.EventInterop.il
	ilasm $(EXOUTDIR)\Microsoft.Exchange.Transport.EventInterop.il /dll /output=$(EXOUTDIR)\Microsoft.Exchange.Transport.EventInterop.dll /key=Microsoft.Exchange.Transport.EventInterop.snk
