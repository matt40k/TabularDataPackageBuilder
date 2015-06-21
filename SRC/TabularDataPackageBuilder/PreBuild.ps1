# Only runs on the AppVeyor
if ($Env:APPVEYOR)
{
# Get the latest License json file from OpenDefinition.org
	$scriptpath = Split-Path $MyInvocation.MyCommand.Path
	$url = "http://licenses.opendefinition.org/licenses/groups/all.json"
	
	Invoke-WebRequest $url -OutFile $scriptpath\License.json
	
	$content = (Get-Content $scriptpath\License.json)
	$content = $content| select -Skip 1
	$content = $content.Replace("    ", "`t`t`t")
	$content = $content.Replace(""": {", """,")
	$content = $content.Replace("  """, "`t{`n`t`t""License"": {`n`t`t`t""name"": """)
	$content = $content.Replace("  },", "`t`t}`n`t},")
	$content = $content.Replace("  ", "`t")
	$content = "[" + $content + "]"
	$content | Set-Content $scriptpath\License.json
}