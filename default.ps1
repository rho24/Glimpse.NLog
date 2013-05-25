properties {
  $ciNumber = $null
  $publish = $false
  
  $release = $false
  
  $version = $null
  $packageVersion = $null
  
  
  $base_dir = resolve-path .
  $build_dir = "$base_dir\build"
  $packageTemp_dir = "$build_dir\prePackage"
  $sln = "$base_dir\Glimpse.NLog.sln"

  $assemblyInfo = "$base_dir\Glimpse.NLog\Properties\AssemblyInfo.cs"
  $tempAssemblyInfo = "$assemblyInfo.tmp"
  
  
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

task preCompile -depends clean {
  
  setVersions
  
  $version = version
  $packageVersion = packageVersion
  
  copy $assemblyInfo $tempAssemblyInfo
  
  (gc $assemblyInfo -encoding utf8) -replace """1.0.0.0""" , """$version""" -replace """1.0.0.0-package""" , """$packageVersion""" | sc $assemblyInfo -encoding utf8
}

task compile -depends preCompile {
  exec { msbuild $sln /p:Configuration=Release /nologo /verbosity:minimal }
}

task postCompile -depends compile {
  rm $assemblyInfo
  copy $tempAssemblyInfo $assemblyInfo
  rm $tempAssemblyInfo
}

task test -depends postCompile {
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
  $packageVersion = packageVersion
  exec { & $nuget pack $packageTemp_dir\Glimpse.NLog.nuspec -Symbols -OutputDirectory $build_dir -Version $packageVersion }
}

task publish -depends pack {
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

task ci {
  if(!$ciNumber) {
    throw "Need ciNumber for publishPreRelease"
  }

  Invoke-Task "publish"
}

task release {
  $release = $true
  Invoke-Task "publish"
}

function version(){
  return $script:version
}

function packageVersion(){
  return $script:packageVersion
}

function setVersions()
{
  $script:version = Get-NuSpecVersion("$base_dir\NuSpec\Glimpse.NLog.nuspec")
  $script:packageVersion = $script:version
  
  if(!$release) {
    if($ciNumber) { $preVersion = "CI{0:00000}" -f $ciNumber }
    else { $preVersion = "local" }
    $script:packageVersion = "$script:version-$preVersion"
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