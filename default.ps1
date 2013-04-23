properties {
	$ciNumber = $null
	$publish = $false


    $base_dir = resolve-path .
    $build_dir = "$base_dir\build"
    $packageTemp_dir = "$build_dir\prePackage"
	$sln = "$base_dir\Glimpse.NLog.sln"

	$mspec = "$(ls $base_dir\packages\Machine.Specifications.* | select -last 1)" + "\tools\mspec-clr4.exe"
	$nuget = "$base_dir\.nuget\nuget.exe"
}

#tasks -------------------------------------------------------------------------------------------------------------

task default -depends pack

task clean {
    Delete-Directory $build_dir
	Delete-Directory $base_dir\**\bin
	Delete-Directory $base_dir\**\obj
}

task compile -depends clean {
    exec { msbuild $sln /p:Configuration=Release /nologo /verbosity:minimal }
}

task test -depends compile {
	exec { & $mspec $base_dir\Glimpse.NLog.Tests\bin\Release\Glimpse.NLog.Tests.dll }
	exec { & $mspec $base_dir\Glimpse.NLog.Net40.Tests\bin\Release\Glimpse.NLog.Net40.Tests.dll }
}

task prePack -depends test {
	Make-Directory $packageTemp_dir
	Make-Directory $packageTemp_dir\lib\net45
	Make-Directory $packageTemp_dir\lib\net40

	copy $base_dir\NuSpec\Glimpse.NLog.nuspec $packageTemp_dir
    copy $base_dir\Glimpse.NLog\bin\Release\Glimpse.Nlog.* $packageTemp_dir\lib\net45\
    copy $base_dir\Glimpse.NLog.Net40\bin\Release\Glimpse.Nlog.* $packageTemp_dir\lib\net40\
}

task pack -depends prePack {

	if($ciNumber) { $preVersion = "ci{0:00000}" -f $ciNumber }
	else { $preVersion = "local" }

	$version = "$(Get-NuSpecVersion("$packageTemp_dir\Glimpse.NLog.nuspec"))-$preVersion"

	exec { & $nuget pack $packageTemp_dir\Glimpse.NLog.nuspec -Symbols -OutputDirectory $build_dir -Version $version }
}

task ci -depends pack {
	if(!$ciNumber) {
		throw "Need ciNumber for publishPreRelease"
	}

	if($publish) { "PUBLISHING" } else { "Dummy publishing run..." }

	$packages = ls $build_dir\* -Include *.nupkg -Exclude *.symbols.nupkg
	foreach($package in $packages){
		"Executing: nuget.exe push $package -src http://www.myget.org/F/rholiver/"
		if($publish) { exec { & $nuget push $package -src http://www.myget.org/F/rholiver/ } }
	}
	
	$symbols = ls $build_dir\*.symbols.nupkg
	foreach($symbol in $symbols){
		"Executing: nuget.exe push $symbol -src http://nuget.gw.symbolsource.org/MyGet/rholiver"
		if($publish) { exec { & $nuget push $symbol -src http://nuget.gw.symbolsource.org/MyGet/rholiver } }
	}
}



function Get-NuSpecVersion($path)
{
	$xml = [xml]$(get-content $path)
	return $xml.package.metadata.version
}

function Delete-Directory($path)
{
    if (test-path $path) {
		rd $path -recurse -force | out-null
	}
}

function Make-Directory($path)
{
    mkdir $path | out-null
}