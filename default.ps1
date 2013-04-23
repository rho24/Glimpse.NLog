properties {
    $base_dir = resolve-path .
    $build_dir = "$base_dir\build"
    $packageTemp_dir = "$build_dir\prePackage"
	$sln = "$base_dir\Glimpse.NLog.sln"


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
	exec { & $mspec40 $base_dir\Glimpse.NLog.Net40\bin\Release\Glimpse.NLog.Net40.dll }
}

task prePack -depends compile {
	Make-Directory $packageTemp_dir
	Make-Directory $packageTemp_dir\lib\net45
	Make-Directory $packageTemp_dir\lib\net40

	copy $base_dir\NuSpec\Glimpse.NLog.nuspec $packageTemp_dir
    copy $base_dir\Glimpse.NLog\bin\Release\Glimpse.Nlog.* $packageTemp_dir\lib\net45\
    copy $base_dir\Glimpse.NLog.Net40\bin\Release\Glimpse.Nlog.* $packageTemp_dir\lib\net40\
}

task pack -depends prePack{
	exec { & $base_dir\.nuget\nuget.exe pack $packageTemp_dir\Glimpse.NLog.nuspec -Symbols -OutputDirectory $build_dir }
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