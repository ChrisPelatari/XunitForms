solution_file = "source/XunitForms.sln"
configuration = "release"
fx_path = env("windir") + "/Microsoft.NET/Framework/v4.0.30319/msbuild.exe"
test_assembly = "source/XunitForms.Test/bin/${configuration}/XunitForms.Test.dll"

target default, (init, compile, test, deploy, package):
	pass
  
target init:
	rmdir("build/${configuration}")
  
desc "Compiles the solution"
target compile: 
	msbuild(file: solution_file, configuration: configuration, toolPath: fx_path)
  
desc "Executes tests"
target test: 
	xunit(assembly: test_assembly)
  
desc "Copies binaries and content to the 'build' directory"
target deploy:
	print "Copying to build dir"
  
	with FileList("source/XunitForms/bin/${configuration}"):
		.Include("XunitForms.dll")
		.ForEach def(file):
			file.CopyToDirectory("build/${configuration}")
	  
desc "Creates zip package"
target package:
	zip("build/${configuration}", "build/XunitForms.zip")
