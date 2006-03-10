# -*- makefile -*-
CSC=mcs

SOURCES=Janrain.OpenId/Association.cs \
	Janrain.OpenId/CryptUtil.cs \
	Janrain.OpenId/KVUtil.cs \
	Janrain.OpenId/Util.cs \
	Janrain.OpenId.Consumer/Consumer.cs \
	Janrain.OpenId.Consumer/Fetcher.cs \
	Janrain.OpenId.Consumer/FetchException.cs \
	Janrain.OpenId.Consumer/FetchResponse.cs \
	Janrain.OpenId.Consumer/SimpleFetcher.cs \
	Janrain.OpenId.Consumer/Parse.cs \
	Janrain.OpenId.Store/FileStore.cs \
	Janrain.OpenId.Store/IAssociationStore.cs \
	Janrain.OpenId.Store/SQLStore.cs

ASP_SOURCES=Janrain.OpenId.Consumer.Net/OpenIdModule.cs \
	Janrain.OpenId.Store.Net/MemoryStore.cs

TEST_SOURCES=Tests/Janrain.OpenId/Association.cs \
	Tests/Janrain.OpenId/MemoryStore.cs \
	Tests/Janrain.OpenId/DH.cs \
	Tests/Janrain.OpenId.Consumer/Fetchers.cs \
	Tests/Janrain.OpenId.Consumer/Parse.cs \
	Tests/Janrain.OpenId.Consumer/Consumer.cs \
	Tests/Janrain.OpenId.Store/Generic.cs

MYSQL_SOURCES=Janrain.OpenId.Store/MySqlStore.cs
MYSQL_TEST_SOURCES=Tests/Janrain.OpenId.Store/MySqlStore.cs

TEST_ASSEMBLY_SOURCES=TestHarness/TestAssembly.cs
TEST_HARNESS_SOURCES=TestHarness/Janrain.TestHarness/TestHarness.cs

ALL=TestAssembly.exe TestHarness.dll OpenIdTest.dll Janrain.OpenId.dll Janrain.OpenId.Consumer.Net.dll

all: $(ALL) mysql 

mysql: Janrain.OpenId.Store.MySql.dll OpenIdTest.Store.MySql.dll

doc: Janrain.OpenId.dll Janrain.OpenId.Consumer.Net.dll
	ndoc-console Janrain.OpenId.dll,Janrain.OpenId.xml Janrain.OpenId.Consumer.Net.dll,Janrain.OpenId.Consumer.Net.xml -documenter=MSDN || true
	mv doc/ndoc_msdn_temp/* doc/ && rmdir doc/ndoc_msdn_temp/

Janrain.OpenId.Store.MySql.dll: $(MYSQL_SOURCES) Janrain.OpenId.dll
	$(CSC) -debug+ -define:DEBUG -reference:System.Data,Janrain.OpenId,ByteFX.Data -target:library -out:$@ $(MYSQL_SOURCES)

OpenIdTest.Store.MySql.dll: $(MYSQL_TEST_SOURCES) Janrain.OpenId.Store.MySql.dll Janrain.OpenId.dll TestHarness.dll OpenIdTest.dll
	$(CSC) -debug+ -define:DEBUG -reference:TestHarness,Janrain.OpenId,OpenIdTest,Janrain.OpenId.Store.MySql,ByteFX.Data -target:library -out:$@ $(MYSQL_TEST_SOURCES)

Janrain.OpenId.Consumer.Net.dll: $(ASP_SOURCES)
	$(CSC) -debug+ -define:DEBUG -reference:System.Web,Janrain.OpenId -target:library -doc:Janrain.OpenId.Consumer.Net.xml -out:$@ $(ASP_SOURCES)

TestAssembly.exe: $(TEST_ASSEMBLY_SOURCES) TestHarness.dll
	$(CSC) -debug+ -define:DEBUG -reference:TestHarness -target:exe -out:$@ $(TEST_ASSEMBLY_SOURCES)

OpenIdTest.dll: $(TEST_SOURCES) TestHarness.dll Janrain.OpenId.dll
	$(CSC) -debug+ -define:DEBUG -reference:Mono.Security,System.Web,TestHarness,Janrain.OpenId -target:library -out:$@ $(TEST_SOURCES)

TestHarness.dll: $(TEST_HARNESS_SOURCES)
	$(CSC) -debug+ -define:DEBUG -target:library -out:$@ $(TEST_HARNESS_SOURCES)

Janrain.OpenId.dll: $(SOURCES)
	$(CSC) -debug+ -define:DEBUG -reference:Mono.Security,System.Web,System.Data -target:library -doc:Janrain.OpenId.xml -out:$@ $(SOURCES)

clean:
	find . -type f -name "*.mdb" | xargs rm -f
	find . -type f -name "*.dll" | xargs rm -f
	find . -type f -name "*.exe" | xargs rm -f
