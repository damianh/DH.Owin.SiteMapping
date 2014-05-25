properties { 
	$projectName = "Owin.SiteMapping"
	$buildNumber = 0
	$rootDir  = Resolve-Path .\
	$buildOutputDir = "$rootDir\build"
	$reportsDir = "$buildOutputDir\reports"
	$srcDir = "$rootDir\src"
	$solutionFilePath = "$srcDir\$projectName.sln"
	$assemblyInfoFilePath = "$srcDir\SharedAssemblyInfo.cs"
}

task default -depends Clean, UpdateVersion, RunTests, ILRepack, CopyBuildOutput, CreateNuGetPackages

task Clean {
	Remove-Item $buildOutputDir -Force -Recurse -ErrorAction SilentlyContinue
	exec { msbuild /nologo /verbosity:quiet $solutionFilePath /t:Clean }
}

task UpdateVersion {
	$version = Get-Version $assemblyInfoFilePath
	$oldVersion = New-Object Version $version
	$newVersion = New-Object Version ($oldVersion.Major, $oldVersion.Minor, $oldVersion.Build, $buildNumber)
	Update-Version $newVersion $assemblyInfoFilePath
}

task Compile { 
	exec { msbuild /nologo /verbosity:quiet $solutionFilePath /p:Configuration=Release }
}

task RunTests -depends Compile {
	$xunitRunner = "$srcDir\packages\xunit.runners.1.9.2\tools\xunit.console.clr4.exe"
	Get-ChildItem . -Recurse -Include *Tests.csproj, Tests.*.csproj | % {
		$project = $_.BaseName
		if(!(Test-Path $reportsDir\xUnit\$project)){
			New-Item $reportsDir\xUnit\$project -Type Directory
		}
        .$xunitRunner "$srcDir\$project\bin\Release\$project.dll" /html "$reportsDir\xUnit\$project\index.html"
    }
}

task ILRepack -depends Compile {
	$ilrepack = "$srcDir\packages\ILRepack.1.25.0\tools\ILRepack.exe"
	$workingDir = "$srcDir\Owin.SiteMapping\bin\Release"
	.$ilrepack /targetplatform:v4 /internalize /target:library /out:$workingDir\$projectName.dll $workingDir\$projectName.dll $workingDir\Microsoft.Owin.dll $workingDir\Owin.dll
}

task CopyBuildOutput -depends Compile {
	$binOutputDir = "$buildOutputDir\bin\$projectName\net40\"
	New-Item $binOutputDir -Type Directory
	gci $srcDir\$projectName\bin\Release |% { Copy-Item "$srcDir\$projectName\bin\Release\$_" $binOutputDir}
}

task CreateNuGetPackages -depends CopyBuildOutput {
	$versionString = Get-Version $assemblyInfoFilePath
	$version = New-Object Version $versionString
	$packageVersion = $version.Major.ToString() + "." + $version.Minor.ToString() + "." + $version.Build.ToString()
	copy-item $srcDir\$projectName\$projectName.nuspec $buildOutputDir
	exec { .$srcDir\.nuget\nuget.exe pack $buildOutputDir\$projectName.nuspec -BasePath $buildOutputDir -o $buildOutputDir -version $packageVersion }
}