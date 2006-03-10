# -*- makefile -*-
CSC=gmcs

SOURCES=Mono.Math/BigInteger.cs \
	Mono.Math.Prime/ConfidenceFactor.cs \
	Mono.Math.Prime/PrimalityTests.cs \
	Mono.Math.Prime.Generator/NextPrimeFinder.cs \
	Mono.Math.Prime.Generator/PrimeGeneratorBase.cs \
	Mono.Math.Prime.Generator/SequentialSearchPrimeGeneratorBase.cs \
	Mono.Security.Cryptography/DHKeyGeneration.cs \
	Mono.Security.Cryptography/DHParameters.cs \
	Mono.Security.Cryptography/DiffieHellman.cs \
	Mono.Security.Cryptography/DiffieHellmanManaged.cs \
	Mono.Xml/MiniParser.cs \
	Mono.Xml/SecurityParser.cs


DiffieHellman.dll: $(SOURCES)
	$(CSC) -target:library -clscheck- -out:$@ -unsafe+ $(SOURCES)
